using Interfaces;
using Managers;
using UI;
using UnityEngine;

namespace Pickups
{
    /// <summary>
    ///     Represents a collectible coin pickup in the scene.
    ///     When collected, it updates the coin UI, plays a collect sound, and removes itself.
    /// </summary>
    public class CoinPickup : MonoBehaviour, ICollectable
    {
     public void OnCollect(GameObject collector)
        {
            FindAnyObjectByType<CoinUI>()?.AddCoin();
            AudioManager.Instance?.PlayCollect();
            Destroy(gameObject);
        }
    }
}