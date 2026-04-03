using _Project.Scripts.Interfaces;
using UnityEngine;

namespace _Project.Scripts
{
    /// <summary>
    ///     Controls projectile behavior, including lifetime expiration, collision handling,
    ///     and damage application to objects that implement <see cref="IDamageable" />.
    /// </summary>
    /// <remarks>
    ///     Attach this component to any projectile prefab (for example: arrows or magic bolts)
    ///     and configure its damage and lifetime values in the Inspector.
    /// </remarks>
    [RequireComponent(typeof(Rigidbody))]
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private int damage = 1;
        [SerializeField] private float lifetime = 5f;

        private void Start()
        {
            Destroy(gameObject, lifetime);
        }

        private void OnTriggerEnter(Collider other)
        {
            var damageable = other.GetComponent<IDamageable>();

            if (damageable != null)
            {
                damageable.TakeDamage(damage);
                Destroy(gameObject);
            }

            if (!other.isTrigger) Destroy(gameObject);
        }
    }
}