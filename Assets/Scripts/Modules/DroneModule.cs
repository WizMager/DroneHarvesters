using Core.Interfaces;
using UnityEngine;
using UnityEngine.Pool;
using Views;

namespace Modules
{
    public class DroneModule : IStartable, IUpdatable
    {
        private readonly GameObject _dronePrefab;
        private readonly BaseView _redBase;
        private readonly BaseView _blueBase;
        
        private int _redDroneCount = 3;
        private int _blueDroneCount = 3;

        private readonly ObjectPool<DroneView> _dronePool;
        
        public DroneModule(GameObject dronePrefab, BaseView redBase, BaseView blueBase)
        {
            _dronePrefab = dronePrefab;
            _redBase = redBase;
            _blueBase = blueBase;
            
            _dronePool = new ObjectPool<DroneView>(CreateDrone, OnGetDrone, OnReleaseDrone);
        }
        
#region Object Pool Methodos

        private DroneView CreateDrone()
        {
            var droneView = Object.Instantiate(_dronePrefab).GetComponent<DroneView>();
            
            return droneView;
        }

        private void OnGetDrone(DroneView droneView)
        {
            droneView.gameObject.SetActive(true);
        }
        
        private void OnReleaseDrone(DroneView droneView)
        {
            droneView.gameObject.SetActive(false);
        }

#endregion
        
        public void Start()
        {
            for (var i = 0; i < _redDroneCount; i++)
            {
                var drone = _dronePool.Get();
                drone.homeBase = _redBase;
            }
            
            for (var i = 0; i < _blueDroneCount; i++)
            {
                var drone = _dronePool.Get();
                drone.homeBase = _blueBase;
            }
        }

        public void Update()
        {
            
        }
    }
}