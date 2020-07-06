/*
 * (c) Copyright 2020 Lokel Digital Pty Ltd.
 * https://www.lokeldigital.com
 * 
 * LokelPackage can be used under the Creative Commons License AU by Attribution
 * https://creativecommons.org/licenses/by/3.0/au/legalcode
 */


namespace Lokel.Shockwave
{
    using SerializableAttribute = System.SerializableAttribute;
    using TooltipAttribute = UnityEngine.TooltipAttribute;
    using RangeAttribute = UnityEngine.RangeAttribute;

    using Unity.Mathematics;
    using UnityEngine;

    /// <summary>Master control variables to pass to the jobs and populate in Editor</summary>
    [Serializable]
    public struct ShockMasterParams 
    {
        [Tooltip("Cutoff Threshold - how big in seconds")]
        public float Cutoff;

        [Tooltip("Fraction drop each second")]
        [Range(0,1f)]
        public float DecayRatePerSecond;

        [Tooltip("Influence factor of one link to another before hitting limit")]
        public float InfluenceRadius;

        [Tooltip("Max height")]
        public float HeightFactor;

        [Tooltip("Size of the shockwave surface")]
        public int2 Size;

        public static ShockMasterParams Defaults()
        {
            return new ShockMasterParams()
            {
                Cutoff = 12f,
                DecayRatePerSecond = 0.4f,
                InfluenceRadius = 2f,
                HeightFactor = 8f,
                Size = new int2(20,20)
            };
        }
    }
}
