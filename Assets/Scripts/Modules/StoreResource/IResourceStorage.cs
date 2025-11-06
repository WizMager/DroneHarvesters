using System;
using Utils;

namespace Modules.StoreResource
{
    public interface IResourceStorage
    {
        Action<EFractionName, int> OnResourceChange { get; set; }
        
        void AddResource(EFractionName fraction);
    }
}