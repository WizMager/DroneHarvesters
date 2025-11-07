using System.Collections.Generic;
using Core;
using Core.Interfaces;
using Db;
using Db.Camera;
using Db.Drone;
using Db.Resource;
using Modules.Camera;
using Modules.Drone.Impl;
using Modules.SpawnResources;
using Modules.SpawnResources.Impl;
using Services.GameFieldProvider;
using Services.Input;
using Services.Input.Impl;
using Services.StoreResource;
using Ui;
using UnityEngine;

public class Bootstrap : MonoBehaviour
{
        [SerializeField] private GameFieldProvider _gameFieldProvider;
        [SerializeField] private UiController _uiController;
        [SerializeField] private MinimapController _minimapController;
        [SerializeField] private CameraData _cameraData;
        [SerializeField] private DroneData _droneData;
        [SerializeField] private ResourceData _resourceData;

        private IModulesHandler _modulesHandler;
        private ISpawnResourcesModule _spawnResourcesModule;
        
        private void Awake()
        {
                _minimapController.RegisterBase(_gameFieldProvider.GameField.RedBase.transform, true);
                _minimapController.RegisterBase(_gameFieldProvider.GameField.BlueBase.transform, false);
                
                var services = CreateServices();
                var modulesList = CreateModules(services);
                
                _uiController.Initialize(services.ResourceStorageService, _droneData, _resourceData);
                
                _modulesHandler = new ModulesHandler(modulesList);
                _modulesHandler.Awake();
        }
        
        private (IResourceStorageService ResourceStorageService, IInputService InputService) CreateServices()
        {
                IResourceStorageService resourceStorageService = new ResourceStorageService();
                IInputService inputService = new InputService();
                inputService.Initialize();
                
                return (resourceStorageService, inputService);
        }
        
        private List<IModule> CreateModules((IResourceStorageService ResourceStorageService, IInputService InputService) services)
        {
                var modulesList = new List<IModule>();
                
                var cameraMove = new CameraMoveModule(_gameFieldProvider.GameField.Camera, services.InputService, _cameraData);
                modulesList.Add(cameraMove);
                
                var spawnResourceModule = new SpawnResourcesModule(
                    _gameFieldProvider.GameField.ResourcePrefab, 
                    _gameFieldProvider.GameField.ResourcesSpawnArea, 
                    _uiController, 
                    _minimapController,
                    _resourceData.ResourceSpawnSpeed
                );
                _spawnResourcesModule = spawnResourceModule;
                modulesList.Add(spawnResourceModule);
                
                var droneModule = new DroneModule(
                    _gameFieldProvider.GameField.DronePrefab, 
                    _gameFieldProvider.GameField.RedBase, 
                    _gameFieldProvider.GameField.BlueBase, 
                    _spawnResourcesModule, 
                    _uiController, 
                    services.ResourceStorageService, 
                    _gameFieldProvider.GameField.Camera, 
                    _droneData,
                    _minimapController
                );
                modulesList.Add(droneModule);
                
                return modulesList;
        }

        private void Start()
        {
                _modulesHandler.Start();
        }

        private void Update()
        {
                _modulesHandler.Update();
        }

        private void FixedUpdate()
        {
                _modulesHandler.FixedUpdate();
        }
}