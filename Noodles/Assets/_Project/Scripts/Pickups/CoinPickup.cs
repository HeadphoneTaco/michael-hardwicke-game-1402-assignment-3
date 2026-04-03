using _Project.Scripts.Interfaces;
using _Project.Scripts.Managers;
using _Project.Scripts.UI;
using UnityEngine;

namespace _Project.Scripts
{
    /// <summary>
    ///     Represents a collectible coin pickup in the scene.
    ///     When collected, it updates the coin UI, plays a collect sound, and removes itself.
    /// </summary>
    public class CoinPickup : MonoBehaviour, ICollectable
    {
        /// <summary>
        ///     Handles coin collection logic for the colliding collector object.
        /// </summary>
        /// <param name="collector">
        ///     The GameObject that collected this coin. Included to satisfy the <see cref="ICollectable" /> contract.
        /// </param>
        public void OnCollect(GameObject collector)
        {
            FindAnyObjectByType<CoinUI>()?.AddCoin();
            AudioManager.Instance?.PlayCollect();
            Destroy(gameObject);
        }
    }
}