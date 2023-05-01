using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultTakeDamage : ITakeDamage
{
    public float OnTakeDamage(float health, float armor, float damage)
    {
        float currentHealth = health - (damage - armor);
        if (health <= 0) OnDeath();
        return currentHealth;
    }

    public void OnDeath()
    {
        Debug.Log("Unit Die");
    }
}
