using UnityEngine;

namespace Db.Resource
{
    [CreateAssetMenu(fileName = "ResourceData", menuName = "Data/ResourceData")]
    public class ResourceData : ScriptableObject
    {
        [field: SerializeField] public float ResourceSpawnSpeed { get; private set; } = 2f;
    }
}

