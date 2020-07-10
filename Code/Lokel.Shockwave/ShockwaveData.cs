/*
 * (c) Copyright 2020 Lokel Digital Pty Ltd.
 * https://www.lokeldigital.com
 * 
 * This Lokel package can be used under the Creative Commons License AU by Attribution
 * https://creativecommons.org/licenses/by/3.0/au/legalcode
 */


using Unity.Mathematics;

namespace Lokel.Shockwave
{

    public struct ShockwaveData
    {
        public float2 Position;
        public float Height;
        public float WaveTime;
        public int CentreIndex;
        public float AdditionalHeight;

        public float Angle { get => (WaveTime + 0.5f) * math.PI; }

        public static ShockwaveData Create(
            float2 position,
            float height,
            float waveTime = 0
        )
        {
            return new ShockwaveData()
            {
                Position = position,
                Height = height,
                WaveTime = waveTime,
                CentreIndex = 0,
                AdditionalHeight = 0
            };
        }

        public static ShockwaveData Create(
            float x, float y,
            float height,
            float waveTime
        ) => Create(new float2(x, y), height, waveTime);
    }

}
