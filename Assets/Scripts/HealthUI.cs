using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
	public int healthAmount;
	public List<Image> healthSprites;
	public Sprite fullHealth;
	public Sprite emptyHealth;
	public Slider bossHpBar;
	public bool isBoss = false;

	DamageableEntity damageableScript;

    void Start()
    {
		damageableScript = GetComponent<DamageableEntity>();

		if(isBoss)
        {
			bossHpBar.maxValue = damageableScript.HP;
			bossHpBar.value = damageableScript.HP;
		}
	}

    void Update()
    {
        if (!isBoss)
        {
			if (damageableScript.HP > healthAmount) damageableScript.HP = healthAmount;

			for (int i = 0; i < healthSprites.Count; i++)
			{
				if (i < damageableScript.HP) healthSprites[i].sprite = fullHealth;
				else healthSprites[i].sprite = emptyHealth;

				if (i < healthAmount) healthSprites[i].enabled = true;
				else healthSprites[i].enabled = false;
			}
		}
        else
        {
			bossHpBar.value = damageableScript.HP;
		}
	}
}
