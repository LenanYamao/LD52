using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackEnemy : MonoBehaviour
{
	public int damage = 5;
	void OnTriggerEnter2D(Collider2D col)
	{
		if (col.CompareTag("Player"))
		{
			var damageableScript = col.GetComponent<DamageableEntity>();

			if (damageableScript != null)
			{
				damageableScript.TakeDamage(damage);
			}
		}
	}
}
