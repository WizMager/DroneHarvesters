using System.Collections.Generic;
using Core.Interfaces;

namespace Core
{
    public class ModulesHandler : IModulesHandler
    {
        private readonly List<IAwakable> _awakables = new();
        private readonly List<IStartable> _startables = new();
        private readonly List<IUpdatable> _updatables = new();
        private readonly List<IFixedUpdatable> _fixedUpdatables = new();

        public ModulesHandler(List<IModule> modules)
        {
            foreach (var module in modules)
            {
                RegisterModule(module);
            }
        }

        public void AddModule(IModule module)
        {
            RegisterModule(module);
        }

        public void RemoveModule(IModule module)
        {
            UnregisterModule(module);
        }
        
        private void RegisterModule(IModule module)
        {
            if (module is IAwakable awakable)
            {
                _awakables.Add(awakable);
            }
            
            if (module is IStartable startable)
            {
                _startables.Add(startable);
            }
                
            if (module is IUpdatable updatable)
            {
                _updatables.Add(updatable);
            }
                
            if (module is IFixedUpdatable fixedUpdatable)
            {
                _fixedUpdatables.Add(fixedUpdatable);
            }
        }
        
        private void UnregisterModule(IModule module)
        {
            if (module is IAwakable awakable)
            {
                _awakables.Remove(awakable);
            }
            
            if (module is IStartable startable)
            {
                _startables.Remove(startable);
            }
                
            if (module is IUpdatable updatable)
            {
                _updatables.Remove(updatable);
            }
                
            if (module is IFixedUpdatable fixedUpdatable)
            {
                _fixedUpdatables.Remove(fixedUpdatable);
            }
        }

        public void Awake()
        {
            foreach (var awakable in _awakables)
            {
                awakable.Awake();
            }
        }

        public void Start()
        {
            foreach (var startable in _startables)
            {
                startable.Start();
            }
        }

        public void Update()
        {
            foreach (var updatable in _updatables)
            {
                updatable.Update();
            }
        }

        public void FixedUpdate()
        {
            foreach (var fixedUpdatable in _fixedUpdatables)
            {
                fixedUpdatable.FixedUpdate();
            }
        }
    }
}