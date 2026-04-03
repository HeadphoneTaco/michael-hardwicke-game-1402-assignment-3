using _Project.Scripts.Interfaces;
using _Project.Scripts.Managers;
using UnityEngine;

namespace _Project.Scripts
{
    /// <summary>
    ///     Represents a collectible health pickup that heals a target object
    ///     implementing a <see cref="Health" /> component when collected.
    /// </summary>
    public class HealthPickup : MonoBehaviour, ICollectable
    {
        [SerializeField] private int healAmount = 1;

        public void OnCollect(GameObject healthpack)
        {
            var health = healthpack.GetComponent<Health>();

            if (health != null)
            {
                AudioManager.Instance?.PlayCollect();
                health.Heal(healAmount);
            }

            Destroy(gameObject);
        }
    }
}