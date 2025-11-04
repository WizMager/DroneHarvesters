namespace Modules.SpawnResources
{
    public interface ISpawnResourcesModule
    {
        void SetSpawnResourcesSpeed(float newCooldown);
        void SpawnResourcesActivation(bool isActive);
    }
}