using System;
using System.Collections.Generic;
using Utils;

namespace Modules.StoreResource
{
    public class ResourceStorage : IResourceStorage
    {
        private readonly Dictionary<EFractionName, int> _resources = new ();
        
        public Action<EFractionName, int> OnResourceChange { get; set; }
        
        public ResourceStorage()
        {
            _resources.Add(EFractionName.Red, 0);
            _resources.Add(EFractionName.Blue, 0);
        }

        public void AddResource(EFractionName fraction)
        {
            var value = _resources[fraction];
            value++;
            _resources[fraction] = value;
            OnResourceChange?.Invoke(fraction, value);
        }
    }
}