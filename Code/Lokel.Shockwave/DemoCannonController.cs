/*
 * (c) Copyright 2020 Lokel Digital Pty Ltd.
 * https://www.lokeldigital.com
 * 
 * This Lokel package can be used under the Creative Commons License AU by Attribution
 * https://creativecommons.org/licenses/by/3.0/au/legalcode
 */

using UnityEngine;

using Lokel.Util;

namespace Lokel.Shockwave
{

    [AddComponentMenu("Lokel/Demo Cannon Base Controller")]
    [RequireComponent(typeof(AudioSource))]
    public class DemoCannonController : MonoBehaviour
    {
        [SerializeField] private Transform _CannonBallSpawnPoint = null;
        [SerializeField] private Transform _TurretBase = null;

        [SerializeField] private GameObject _CannonBallPrefab = null;
        [SerializeField] private float _FireForcePower = 50f;

        [Tooltip("Y axis cannon rotation delta in degrees")]
        [SerializeField] private float _BaseYawIncrementDegrees = 5f;
        [SerializeField] private float _BaseYawMinDegrees = -10f;
        [SerializeField] private float _BaseYawMaxDegrees = 95f;

        [Tooltip("X axis turret rotation delta in degrees")]
        [SerializeField] private float _TurretPitchIncrementDegrees = 5f;
        [SerializeField] private float _TurretPitchMinDegrees = 15f;
        [SerializeField] private float _TurretPitchMaxDegrees = 75f;

        private Transform _CannonBase;
        private AudioSource _Audio;
        private bool _IsReady;

        public void OnBaseYawClockwise()
        {
            RotateBaseIfValid(_BaseYawIncrementDegrees);
        }

        public void OnBaseYawAntilockwise()
        {
            RotateBaseIfValid(- _BaseYawIncrementDegrees);
        }

        public void OnTurretPitchClockwise()
        {
            if (_IsReady) RotateTurretIfValid( _TurretPitchIncrementDegrees);
        }

        public void OnTurretPitchAnticlockwise()
        {
            if (_IsReady) RotateTurretIfValid(- _TurretPitchIncrementDegrees);
        }

        public void OnFireCannon()
        {
            if (_IsReady)
            {
                GameObject cannonBall = GameObject.Instantiate(
                    _CannonBallPrefab, _CannonBallSpawnPoint.position, Quaternion.identity
                );
                SetCannonBallInMotion(cannonBall);
                MakeSoundIfAvailable();
            }
        }

        private void MakeSoundIfAvailable()
        {
            if (_Audio.clip != null) _Audio.Play();
        }

        private void RotateBaseIfValid(float angleChange)
        {
            var eulerAngles = _CannonBase.rotation.eulerAngles;
            float newAngle = (eulerAngles.y + angleChange);
            if (Angles.IsWithinDegreeLimits(newAngle, _BaseYawMinDegrees, _BaseYawMaxDegrees))
            {
                _CannonBase.Rotate(Vector3.up, angleChange);
            }
        }

        private void RotateTurretIfValid(float angleChange)
        {
            if (_IsReady)
            {
                var eulerAngles = _TurretBase.rotation.eulerAngles;
                float newAngle = (eulerAngles.x + angleChange);
                if (
                    Angles.IsWithinDegreeLimits(
                        newAngle,
                        _TurretPitchMinDegrees,
                        _TurretPitchMaxDegrees
                    ))
                {
                    _TurretBase.Rotate(Vector3.right, angleChange);
                }
            }
        }

        private void SetCannonBallInMotion(GameObject ball)
        {
            var rigidBody = ball.GetComponent<Rigidbody>();
            Vector3 direction = _CannonBallSpawnPoint.position - _TurretBase.position;
            if (rigidBody == null)
            {
                rigidBody = ball.AddComponent<Rigidbody>();
            }
            rigidBody.AddForce(direction * _FireForcePower);
        }

        private void Awake()
        {
            _CannonBase = GetComponent<Transform>();
            _Audio = GetComponent<AudioSource>();
            CheckMandatoryInspectorFields();
        }

        private void CheckMandatoryInspectorFields()
        {
            const bool ASSUME_TRUE_AND_INVALIDATE_WHEN_SOMETHING_MISSING = true;

            _IsReady = ASSUME_TRUE_AND_INVALIDATE_WHEN_SOMETHING_MISSING;

            if (_CannonBallSpawnPoint == null)
                ErrorOutAndMakeUnready("Need the reference to the Empty game object where cannon balls should spawn.");

            if (_TurretBase == null)
                ErrorOutAndMakeUnready("Need reference to object at bottom axle of turrent.");

            if (_CannonBallPrefab == null)
                ErrorOutAndMakeUnready("Configure a Cannon Ball Prefab.");
        }

        private void ErrorOutAndMakeUnready(string msg)
        {
            Debug.LogError(msg);
            _IsReady = false;
        }
    }

}
