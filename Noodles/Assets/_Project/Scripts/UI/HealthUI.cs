using TMPro;
using UnityEngine;

namespace _Project.Scripts.UI
{
    /// <summary>
    ///     Displays the player's health in a TMP text element and keeps it synchronized
    ///     with health change events from the assigned <see cref="Health" /> component.
    /// </summary>
    public class HealthUI : MonoBehaviour
    {
        [SerializeField] private Health playerHealth;
        [SerializeField] private TMP_Text healthText;

        private void Start()
        {
            UpdateHealthDisplay(playerHealth.GetCurrentHealth(), playerHealth.GetMaxHealth());
        }

        private void OnEnable()
        {
            playerHealth.OnHealthChanged += UpdateHealthDisplay;
        }

        private void OnDisable()
        {
            playerHealth.OnHealthChanged -= UpdateHealthDisplay;
        }

        private void UpdateHealthDisplay(int current, int max)
        {
            healthText.SetText($"HP: {current}/{max}");
        }
    }
}