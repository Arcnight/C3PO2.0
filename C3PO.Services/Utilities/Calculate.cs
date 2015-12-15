using System;

namespace C3PO.Utilities
{
    public static class Calculate
    {
        public static long GetDecimalDegrees(string direction, long degrees, long minutes, long seconds)
        {
            direction = (direction ?? string.Empty).ToLower();

            var val = degrees + (minutes / 60) + (seconds / 3600);

            if (direction == "s" || direction == "w") val *= -1;

            return val;
        }
    }
}
