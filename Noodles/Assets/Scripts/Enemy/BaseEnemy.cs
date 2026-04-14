using Interfaces;
using UnityEngine;

public class BaseEnemy : MonoBehaviour, IDamageable
{
    private int _currentHealth;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public virtual void TakeDamage()
    {
        
    }

    public void TakeDamage(int amount)
    {
        throw new System.NotImplementedException();
    }
}
