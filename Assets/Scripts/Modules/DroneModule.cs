using System;
using System.Collections.Generic;
using Core.Interfaces;
using Cysharp.Threading.Tasks;
using Drone;
using Modules.SpawnResources;
using UnityEngine;
using UnityEngine.Pool;
using Views;
using Object = UnityEngine.Object;

namespace Modules
{
    public class DroneModule : IStartable, IUpdatable, IDisposable
    {
        private readonly GameObject _dronePrefab;
        private readonly BaseView _redBase;
        private readonly BaseView _blueBase;
        private readonly List<IDroneController> _droneControllers = new ();
        private readonly ObjectPool<DroneView> _dronePool;
        private readonly IResourcesModule _resourcesModule;
        private readonly List<ResourceView> _freeResources = new ();
        
        public IReadOnlyList<ResourceView> FreeResources => _freeResources;
        
        private int _redDroneCount = 1;
        private int _blueDroneCount = 1;
        
        public DroneModule(
            GameObject dronePrefab, 
            BaseView redBase, 
            BaseView blueBase, 
            IResourcesModule resourcesModule
        )
        {
            _dronePrefab = dronePrefab;
            _redBase = redBase;
            _blueBase = blueBase;
            _resourcesModule = resourcesModule;
            
            _resourcesModule.OnResourceSpawned += OnResourceSpawned;
            _dronePool = new ObjectPool<DroneView>(CreateDrone, OnGetDrone, OnReleaseDrone);
        }

        private void OnResourceSpawned(ResourceView freeResource)
        {
            _freeResources.Add(freeResource);
        }


#region Object Pool Methodos

        private DroneView CreateDrone()
        {
            var droneView = Object.Instantiate(_dronePrefab).GetComponent<DroneView>();
            var droneController = new DroneController(droneView, FreeResources);
            droneController.OnHarvestResource += OnResourceHarvested;
            _droneControllers.Add(droneController);
            
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
        
        private void OnResourceHarvested(ResourceView resourceView)
        {
            _freeResources.Remove(resourceView);
            
            WaitAndHarvest().Forget();

            return;
            
            async UniTaskVoid WaitAndHarvest()
            {
                await UniTask.Delay(TimeSpan.FromSeconds(2));
            
                _resourcesModule.ResourceHarvested(resourceView);
            }
        }
        
        public void Start()
        {
            for (var i = 0; i < _redDroneCount; i++)
            {
                var drone = _dronePool.Get();
                drone.DroneController.Initialize(_redBase);
            }
            
            for (var i = 0; i < _blueDroneCount; i++)
            {
                var drone = _dronePool.Get();
                drone.DroneController.Initialize(_blueBase);
            }

            foreach (var droneController in _droneControllers)
            {
                droneController.Start();
            }
        }

        public void Update()
        {
            foreach (var droneController in _droneControllers)
            {
                droneController.Update();
            }
        }

        public void Dispose()
        {
            _resourcesModule.OnResourceSpawned -= OnResourceSpawned;
        }
    }
}