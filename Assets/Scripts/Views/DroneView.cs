using System;
using Drone;
using UnityEngine;
using UnityEngine.AI;

namespace Views
{
    public class DroneView : MonoBehaviour
    {
        public BaseView homeBase;
        public NavMeshAgent agent;
        public IDroneState currentState;
        public GameObject targetResource;
        public float collectDistance = 2f;

        void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            ChangeState(new DroneIdleState());
        }

        void Update()
        {
            currentState?.UpdateState(this);
        }

        public void ChangeState(IDroneState newState)
        {
            currentState?.ExitState(this);
            currentState = newState;
            currentState?.EnterState(this);
        }

        private void OnDrawGizmos()
        {
            if (!agent || agent.path == null) return;

            var corners = agent.path.corners;
            if (corners.Length < 2) return;

            Gizmos.color = Color.cyan;

            for (var i = 0; i < corners.Length - 1; i++)
            {
                Gizmos.DrawLine(corners[i], corners[i + 1]);
                Gizmos.DrawSphere(corners[i], 0.1f);
            }
        }
    }
}