using Managers;
using UnityEngine;

//TODO:Make the game actually get to the loss state

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