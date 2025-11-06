using System;
using System.Collections.Generic;
using Modules.Drone.States;
using UnityEngine;

namespace Db.Drone
{
    [CreateAssetMenu(fileName = "DroneData", menuName = "Data/DroneData")]
    public class DroneData : ScriptableObject
    {
        [SerializeField] private List<DroneState> _droneIcons;
        
        public Sprite GetStatusIcon(EDroneState droneState)
        {
            foreach (var drone in _droneIcons)
            {
                if (drone.State != droneState)
                    continue;

                return drone.Icon;
            }
            
            throw new Exception($"[{nameof(DroneData)}]: No icon found for state: {droneState}");
        }
    }
}