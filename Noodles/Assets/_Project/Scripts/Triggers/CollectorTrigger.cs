using _Project.Scripts.Interfaces;
using UnityEngine;

namespace _Project.Scripts.Triggers
{
    /// <summary>
    ///     Detects trigger collisions and forwards collection events to objects
    ///     that implement the <see cref="ICollectable" /> interface.
    /// </summary>
    public class CollectorTrigger : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            var otherCollectable = other.GetComponent<ICollectable>();

            if (otherCollectable != null) otherCollectable.OnCollect(gameObject);
        }
    }
}