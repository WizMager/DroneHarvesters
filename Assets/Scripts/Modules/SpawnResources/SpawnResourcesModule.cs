using System;
using Core.Interfaces;
using Cysharp.Threading.Tasks;
using Ui;
using UnityEngine;
using UnityEngine.Pool;
using Views;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Modules.SpawnResources
{
    public class SpawnResourcesModule : ISpawnResourcesModule, IStartable, IDisposable
    {
        private readonly GameObject _resourceGameObject;
        private readonly ResourcesSpawnArea _resourcesSpawnArea;
        private readonly Transform _resourceContainer;
        private readonly ObjectPool<ResourceView> _resourcePool;
        private readonly UiController _uiController;
        private readonly MinimapController _minimapController;
        
        private float _spawnResourcesCooldown;
        private bool _spawnResourcesEnabled;

        public Action<ResourceView> OnResourceSpawned { get; set; }
        
        public SpawnResourcesModule(
            GameObject resourceGameObject, 
            ResourcesSpawnArea resourcesSpawnArea, 
            UiController uiController, 
            MinimapController minimapController
        )
        {
            _resourceGameObject = resourceGameObject;
            _resourcesSpawnArea = resourcesSpawnArea;
            _uiController = uiController;
            _minimapController = minimapController;

            _resourceContainer = new GameObject("ResourceContainer").GetComponent<Transform>();
            _resourcePool = new ObjectPool<ResourceView>(CreateResource, OnGetResource, OnReleaseResource);
            
            _uiController.OnResourceSpawnTimerChanged += SetSpawnResourcesSpeed;
        }

#region Object Pool Methodos

        private ResourceView CreateResource()
        {
            var resourceView = Object.Instantiate(_resourceGameObject, _resourceContainer).GetComponent<ResourceView>();
            
            return resourceView;
        }

        private void OnGetResource(ResourceView resourceView)
        {
            _minimapController.RegisterResource(resourceView.transform);
            resourceView.gameObject.SetActive(true);
        }
        
        private void OnReleaseResource(ResourceView resourceView)
        {
            _minimapController.UnregisterResource(resourceView.transform);
            resourceView.gameObject.SetActive(false);
        }

#endregion

        public void SetSpawnResourcesSpeed(float newCooldown)
        {
            _spawnResourcesCooldown = newCooldown;
        }

        public void SpawnResourcesActivation(bool isActive)
        {
            _spawnResourcesEnabled = isActive;
        }
        
        public void Start()
        {
            SpawnResources().Forget();
        }

        public void ResourceHarvested(ResourceView resourceView)
        {
            _resourcePool.Release(resourceView);
        }
        
        private async UniTaskVoid SpawnResources()
        {
            while (_spawnResourcesEnabled)
            {
                var randomCirclePosition = Random.onUnitSphere;
                randomCirclePosition.y = 0;
                var spawnPosition = _resourcesSpawnArea.transform.position + randomCirclePosition * _resourcesSpawnArea.Radius;
                spawnPosition.y = 1;
                var resourceView = _resourcePool.Get();
                resourceView.transform.position = spawnPosition;
                
                OnResourceSpawned?.Invoke(resourceView);
                
                await UniTask.Delay(TimeSpan.FromSeconds(_spawnResourcesCooldown));
            }
        }

        public void Dispose()
        {
            _uiController.OnResourceSpawnTimerChanged -= SetSpawnResourcesSpeed;
        }
    }
}