using System.Collections.Generic;
using Core;
using Core.Interfaces;
using Modules.Drone;
using Modules.SpawnResources;
using Modules.StoreResource;
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
        
        [SerializeField] private UiController _uiController;

        private IModulesHandler _modulesHandler;
        private ISpawnResourcesModule _spawnResourcesModule;
        
        private void Awake()
        {
                var modulesList = new List<IModule>();
                
                IResourceStorage resourceStorage = new ResourceStorage();
                
                var spawnResourceModule = new SpawnResourcesModule(_resourcePrefab, _resourcesSpawnArea, _uiController);
                _spawnResourcesModule = spawnResourceModule;
                _spawnResourcesModule.SetSpawnResourcesSpeed(2f);
                _spawnResourcesModule.SpawnResourcesActivation(true);
                modulesList.Add(spawnResourceModule);
                
                var droneModule = new DroneModule(_dronePrefab, _redBase, _blueBase, _spawnResourcesModule, _uiController, resourceStorage);
                modulesList.Add(droneModule);
                
                _uiController.Initialize(resourceStorage);
                
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