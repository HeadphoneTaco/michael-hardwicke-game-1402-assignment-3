using Interfaces;
using UnityEngine;

/// <summary>
///     Represents a shootable target that can receive damage and toggle a linked door.
/// </summary>
public class ShootingTarget : MonoBehaviour, IDamageable
{
    [SerializeField] private Door linkedDoor;

    public void TakeDamage(int amount)
    {
        linkedDoor?.Toggle();
    }
}