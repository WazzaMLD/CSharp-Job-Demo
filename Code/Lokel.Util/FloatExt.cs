/*
 * (c) Copyright 2020 Lokel Digital Pty Ltd.
 * https://www.lokeldigital.com
 * 
 * LokelPackage can be used under the Creative Commons License AU by Attribution
 * https://creativecommons.org/licenses/by/3.0/au/legalcode
 */


using Lokel.Shockwave;
using Unity.Mathematics;
using static Unity.Mathematics.math;

namespace Lokel.Util
{
    public static class FloatExt
    {
        const float FALLOFF = 0.01f;
        public static bool IsWithinFalloff(this float value)
            => -FALLOFF <= value && value <= FALLOFF;
        public static float ZeroIfSmall(this float value) => IsWithinFalloff(value) ? 0f : value;

        /// <summary>Returns positive value of given float.</summary>
        /// <param name="value">supplied value - pos or neg</param>
        /// <returns>magnitude of value</returns>
        public static float PositiveValueOf(this float value)
            => math.sign(value) * value;

        public static float2 square(this float2 value) => value * value;
        public static float square(this float value) => value * value;
        public static float SumXY(this float2 value) => value.x + value.y;
    }
}
