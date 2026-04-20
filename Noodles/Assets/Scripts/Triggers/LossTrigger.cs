using Managers;
using UnityEngine;

namespace Triggers
{
    /// <summary>
    ///     Causes the game to enter a loss state.
    /// </summary>
    public class LossTrigger : MonoBehaviour
    {
        private void Start()
        {
            GetComponent<Collider>().isTrigger = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player")) GameManager.Instance.TriggerLoss();
        }
    }
}