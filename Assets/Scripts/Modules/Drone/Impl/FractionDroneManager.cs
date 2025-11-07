using System;
using System.Collections.Generic;
using Db.Drone;
using UnityEngine;
using UnityEngine.Pool;
using Utils;
using Views;
using Object = UnityEngine.Object;

namespace Modules.Drone.Impl
{
    public class FractionDroneManager
    {
        private readonly EFractionName _fraction;
        private readonly BaseView _base;
        private readonly ObjectPool<DroneView> _pool;
        private readonly List<DroneView> _activeDrones = new();
        private readonly List<IDroneController> _controllers = new();
        private readonly List<DroneView> _allDrones = new();

        private int _droneCount;
        private float _currentSpeed;
        private bool _currentPathEnabled;
        
        public FractionDroneManager(
            EFractionName fraction,
            BaseView baseView,
            GameObject dronePrefab,
            IReadOnlyList<ResourceView> freeResources,
            DroneData droneData,
            UnityEngine.Camera camera,
            int initialDroneCount,
            float initialSpeed,
            bool initialPathEnabled,
            Action<ResourceView> onHarvest,
            Action<EFractionName> onUnload,
            MinimapController minimapController
        )
        {
            _fraction = fraction;
            _base = baseView;
            _droneCount = initialDroneCount;
            _currentSpeed = initialSpeed;
            _currentPathEnabled = initialPathEnabled;
            
            _pool = new ObjectPool<DroneView>(
                () => CreateDrone(dronePrefab, freeResources, droneData, camera, onHarvest, onUnload),
                drone => OnGetDrone(drone, minimapController),
                drone => OnReleaseDrone(drone, minimapController)
            );
        }

#region Object Pool Methodos
        
        private DroneView CreateDrone(
            GameObject prefab,
            IReadOnlyList<ResourceView> freeResources,
            DroneData droneData,
            UnityEngine.Camera camera,
            Action<ResourceView> onHarvest,
            Action<EFractionName> onUnload
        )
        {
            var droneView = Object.Instantiate(prefab).GetComponent<DroneView>();
            
            var controller = new DroneController(droneView, freeResources, droneData);
            droneView.Initialize(controller, _fraction, camera);
            controller.Initialize(_base);
            controller.OnHarvestResource += onHarvest;
            controller.OnResourceUnload += onUnload;
            
            _controllers.Add(controller);
            _allDrones.Add(droneView);
            
            return droneView;
        }
        
        private void OnGetDrone(DroneView drone, MinimapController minimap)
        {
            if (drone == null || drone.gameObject == null)
            {
                return;
            }
            
            minimap.RegisterUnit(drone.transform, _fraction);
            _activeDrones.Add(drone);
            drone.SetDroneSpeed(_currentSpeed);
            drone.SetIsDrawPath(_currentPathEnabled);
            drone.gameObject.SetActive(true);
        }
        
        private void OnReleaseDrone(DroneView drone, MinimapController minimap)
        {
            if (drone == null || drone.gameObject == null)
            {
                return;
            }
            
            minimap.UnregisterUnit(drone.transform, _fraction);
            _activeDrones.Remove(drone);
            drone.gameObject.SetActive(false);
        }
        
#endregion
        
        public void SetDroneCount(int newCount)
        {
            var changeCount = newCount - _droneCount;
            
            switch (changeCount)
            {
                case < 0:
                {
                    for (var i = 0; i < Math.Abs(changeCount); i++)
                    {
                        if (_activeDrones.Count > 0)
                        {
                            _pool.Release(_activeDrones[^1]);
                        }
                    }

                    break;
                }
                case > 0:
                {
                    for (var i = 0; i < changeCount; i++)
                    {
                        var drone = _pool.Get();
                        drone.DroneController.Start();
                    }

                    break;
                }
            }
            
            _droneCount = newCount;
        }
        
        public void InitialStartDrones()
        {
            for (var i = 0; i < _droneCount; i++)
            {
                var drone = _pool.Get();
                drone.DroneController.Start();
            }
        }
        
        public void SetSpeed(float speed)
        {
            _currentSpeed = speed;
            
            foreach (var drone in _allDrones)
            {
                drone.SetDroneSpeed(speed);
            }
        }
        
        public void SetPathEnabled(bool enabled)
        {
            _currentPathEnabled = enabled;
            
            foreach (var drone in _allDrones)
            {
                drone.SetIsDrawPath(enabled);
            }
        }
        
        public void UpdateControllers()
        {
            foreach (var controller in _controllers)
            {
                controller.Update();
            }
        }
        
        public void Dispose(Action<ResourceView> onHarvest, Action<EFractionName> onUnload)
        {
            foreach (var controller in _controllers)
            {
                controller.OnHarvestResource -= onHarvest;
                controller.OnResourceUnload -= onUnload;
            }
        }
    }
}

