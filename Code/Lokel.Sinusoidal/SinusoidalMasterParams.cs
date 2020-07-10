/*
 * (c) Copyright 2020 Lokel Digital Pty Ltd.
 * https://www.lokeldigital.com
 * 
 * This Lokel package can be used under the Creative Commons License AU by Attribution
 * https://creativecommons.org/licenses/by/3.0/au/legalcode
 */


namespace Lokel.Sinusoidal
{
    using SerializableAttribute = System.SerializableAttribute;
    using TooltipAttribute = UnityEngine.TooltipAttribute;
    using RangeAttribute = UnityEngine.RangeAttribute;

    using Unity.Mathematics;

    /// <summary>Master control variables to pass to the jobs and populate in Editor</summary>
    [Serializable]
    public struct SinusoidalMasterParams 
    {
        [Tooltip("How fast will sinusoid move?")]
        [Range(0f, 5f)]
        public float Speed;

        [Tooltip("Angular offset (rads) for each index")]
        [Range(0f, 1f)]
        public float OffsetFactor;


        [Tooltip("Size of the shockwave surface")]
        public int2 Size;

        public static SinusoidalMasterParams Defaults()
        {
            return new SinusoidalMasterParams()
            {
                OffsetFactor = 0.2f,
                Speed = 0.2f,
                Size = new int2(5,5)
            };
        }
    }
}
