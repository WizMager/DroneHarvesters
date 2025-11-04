using Core.Interfaces;

namespace Core
{
    public interface IModulesHandler
    {
        void AddModule(IModule module);
        void RemoveModule(IModule module);
        
        void Awake();
        void Start();
        void Update();
        void FixedUpdate();
    }
}