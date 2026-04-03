using _Project.Scripts.Interfaces;
using UnityEngine;

namespace _Project.Scripts
{
    /// <summary>
    ///     Represents a shootable target that can receive damage and toggle a linked door.
    /// </summary>
    //TODO:Make Generic trigger for uses other than just toggling a door.
    [RequireComponent(typeof(Collider))]
    public class ShootingTarget : MonoBehaviour, IDamageable
    {
        [SerializeField] private Door linkedDoor;

        public void TakeDamage(int amount)
        {
            linkedDoor?.Toggle();
        }
    }
}