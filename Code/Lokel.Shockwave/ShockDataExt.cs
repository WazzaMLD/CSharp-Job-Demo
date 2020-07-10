/*
 * (c) Copyright 2020 Lokel Digital Pty Ltd.
 * https://www.lokeldigital.com
 * 
 * This Lokel package can be used under the Creative Commons License AU by Attribution
 * https://creativecommons.org/licenses/by/3.0/au/legalcode
 */


using Unity.Mathematics;

using Lokel.Util;

namespace Lokel.Shockwave
{

    public static class ShockDataExt
    {
        public static (float2 position, float height, float angle) ToParts(
            this ShockwaveData value
        )   => (value.Position, value.Height, value.Angle);

        public static ShockwaveData FromParts(float2 pos, float height, float waveTime)
            => ShockwaveData.Create(pos, height, waveTime);

        public static float Distance(float2 pos1, float2 pos2)
            => math.sqrt(FloatExt.square(pos1 - pos2).SumXY());

        public static float DiminishingFactor(ShockMasterParams master, float waveTime)
            => math.pow(1 - master.DecayRatePerSecond, waveTime);

        public static int GetInnerLoopCountFromSize(ShockMasterParams master)
            => (int)master.Size.x;
    }

}
