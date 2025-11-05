using System;
using Views;

namespace Modules.SpawnResources
{
    public interface IResourcesModule
    {
        Action<ResourceView>  OnResourceSpawned { get; set; }
        
        void SetSpawnResourcesSpeed(float newCooldown);
        void SpawnResourcesActivation(bool isActive);
        void ResourceHarvested(ResourceView resourceView);
    }
}