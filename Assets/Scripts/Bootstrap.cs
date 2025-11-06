using System.Collections.Generic;
using Core;
using Core.Interfaces;
using Db.Camera;
using Modules.Camera;
using Modules.Drone;
using Modules.SpawnResources;
using Services.Input;
using Services.Input.Impl;
using Services.StoreResource;
using Ui;
using UnityEngine;
using Views;

public class Bootstrap : MonoBehaviour
{
        [SerializeField] private ResourcesSpawnArea _resourcesSpawnArea;
        [SerializeField] private GameObject _resourcePrefab;
        [SerializeField] private GameObject _dronePrefab;
        [SerializeField] private BaseView _redBase;
        [SerializeField] private BaseView _blueBase;
        [SerializeField] private Camera _camera;
        
        [SerializeField] private UiController _uiController;
        
        [SerializeField] private CameraData _cameraData;

        private IModulesHandler _modulesHandler;
        private ISpawnResourcesModule _spawnResourcesModule;
        
        private void Awake()
        {
                var modulesList = new List<IModule>();
                
                IResourceStorageService resourceStorageService = new ResourceStorageService();
                IInputService inputService = new InputService();
                inputService.Initialize();
                
                var cameraMove = new CameraMoveModule(_camera, inputService, _cameraData);
                modulesList.Add(cameraMove);
                
                var spawnResourceModule = new SpawnResourcesModule(_resourcePrefab, _resourcesSpawnArea, _uiController);
                _spawnResourcesModule = spawnResourceModule;
                _spawnResourcesModule.SetSpawnResourcesSpeed(2f);
                _spawnResourcesModule.SpawnResourcesActivation(true);
                modulesList.Add(spawnResourceModule);
                
                var droneModule = new DroneModule(_dronePrefab, _redBase, _blueBase, _spawnResourcesModule, _uiController, resourceStorageService);
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