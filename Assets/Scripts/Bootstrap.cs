using System.Collections.Generic;
using Core;
using Core.Interfaces;
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
        private IInputService _inputService;
        private IResourceStorageService _resourceStorageService;
        
        private void Awake()
        {
                _minimapController.RegisterBase(_gameFieldProvider.GameField.RedBase.transform, true);
                _minimapController.RegisterBase(_gameFieldProvider.GameField.BlueBase.transform, false);
                
                _resourceStorageService = new ResourceStorageService();
                _inputService = new InputService();
                _inputService.Initialize();
                
                CreateModules();
                
                _uiController.Initialize(_resourceStorageService, _droneData, _resourceData);
                
                _modulesHandler.Awake();
        }
        
        private void CreateModules()
        {
                var modulesList = new List<IModule>();
                
                var cameraMove = new CameraMoveModule(_gameFieldProvider.GameField.Camera, _inputService, _cameraData, _minimapController);
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
                    _resourceStorageService, 
                    _gameFieldProvider.GameField.Camera, 
                    _droneData,
                    _minimapController
                );
                modulesList.Add(droneModule);

                _modulesHandler = new ModulesHandler(modulesList);
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