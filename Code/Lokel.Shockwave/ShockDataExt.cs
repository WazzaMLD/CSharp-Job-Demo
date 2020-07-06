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

    public static class ShockDataExt
    {
        public static float Height(this ShockwaveData value) => value.Height;

        public static float CentreAdjustedHeight(this ShockwaveData value)
            => value.Height / (1 + value.NumberCentres);

        public static float Angle(this ShockwaveData value) => value.Angle;
        public static float TimeInSeconds(this ShockwaveData value) => value.Angle() / math.PI;
        public static float2 Position(this ShockwaveData value) => value.Position;

        public static (float2 position, float height, float angle) ToParts(
            this ShockwaveData value
        )   => (value.Position(), value.Height(), value.Angle());

        public static ShockwaveData FromParts(float2 pos, float height, float angle)
            => ShockwaveData.Create(pos, height, angle);

        public static float Distance(float2 pos1, float2 pos2)
            => math.sqrt(FloatExt.square(pos1 - pos2).SumXY());

        public static float DiminishingFactor(ShockMasterParams master, float angle)
            => math.pow(1 - master.DecayRatePerSecond, angle);
    }

}
