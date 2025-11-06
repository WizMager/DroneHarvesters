using System;
using Modules.Drone.States;
using UnityEngine;

namespace Db.Drone
{
    [Serializable]
    public struct DroneState
    {
        public EDroneState State;
        public Sprite Icon;
    }
}