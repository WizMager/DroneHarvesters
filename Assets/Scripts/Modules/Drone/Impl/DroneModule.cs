using System;
using System.Collections.Generic;
using Core.Interfaces;
using Cysharp.Threading.Tasks;
using Db.Drone;
using Modules.SpawnResources;
using Services.StoreResource;
using Ui;
using UnityEngine;
using Utils;
using Views;

namespace Modules.Drone.Impl
{
    public class DroneModule : IStartable, IUpdatable, IDisposable
    {
        private readonly Dictionary<EFractionName, FractionDroneManager> _fractionManagers = new();
        private readonly List<ResourceView> _freeResources = new();
        private readonly UiController _uiController;
        private readonly IResourceStorageService _resourceStorageService;
        private readonly ISpawnResourcesModule _spawnResourcesModule;

        private float _droneSpeed;
        private bool _isDronePathEnabled;
        
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
            _uiController = uiController;
            _resourceStorageService = resourceStorageService;
            _spawnResourcesModule = spawnResourcesModule;

            _droneSpeed = droneData.DefaultDroneSpeed;
            _isDronePathEnabled = droneData.DefaultIsDronePathEnabled;
            
            _fractionManagers[EFractionName.Red] = new FractionDroneManager(
                EFractionName.Red, redBase, dronePrefab, _freeResources, droneData, camera,
                droneData.DefaultDroneCount, _droneSpeed, _isDronePathEnabled,
                OnResourceHarvested, OnResourceUnload, minimapController
            );
            
            _fractionManagers[EFractionName.Blue] = new FractionDroneManager(
                EFractionName.Blue, blueBase, dronePrefab, _freeResources, droneData, camera,
                droneData.DefaultDroneCount, _droneSpeed, _isDronePathEnabled,
                OnResourceHarvested, OnResourceUnload, minimapController
            );
            
            _spawnResourcesModule.OnResourceSpawned += OnResourceSpawned;
            _uiController.OnDroneCountChanged += OnDroneCountChanged;
            _uiController.OnDronePathToggleChanged += OnDronePathToggleChange;
            _uiController.OnDroneSpeedChanged += OnDroneSpeedChange;
        }

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
        
        private void OnResourceSpawned(ResourceView freeResource)
        {
            _freeResources.Add(freeResource);
        }
        
        private void OnDroneCountChanged(EFractionName fractionName, int value)
        {
            if (_fractionManagers.TryGetValue(fractionName, out var manager))
            {
                manager.SetDroneCount(value);
            }
        }

        private void OnDronePathToggleChange(bool value)
        {
            _isDronePathEnabled = value;
            foreach (var manager in _fractionManagers.Values)
            {
                manager.SetPathEnabled(value);
            }
        }

        private void OnDroneSpeedChange(float value)
        {
            _droneSpeed = value;
            foreach (var manager in _fractionManagers.Values)
            {
                manager.SetSpeed(value);
            }
        }
        
        public void Start()
        {
            foreach (var manager in _fractionManagers.Values)
            {
                manager.InitialStartDrones();
                manager.SetSpeed(_droneSpeed);
                manager.SetPathEnabled(_isDronePathEnabled);
            }
        }

        public void Update()
        {
            foreach (var manager in _fractionManagers.Values)
            {
                manager.UpdateControllers();
            }
        }

        public void Dispose()
        {
            _spawnResourcesModule.OnResourceSpawned -= OnResourceSpawned;
            _uiController.OnDroneCountChanged -= OnDroneCountChanged;
            _uiController.OnDronePathToggleChanged -= OnDronePathToggleChange;
            _uiController.OnDroneSpeedChanged -= OnDroneSpeedChange;

            foreach (var manager in _fractionManagers.Values)
            {
                manager.Dispose(OnResourceHarvested, OnResourceUnload);
            }
        }
    }
}