using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTrigger:MonoBehaviour
{
	public int damage = 5;
    private void Start()
    {
        if (Upgrades.damage != 0)
        {
            damage = damage + (3 * Upgrades.attackSpeed);
        }
    }
    void OnTriggerEnter2D(Collider2D col)
	{
		if (col.CompareTag("Enemy"))
		{
			var damageableScript = col.GetComponent<DamageableEntity>();
            var enemyScript = col.GetComponent<Enemy>();

            if (damageableScript != null && enemyScript != null)
            {
                enemyScript.GetHit();
                damageableScript.TakeDamage(damage);
            }
        }
        Destroy(gameObject);
    }
}
