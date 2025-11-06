using System;
using System.Collections.Generic;
using Utils;

namespace Services.StoreResource
{
    public class ResourceStorageService : IResourceStorageService
    {
        private readonly Dictionary<EFractionName, int> _resources = new ();
        
        public Action<EFractionName, int> OnResourceChange { get; set; }
        
        public ResourceStorageService()
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