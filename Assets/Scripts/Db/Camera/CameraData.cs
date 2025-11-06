using UnityEngine;

namespace Db.Camera
{
    [CreateAssetMenu(fileName = "CameraData", menuName = "Data/CameraData")]
    public class CameraData : ScriptableObject
    {
        [field:SerializeField] public float MoveSpeed = 10f;
    }
}