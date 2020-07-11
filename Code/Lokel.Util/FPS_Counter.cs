/*
 * (c) Copyright 2020 Lokel Digital Pty Ltd.
 * https://www.lokeldigital.com
 * 
 * This Lokel package can be used under the Creative Commons License AU by Attribution
 * https://creativecommons.org/licenses/by/3.0/au/legalcode
 */


using UnityEngine;
using UnityEngine.UI;

namespace Lokel.Util
{

    [RequireComponent(typeof(Text))]
    [AddComponentMenu("Lokel Utils/FPS Counter")]

    public class FPS_Counter : MonoBehaviour
    {
        private Text _FpsBox;
        private float _MaxFps;
        private float _MinFps;
        private float _ActualFps;
        private bool _ShowMinMax;

        [Tooltip("How many frames to wait before measuring Min & Max")]
        [SerializeField] private int _InitMinMaxAtFrame = 5;

        [Tooltip("How many frames before updating text (more is more accurate)")]
        [SerializeField] private int _FramesBetweenUpdate = 5;

        [Tooltip("Correction factor so runtime measure = Statistic Window")]
        [SerializeField] private float _FpsMultiplier = 3.3f;

        private void Awake()
        {
            _FpsBox = GetComponent<Text>();
            _ShowMinMax = false;
            _MinFps = 2000f;
            _MaxFps = 0f;
        }

        private void Update()
        {
            CheckMinMaxStatus();
            _ActualFps = _FpsMultiplier * 1.0f / Time.deltaTime;
            if (_ShowMinMax) UpdateMinMax();
            if (IsDisplayUpdateFrame()) UpdateDisplay();
        }

        private bool IsDisplayUpdateFrame() => (Time.frameCount % _FramesBetweenUpdate) == 0;

        private void CheckMinMaxStatus() => _ShowMinMax = Time.frameCount > _InitMinMaxAtFrame;

        private void UpdateMinMax()
        {
            if (_ActualFps > _MaxFps) _MaxFps = _ActualFps;
            if (_ActualFps < _MinFps) _MinFps = _ActualFps;
        }

        private void UpdateDisplay()
        {
            string message;
            message = _ShowMinMax
                ? $"Approx FPS: {_ActualFps,6:F0} Min: {_MinFps,3:F0} Max: {_MaxFps,3:F0}"
                : $"Approx FPS: {_ActualFps,6:F0} Min: --- Max: ---";
            _FpsBox.text = message;
        }
    }

}
