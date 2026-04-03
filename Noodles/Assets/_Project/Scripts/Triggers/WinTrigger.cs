using _Project.Scripts.Managers;
using UnityEngine;

//TODO:Make the game have a win condition that isn't just a collision volume
namespace _Project.Scripts.Triggers
{
    /// <summary>
    ///     Trigger volume that ends the level with a win state when the player enters it.
    /// </summary>
    [RequireComponent(typeof(Collider))]
    public class WinTrigger : MonoBehaviour
    {
        private void Start()
        {
            GetComponent<Collider>().isTrigger = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player")) GameManager.Instance.TriggerWin();
        }
    }
}