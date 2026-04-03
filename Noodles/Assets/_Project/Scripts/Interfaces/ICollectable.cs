using UnityEngine;

namespace _Project.Scripts.Interfaces
{
    public interface ICollectable
    {
        void OnCollect(GameObject collector);
    }
}