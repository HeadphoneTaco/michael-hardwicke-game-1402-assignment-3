using TMPro;
using UnityEngine;

namespace UI
{
    /// <summary>
    ///     Updates and displays the player's current coin total in the UI.
    /// </summary>
    public class CoinUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text coinText;
        private int _coinCount;

        public void AddCoin()
        {
            _coinCount++;
            coinText.SetText($"Coins: {_coinCount}");
        }
    }
}