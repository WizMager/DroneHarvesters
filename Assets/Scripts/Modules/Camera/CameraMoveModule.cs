using System;
using Core.Interfaces;
using Db.Camera;
using Services.Input;
using UnityEngine;

namespace Modules.Camera
{
    public class CameraMoveModule : IUpdatable, IDisposable
    {
        private readonly UnityEngine.Camera _camera;
        private readonly IInputService _inputService;
        private readonly CameraData _cameraData;
        private readonly MinimapController _minimapController;
        
        private Transform _targetToFollow;
        private Vector3 _basePosition;
        private Quaternion _baseRotation;
        private bool _isFollowing;

        public CameraMoveModule(
            UnityEngine.Camera camera, 
            IInputService inputService, 
            CameraData cameraData,
            MinimapController minimapController
        )
        {
            _camera = camera;
            _inputService = inputService;
            _cameraData = cameraData;
            _minimapController = minimapController;
            _basePosition = camera.transform.position;
            _baseRotation = camera.transform.rotation;
            
            _inputService.OnCancelPressed += StopFollowing;
            _minimapController.OnUnitSelected += OnUnitSelected;
        }
        
        private void StopFollowing()
        {
            if (!_isFollowing)
                return;
                
            _isFollowing = false;
            _targetToFollow = null;
            
            var currentPos = _camera.transform.position;
            var returnPosition = new Vector3(currentPos.x, _basePosition.y, currentPos.z);
            _camera.transform.position = returnPosition;
            _camera.transform.rotation = _baseRotation;
        }
        
        private void OnUnitSelected(Transform unit)
        {
            if (unit != null)
            {
                StartFollowing(unit);
            }
        }

        public void Update()
        {
            if (_isFollowing && _targetToFollow != null)
            {
                FollowTarget();
                
                return;
            }
            
            if (!_inputService.IsMoved)
                return;

            var position = _camera.transform.position;
            position += new Vector3(-_inputService.MoveDirection.y, 0, _inputService.MoveDirection.x) * Time.deltaTime * _cameraData.MoveSpeed;
            _camera.transform.position = position;
            _basePosition = position;
            _baseRotation = _camera.transform.rotation;
        }

        private void StartFollowing(Transform target)
        {
            _basePosition = _camera.transform.position;
            _baseRotation = _camera.transform.rotation;
            _targetToFollow = target;
            _isFollowing = true;
        }
        
        private void FollowTarget()
        {
            if (_targetToFollow == null)
            {
                StopFollowing();
                return;
            }
            
            var droneForward = _targetToFollow.forward;
            var dronePosition = _targetToFollow.position;
            
            var offsetDirection = -droneForward;
            var targetPosition = dronePosition + offsetDirection * Mathf.Abs(_cameraData.FollowOffset.z) + Vector3.up * _cameraData.FollowOffset.y;
            
            var followSpeed = _cameraData.MoveSpeed * 2f;
            _camera.transform.position = Vector3.Lerp(_camera.transform.position, targetPosition, Time.deltaTime * followSpeed);
            
            var directionToDrone = (dronePosition - _camera.transform.position).normalized;
            var targetRotation = Quaternion.LookRotation(directionToDrone);
            _camera.transform.rotation = Quaternion.Lerp(_camera.transform.rotation, targetRotation, Time.deltaTime * followSpeed);
        }

        public void Dispose()
        {
            _inputService.OnCancelPressed -= StopFollowing;
            _minimapController.OnUnitSelected -= OnUnitSelected;
        }
    }
}