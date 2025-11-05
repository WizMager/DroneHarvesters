using System;
using System.Collections.Generic;
using UnityEngine;
using Views;

namespace Drone
{
    public interface IDroneController
    {
        ResourceView TargetResource { get; }
        Vector3 CurrentDronePosition { get; }
        Vector3 BasePosition { get; }
        float ResourceCollectDistance { get; }
        float BaseDestinationDistance { get; }
        IReadOnlyList<ResourceView> FreeResourcesList { get; }
        Action<ResourceView> OnHarvestResource { get; set; }
        
        void Initialize(BaseView baseView);
        void Start();
        void Update();

        void ChangeState(IDroneState newState);
        void SetTarget(ResourceView target);
        void SetDestination(Vector3 position);
        void ResourceUnload();
        void StartHarvestResource();
    }
}