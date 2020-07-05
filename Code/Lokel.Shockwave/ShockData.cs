/*
 * (c) Copyright 2020 Lokel Digital Pty Ltd.
 * https://www.lokeldigital.com
 * 
 * LokelPackage can be used under the Creative Commons License AU by Attribution
 * https://creativecommons.org/licenses/by/3.0/au/legalcode
 */

using Unity.Mathematics;

using Lokel.Util;

namespace Lokel.Shockwave
{

    public static class ShockData
    {
        public static float Height(this float4 value) => value.z;
        public static float Angle(this float4 value) => value.w;
        public static float TimeInSeconds(this float4 value) => value.Angle() / math.PI;
        public static float2 Position(this float4 value) => new float2(value.x, value.y);

        public static (float2 position, float height, float angle) ToParts(this float4 value)
            => (value.Position(), value.Height(), value.Angle());

        public static float4 FromParts(float2 pos, float height, float angle)
            => new float4(pos.x, pos.y, height, angle);

        public static float Distance(float2 pos1, float2 pos2)
            => math.sqrt(FloatExt.square(pos1 - pos2).SumXY());

        public static float DiminishingFactor(ShockMasterParams master, float angle)
            => math.pow(1 - master.DecayRatePerSecond, angle);
    }

}
