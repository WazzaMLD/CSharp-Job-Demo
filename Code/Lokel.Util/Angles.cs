/*
 * (c) Copyright 2020 Lokel Digital Pty Ltd.
 * https://www.lokeldigital.com
 * 
 * This Lokel package can be used under the Creative Commons License AU by Attribution
 * https://creativecommons.org/licenses/by/3.0/au/legalcode
 */


namespace Lokel.Util
{

    /// <summary>An angle handling multi-tool</summary>
    public static class Angles
    {
        public static bool IsWithinDegreeLimits(float angle, float min, float max)
        {
            float safeMin = SafeDegrees(min);
            float safeMax = SafeDegrees(max);
            float safeAngle = SafeDegrees(angle);
            return safeMin <= safeAngle && safeAngle <= safeMax;
        }

        public static float SafeDegrees(float inAngle)
        {
            const float WRAP_POINT = 360f; // degrees.
            return (inAngle % WRAP_POINT) + WRAP_POINT;
        }

    }

}
