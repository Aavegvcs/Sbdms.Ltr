namespace Sbdms.Ltr.Core.Common.Helper;

// Central "now" for every stored/displayed timestamp in the app (CreatedOn, ModifiedOn,
// BookedOn, RecordedOn, OTP/refresh-token expiry windows, etc.) — IST instead of UTC, per
// product requirement. Implemented as a fixed +5:30 offset rather than a TimeZoneInfo/tzdata
// lookup: India has had one unchanging UTC+5:30 offset (no DST) for decades, so this is
// correct on any host — Windows, bare Linux, a minimal container — regardless of whether it
// has timezone data installed at all.
public static class IndianStandardTime
{
    private static readonly TimeSpan Offset = new(5, 30, 0);

    public static DateTime Now => DateTime.UtcNow.Add(Offset);
}
