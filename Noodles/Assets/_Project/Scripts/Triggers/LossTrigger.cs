using _Project.Scripts.Managers;
using UnityEngine;

//TODO:Make the game actually get to the loss state

namespace _Project.Scripts.Triggers
{
    /// <summary>
    ///     Trigger volume that causes the game to enter a loss state when the player enters it.
    /// </summary>
    [RequireComponent(typeof(Collider))]
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