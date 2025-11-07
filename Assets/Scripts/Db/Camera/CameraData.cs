using UnityEngine;

namespace Db.Camera
{
    [CreateAssetMenu(fileName = "CameraData", menuName = "Data/CameraData")]
    public class CameraData : ScriptableObject
    {
        [field:SerializeField] public float MoveSpeed = 10f;
        [field:SerializeField] public Vector3 FollowOffset = new Vector3(0, 10, -5);
    }
}