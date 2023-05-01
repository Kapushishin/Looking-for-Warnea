using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITakeDamage
{
    public float OnTakeDamage(float health, float armor, float damage);
    public void OnDeath();
}
