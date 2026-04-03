using UnityEngine;

namespace _Project.Scripts
{
    /// <summary>
    ///     Marks a GameObject's transform as a spawn location in the scene.
    /// </summary>
    public class SpawnPoint : MonoBehaviour
    {
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, 0.5f);
            Gizmos.DrawLine(transform.position, transform.position + Vector3.up * 2f);
        }
    }
}