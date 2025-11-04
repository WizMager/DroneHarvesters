using System;
using System.Collections.Generic;
using Core;
using Core.Interfaces;
using Modules.SpawnResources;
using UnityEngine;
using Views;

public class Bootstrap : MonoBehaviour
{
        [SerializeField] private ResourcesSpawnArea _resourcesSpawnArea;
        [SerializeField] private GameObject _resourcePrefab;

        private IModulesHandler _modulesHandler;
        private ISpawnResourcesModule _spawnResourcesModule;
        
        private void Awake()
        {
                var modulesList = new List<IModule>();
                
                var spawnResourceModule = new SpawnResourcesModule(_resourcePrefab, _resourcesSpawnArea);
                _spawnResourcesModule = spawnResourceModule;
                _spawnResourcesModule.SetSpawnResourcesSpeed(2f);
                _spawnResourcesModule.SpawnResourcesActivation(true);
                modulesList.Add(spawnResourceModule);
                
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