using System;
using UnityEngine;

namespace CodeBase.Components.Animation
{
    [Serializable]
    public sealed class Animation
    {
        [SerializeField] private GameObject _animateObject;

        [Header("Default Settings")]
        [SerializeField] private bool _useCurrentPosition;
        [SerializeField] private Vector3 _defaultPosition;
        [SerializeField] private bool _useCurrentRotation;
        [SerializeField] private Quaternion _defaultRotation;
        [SerializeField] private bool _useCurrentScale;
        [SerializeField] private Vector3 _defaultScale;

        [Header("Expected Settings")]
        [SerializeField] private bool _useCurrentExpectedPosition;
        [SerializeField] private Vector3 _expectedPosition;
        [SerializeField] private bool _useCurrentExpectedRotation;
        [SerializeField] private Quaternion _expectedRotation;
        [SerializeField] private bool _useCurrentExpectedScale;
        [SerializeField] private Vector3 _expectedScale;

        [SerializeField] private float _speed = 1f;

        private bool _animatePosition = false;
        private bool _animateRotation = false;
        private bool _animateScale = false;

        public void OnValidate()
        {
            if (_useCurrentPosition)
                _defaultPosition = _animateObject.transform.position;

            if (_useCurrentRotation)
                _defaultRotation = _animateObject.transform.rotation;

            if (_useCurrentScale)
                _defaultScale = _animateObject.transform.localScale;

            if (_useCurrentExpectedPosition)
                _expectedPosition = _animateObject.transform.position;

            if (_useCurrentExpectedRotation)
                _expectedRotation = _animateObject.transform.rotation;

            if (_useCurrentExpectedScale)
                _expectedScale = _animateObject.transform.localScale;
        }
        public void OnUpdate(float deltaTime)
        {
            if (_animatePosition)
            {
                _animateObject.transform.position = Vector3.Lerp(
                    _animateObject.transform.position,
                    _expectedPosition,
                    _speed * deltaTime
                );

                if (Vector3.Distance(_animateObject.transform.position, _expectedPosition) < 0.01f)
                {
                    _animateObject.transform.position = _expectedPosition;
                    _animatePosition = false;
                }
            }

            if (_animateRotation)
            {
                _animateObject.transform.rotation = Quaternion.Lerp(
                    _animateObject.transform.rotation,
                    _expectedRotation,
                    _speed * deltaTime
                );

                if (Quaternion.Angle(_animateObject.transform.rotation, _expectedRotation) < 0.1f)
                {
                    _animateObject.transform.rotation = _expectedRotation;
                    _animateRotation = false;
                }
            }

            if (_animateScale)
            {
                _animateObject.transform.localScale = Vector3.Lerp(
                    _animateObject.transform.localScale,
                    _expectedScale,
                    _speed * deltaTime
                );

                if (Vector3.Distance(_animateObject.transform.localScale, _expectedScale) < 0.01f)
                {
                    _animateObject.transform.localScale = _expectedScale;
                    _animateScale = false;
                }
            }
        }

        public void Play()
        {
            _animatePosition = _animateObject.transform.position != _expectedPosition;
            _animateRotation = _animateObject.transform.rotation != _expectedRotation;
            _animateScale = _animateObject.transform.localScale != _expectedScale;
        }

        public void ResetToDefault()
        {
            _animateObject.transform.position = _defaultPosition;
            _animateObject.transform.rotation = _defaultRotation;
            _animateObject.transform.localScale = _defaultScale;

            _animatePosition = false;
            _animateRotation = false;
            _animateScale = false;
        }
    }
}