using TMPro;
using UnityEngine;

namespace _Project.Scripts.UI
{
    /// <summary>
    ///     Updates and displays the player's current coin total in the UI.
    /// </summary>
    public class CoinUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text coinText;
        private int coinCount;

        public void AddCoin()
        {
            coinCount++;
            coinText.SetText($"Coins: {coinCount}");
        }
    }
}