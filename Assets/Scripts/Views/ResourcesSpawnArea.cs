using UnityEngine;

namespace Views
{
    public class ResourcesSpawnArea : MonoBehaviour
    {
        [SerializeField] private bool _isDrawArea;

        [field: SerializeField] public float Radius { get; private set; } = 50f;
        
        private void OnDrawGizmos()
        {
            if (!_isDrawArea) return;
            
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, Radius);

        }
    }
}