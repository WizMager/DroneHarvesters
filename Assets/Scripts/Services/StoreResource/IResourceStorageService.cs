using System;
using Utils;

namespace Services.StoreResource
{
    public interface IResourceStorageService
    {
        Action<EFractionName, int> OnResourceChange { get; set; }
        
        void AddResource(EFractionName fraction);
    }
}