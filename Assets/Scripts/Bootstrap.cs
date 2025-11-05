using System;
using System.Collections.Generic;
using Core;
using Core.Interfaces;
using Modules;
using Modules.SpawnResources;
using UnityEngine;
using Views;

public class Bootstrap : MonoBehaviour
{
        [SerializeField] private ResourcesSpawnArea _resourcesSpawnArea;
        [SerializeField] private GameObject _resourcePrefab;
        [SerializeField] private GameObject _dronePrefab;
        [SerializeField] private BaseView _redBase;
        [SerializeField] private BaseView _blueBase;

        private IModulesHandler _modulesHandler;
        private IResourcesModule _resourcesModule;
        
        private void Awake()
        {
                var modulesList = new List<IModule>();
                
                var spawnResourceModule = new ResourcesModule(_resourcePrefab, _resourcesSpawnArea);
                _resourcesModule = spawnResourceModule;
                _resourcesModule.SetSpawnResourcesSpeed(2f);
                _resourcesModule.SpawnResourcesActivation(true);
                modulesList.Add(spawnResourceModule);
                
                var droneModule = new DroneModule(_dronePrefab, _redBase, _blueBase, _resourcesModule);
                modulesList.Add(droneModule);
                
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