using System;
using System.Collections.Generic;
using Core.Interfaces;
using Cysharp.Threading.Tasks;
using Db.Drone;
using Modules.SpawnResources;
using Services.StoreResource;
using Ui;
using UnityEngine;
using UnityEngine.Pool;
using Utils;
using Views;
using Object = UnityEngine.Object;

namespace Modules.Drone
{
    public class DroneModule : IStartable, IUpdatable, IDisposable
    {
        private readonly GameObject _dronePrefab;
        private readonly BaseView _redBase;
        private readonly BaseView _blueBase;
        private readonly List<IDroneController> _droneControllers = new ();
        private readonly ObjectPool<DroneView> _redDronePool;
        private readonly ObjectPool<DroneView> _blueDronePool;
        private readonly ISpawnResourcesModule _spawnResourcesModule;
        private readonly List<ResourceView> _freeResources = new ();
        private readonly List<DroneView> _spawnedDrones = new ();
        private readonly List<DroneView> _activeRedDrones = new ();
        private readonly List<DroneView> _activeBlueDrones = new ();
        private readonly UiController _uiController;
        private readonly IResourceStorageService _resourceStorageService;
        private readonly UnityEngine.Camera _camera;
        private readonly DroneData _droneData;
        private readonly MinimapController _minimapController;
        
        public IReadOnlyList<ResourceView> FreeResources => _freeResources;
        
        private int _redDroneCount = 3;
        private int _blueDroneCount = 3;
        private float _droneSpeed = 10f;
        private bool _isDronePathEnabled = true;
        
        public DroneModule(
            GameObject dronePrefab, 
            BaseView redBase, 
            BaseView blueBase, 
            ISpawnResourcesModule spawnResourcesModule,
            UiController uiController, 
            IResourceStorageService resourceStorageService,
            UnityEngine.Camera camera, 
            DroneData droneData, 
            MinimapController minimapController
        )
        {
            _dronePrefab = dronePrefab;
            _redBase = redBase;
            _blueBase = blueBase;
            _spawnResourcesModule = spawnResourcesModule;
            _uiController = uiController;
            _resourceStorageService = resourceStorageService;
            _camera = camera;
            _droneData = droneData;
            _minimapController = minimapController;

            _spawnResourcesModule.OnResourceSpawned += OnResourceSpawned;
            _redDronePool = new ObjectPool<DroneView>(CreateRedDrone, OnGetDrone, OnReleaseDrone);
            _blueDronePool = new ObjectPool<DroneView>(CreateBlueDrone, OnGetDrone, OnReleaseDrone);
            
            _uiController.OnDroneCountChanged += OnDroneCountChanged;
            _uiController.OnDronePathToggleChanged += OnDronePathToggleChange;
            _uiController.OnDroneSpeedChanged += OnDroneSpeedChange;
        }

        private void OnDroneCountChanged(EFractionName fractionName, int value)
        {
            int changeCount;
            
            switch (fractionName)
            {
                case EFractionName.Red:
                    changeCount = value - _redDroneCount;
                    
                    if (changeCount < 0)
                    {
                        for (var i = 0; i < Math.Abs(changeCount); i++)
                        {
                            _redDronePool.Release(_activeRedDrones[^1]);
                        }
                    }
                    else
                    {
                        for (var i = 0; i < changeCount; i++)
                        {
                            var spawnedDrone = _redDronePool.Get();
                            spawnedDrone.DroneController.Start();
                        }
                    }
                    _redDroneCount = value;
                    break;
                case EFractionName.Blue:
                    changeCount = value - _blueDroneCount;
                    
                    if (changeCount < 0)
                    {
                        for (var i = 0; i < Math.Abs(changeCount); i++)
                        {
                            _blueDronePool.Release(_activeBlueDrones[^1]);
                        }
                    }
                    else
                    {
                        for (var i = 0; i < changeCount; i++)
                        {
                            var spawnedDrone = _blueDronePool.Get();
                            spawnedDrone.DroneController.Start();
                        }
                    }

                    _blueDroneCount = value;
                    break;
            }
        }

        private void OnDronePathToggleChange(bool value)
        {
            _isDronePathEnabled = value;
            
            foreach (var spawnedDrone in _spawnedDrones)
            {
                spawnedDrone.SetIsDrawPath(_isDronePathEnabled);
            }
        }

        private void OnDroneSpeedChange(float value)
        {
            _droneSpeed = value;

            foreach (var spawnedDrone in _spawnedDrones)
            {
                spawnedDrone.SetDroneSpeed(_droneSpeed);
            }
        }

        private void OnResourceSpawned(ResourceView freeResource)
        {
            _freeResources.Add(freeResource);
        }

#region Object Pool Methodos

        private DroneView CreateRedDrone()
        {
            var droneView = Object.Instantiate(_dronePrefab).GetComponent<DroneView>();
            droneView.SetDroneSpeed(_droneSpeed);
            droneView.SetIsDrawPath(_isDronePathEnabled);
            var droneController = new DroneController(droneView, FreeResources, _droneData);
            droneView.Initialize(droneController, EFractionName.Red, _camera);
            droneController.Initialize(_redBase);
            droneController.OnHarvestResource += OnResourceHarvested;
            droneController.OnResourceUnload += OnResourceUnload;
            _droneControllers.Add(droneController);
            
            _spawnedDrones.Add(droneView);
            
            return droneView;
        }
        
        private DroneView CreateBlueDrone()
        {
            var droneView = Object.Instantiate(_dronePrefab).GetComponent<DroneView>();
            droneView.SetDroneSpeed(_droneSpeed);
            droneView.SetIsDrawPath(_isDronePathEnabled);
            var droneController = new DroneController(droneView, FreeResources, _droneData);
            droneView.Initialize(droneController, EFractionName.Blue, _camera);
            droneController.Initialize(_blueBase);
            droneController.OnHarvestResource += OnResourceHarvested;
            droneController.OnResourceUnload += OnResourceUnload;
            _droneControllers.Add(droneController);
            
            _spawnedDrones.Add(droneView);
            
            return droneView;
        }

        private void OnGetDrone(DroneView droneView)
        {
            switch (droneView.Fraction)
            {
                case EFractionName.Red:
                    _minimapController.RegisterUnit(droneView.transform, true);
                    _activeRedDrones.Add(droneView);
                    break;
                case EFractionName.Blue:
                    _minimapController.RegisterUnit(droneView.transform, false);
                    _activeBlueDrones.Add(droneView);
                    break;
            }
            
            droneView.gameObject.SetActive(true);
        }
        
        private void OnReleaseDrone(DroneView droneView)
        {
            switch (droneView.Fraction)
            {
                case EFractionName.Red:
                    _minimapController.UnregisterUnit(droneView.transform, true);
                    _activeRedDrones.Remove(droneView);
                    break;
                case EFractionName.Blue:
                    _minimapController.UnregisterUnit(droneView.transform, false);
                    _activeBlueDrones.Remove(droneView);
                    break;
            }
            
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
                
                _spawnResourcesModule.ResourceHarvested(resourceView);
            }
        }
        
        private void OnResourceUnload(EFractionName fraction)
        {
            _resourceStorageService.AddResource(fraction);
        }
        
        public void Start()
        {
            for (var i = 0; i < _redDroneCount; i++)
            {
                _redDronePool.Get();
            }
            
            for (var i = 0; i < _blueDroneCount; i++)
            {
                _blueDronePool.Get();
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
            _spawnResourcesModule.OnResourceSpawned -= OnResourceSpawned;
            _uiController.OnDroneCountChanged -= OnDroneCountChanged;
            _uiController.OnDronePathToggleChanged -= OnDronePathToggleChange;
            _uiController.OnDroneSpeedChanged -= OnDroneSpeedChange;

            foreach (var droneController in _droneControllers)
            {
                droneController.OnHarvestResource -= OnResourceHarvested;
                droneController.OnResourceUnload -= OnResourceUnload;
            }
        }
    }
}