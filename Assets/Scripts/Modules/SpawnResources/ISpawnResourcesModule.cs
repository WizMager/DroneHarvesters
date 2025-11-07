using System;
using Views;

namespace Modules.SpawnResources
{
    public interface ISpawnResourcesModule
    {
        Action<ResourceView>  OnResourceSpawned { get; set; }
        
        void ResourceHarvested(ResourceView resourceView);
    }
}