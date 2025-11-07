using System.Collections.Generic;
using Core;
using Core.Interfaces;
using Db.Camera;
using Db.Drone;
using Modules.Camera;
using Modules.Drone.Impl;
using Modules.SpawnResources;
using Modules.SpawnResources.Impl;
using Services.GameFieldProvider.Impl;
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

        private IModulesHandler _modulesHandler;
        private ISpawnResourcesModule _spawnResourcesModule;
        
        private void Awake()
        {
                _minimapController.RegisterBase(_gameFieldProvider.GameField.RedBase.transform, true);
                _minimapController.RegisterBase(_gameFieldProvider.GameField.BlueBase.transform, false);
                
                var modulesList = new List<IModule>();
                
                IResourceStorageService resourceStorageService = new ResourceStorageService();
                IInputService inputService = new InputService();
                inputService.Initialize();
                
                var cameraMove = new CameraMoveModule(_gameFieldProvider.GameField.Camera, inputService, _cameraData);
                modulesList.Add(cameraMove);
                
                var spawnResourceModule = new SpawnResourcesModule(
                    _gameFieldProvider.GameField.ResourcePrefab, 
                    _gameFieldProvider.GameField.ResourcesSpawnArea, 
                    _uiController, 
                    _minimapController
                );
                _spawnResourcesModule = spawnResourceModule;
                _spawnResourcesModule.SetSpawnResourcesSpeed(2f);
                _spawnResourcesModule.SpawnResourcesActivation(true);
                modulesList.Add(spawnResourceModule);
                
                var droneModule = new DroneModule(
                    _gameFieldProvider.GameField.DronePrefab, 
                    _gameFieldProvider.GameField.RedBase, 
                    _gameFieldProvider.GameField.BlueBase, 
                    _spawnResourcesModule, 
                    _uiController, 
                    resourceStorageService, 
                    _gameFieldProvider.GameField.Camera, 
                    _droneData, 
                    _minimapController
                );
                modulesList.Add(droneModule);
                
                _uiController.Initialize(resourceStorageService);
                
                _modulesHandler = new ModulesHandler(modulesList);
                
                _modulesHandler.Awake();
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