using Core.Interfaces;
using Db.Camera;
using Services.Input;
using UnityEngine;

namespace Modules.Camera
{
    public class CameraMoveModule : IUpdatable
    {
        private readonly UnityEngine.Camera _camera;
        private readonly IInputService _inputService;
        private readonly CameraData _cameraData;

        public CameraMoveModule(
            UnityEngine.Camera camera, 
            IInputService inputService, 
            CameraData cameraData
        )
        {
            _camera = camera;
            _inputService = inputService;
            _cameraData = cameraData;
        }

        public void Update()
        {
            if (!_inputService.IsMoved)
                return;

            var position = _camera.transform.position;
            position += new Vector3(-_inputService.MoveDirection.y, 0, _inputService.MoveDirection.x) * Time.deltaTime * _cameraData.MoveSpeed;
            _camera.transform.position = position;
        }

    }
}