using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultDoDamage : IDoDamage
{
    public void OnDoDamage(Units enemyUnit, float damage)
    {
        enemyUnit.TakeDamage(damage);
    }
}
