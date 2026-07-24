using Sbdms.Ltr.Contracts.Vehicle;
using Sbdms.Ltr.Core.Common.Helper;
using Sbdms.Ltr.Core.Domain;
using Sbdms.Ltr.Core.Interface;
using Sbdms.SharedLibrary.ApiResponse;
using Sbdms.SharedLibrary.Common;
using Sbdms.SharedLibrary.ResultPattern;

namespace Sbdms.Ltr.Core.Feature.Vehicles;

public class BulkUpdateVehicleLocationHandler
{
    private readonly IVehicleRepository _vehicleRepository;
    private readonly IVehicleLocationRepository _locationRepository;
    private readonly IVehicleLocationHistoryRepository _historyRepository;
    private readonly IUnitOfWork _unitOfWork;

    private const int DEFAULT_VENDOR_ID = 1;
    private const int DEFAULT_VEHICLE_TYPE_CODE = 1;
    private const int DEFAULT_STATUS_ID = 1;
    private const string DEFAULT_VEHICLE_COMPANY = "Unknown";
    private const string DEFAULT_MODAL = "Unknown";

    public BulkUpdateVehicleLocationHandler(
        IVehicleRepository vehicleRepository,
        IVehicleLocationRepository locationRepository,
        IVehicleLocationHistoryRepository historyRepository,
        IUnitOfWork unitOfWork)
    {
        _vehicleRepository = vehicleRepository;
        _locationRepository = locationRepository;
        _historyRepository = historyRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<CoreResponse<BulkVehicleLocationResponse>>> HandleAsync(BulkUpdateVehicleLocationRequest request)
    {
        // 1. Validate input
        if (request?.VehicleLocations == null || !request.VehicleLocations.Any())
        {
            return Error.Failure("No vehicle locations provided.");
        }

        var now = IndianStandardTime.Now;
        var vehicleNumbers = request.VehicleLocations.Select(v => v.VehicleNumber).Distinct().ToList();

        // 2. Fetch existing vehicles
        var existingVehicles = await _vehicleRepository.GetAllByAsync(v => vehicleNumbers.Contains(v.VehicleNumber));
        var vehicleDict = existingVehicles.ToDictionary(v => v.VehicleNumber, v => v);

        var successCount = 0;
        var errorCount = 0;
        var errors = new List<string>();
        var createdVehicles = new List<VehicleCreationInfo>();

        // 3. Process each location
        foreach (var locationUpdate in request.VehicleLocations)
        {
            try
            {
                // 3.1 Validate coordinates
                if (!IsValidCoordinate(locationUpdate.Latitude, locationUpdate.Longitude, out string coordError))
                {
                    errorCount++;
                    errors.Add($"Vehicle '{locationUpdate.VehicleNumber}': {coordError}");
                    continue;
                }

                // 3.2 Get or create vehicle
                if (!vehicleDict.TryGetValue(locationUpdate.VehicleNumber, out var vehicle))
                {
                    // Create new vehicle
                    var qrCode = GenerateQrCode(locationUpdate.VehicleNumber);
                    vehicle = Vehicle.Create(
                        vendorId: DEFAULT_VENDOR_ID,
                        vehicleTypeCode: DEFAULT_VEHICLE_TYPE_CODE,
                        driverId: null,
                        currentStatusId: DEFAULT_STATUS_ID,
                        vehicleNumber: locationUpdate.VehicleNumber,
                        vehicleCompany: DEFAULT_VEHICLE_COMPANY,
                        modal: DEFAULT_MODAL,
                        qrUniqueCode: qrCode,
                        createdOn: now
                    );

                    var addResult = await _vehicleRepository.AddAsync(vehicle);
                    if (addResult.IsError)
                    {
                        errorCount++;
                        errors.Add($"Vehicle '{locationUpdate.VehicleNumber}': Failed to create - {string.Join(", ", addResult.Errors)}");
                        continue;
                    }

                    // ⭐ CRITICAL: Save immediately to generate the real database Id
                    await _unitOfWork.SaveChangesAsync();

                    vehicleDict[locationUpdate.VehicleNumber] = vehicle;
                    createdVehicles.Add(new VehicleCreationInfo(
                        locationUpdate.VehicleNumber,
                        vehicle.Id,
                        "Vehicle created automatically from GPS data with default values"
                    ));
                }

                // 3.3 Update current location (now vehicle.Id is guaranteed valid)
                var currentLocation = await _locationRepository.GetByVehicleIdAsync(vehicle.Id);

                if (currentLocation is null)
                {
                    var newLocation = VehicleLocation.Create(
                        vehicle.Id,
                        locationUpdate.Latitude,
                        locationUpdate.Longitude,
                        now
                    );
                    _locationRepository.Add(newLocation);
                }
                else
                {
                    currentLocation.UpdatePosition(
                        locationUpdate.Latitude,
                        locationUpdate.Longitude,
                        now
                    );
                    _locationRepository.Update(currentLocation);
                }

                // 3.4 Add history record
                var history = VehicleLocationHistory.Create(
                    vehicle.Id,
                    locationUpdate.Latitude,
                    locationUpdate.Longitude,
                    now
                );
                _historyRepository.Add(history);

                successCount++;
            }
            catch (Exception ex)
            {
                errorCount++;
                errors.Add($"Vehicle '{locationUpdate.VehicleNumber}': {ex.Message}");
            }
        }

        // 4. Save all location and history changes
        await _unitOfWork.SaveChangesAsync();

        // 5. Build response
        var response = new BulkVehicleLocationResponse(successCount, errorCount, errors, createdVehicles);
        var message = errorCount > 0
            ? $"Processed {successCount + errorCount} locations. Success: {successCount}, Failed: {errorCount}. Created {createdVehicles.Count} new vehicles."
            : $"Successfully updated {successCount} vehicle locations. Created {createdVehicles.Count} new vehicles.";

        return new CoreResponse<BulkVehicleLocationResponse>(response, true, message);
    }

    private bool IsValidCoordinate(decimal latitude, decimal longitude, out string error)
    {
        if (latitude < -90 || latitude > 90)
        {
            error = $"Invalid latitude {latitude}. Must be between -90 and 90.";
            return false;
        }

        if (longitude < -180 || longitude > 180)
        {
            error = $"Invalid longitude {longitude}. Must be between -180 and 180.";
            return false;
        }

        error = null!;
        return true;
    }

    private string GenerateQrCode(string vehicleNumber)
    {
        var timestamp = DateTime.UtcNow.Ticks.ToString();
        var combined = $"{vehicleNumber}_{timestamp}";

        using var sha256 = System.Security.Cryptography.SHA256.Create();
        var hash = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(combined));
        return Convert.ToBase64String(hash)
            .Replace("/", "_")
            .Replace("+", "-")
            .Substring(0, 32);
    }
}