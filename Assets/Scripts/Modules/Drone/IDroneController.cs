using System;
using System.Collections.Generic;
using Modules.Drone.States;
using UnityEngine;
using Utils;
using Views;

namespace Modules.Drone
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
        Action<EFractionName> OnResourceUnload { get; set; }
        
        void Initialize(BaseView baseView);
        void Start();
        void Update();

        void ChangeState(IDroneState newState);
        void SetTarget(ResourceView target);
        void SetDestination(Vector3 position);
        void StartUnload();
        void ResourceUnload();
        void StartHarvestResource();
    }
}