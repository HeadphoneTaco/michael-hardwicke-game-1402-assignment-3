using Managers;
using UnityEngine;

//TODO:Make the game have a win condition that isn't just a collision volume
namespace Triggers
{
    /// <summary>
    ///     Causes the game to enter a win state.
    /// </summary>
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