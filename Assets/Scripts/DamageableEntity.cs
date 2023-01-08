using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageableEntity:MonoBehaviour
{
	public GameManager gm;
	public int HP = 5;
	public bool hasIframes = false;
	public float iframeDuration = 0.5f;
	public bool canTakeDamage = true;
	public bool blinkOnDamage = false;
	public bool camShakeOnDamage = false;
	public bool isEnemy = true;
	public bool isPlayer = false;
	public string levelName = "";
	public AudioSource audioSource;
	public AudioClip sfxHit;
	public GameObject playerHitFx;
	public GameObject enemyHitFx;
	public Transform fxPoint;

	float spriteBlinkingTimer = 0.0f;
	float spriteBlinkingMiniDuration = 0.1f;
	float spriteBlinkingTotalTimer = 0.0f;

	bool blinking = false;
	bool endGame = false;

	Rigidbody2D rb;
	SpriteRenderer sprite;
	private void Start()
	{
		rb = GetComponent<Rigidbody2D>();
		sprite = GetComponentInChildren<SpriteRenderer>();
	}

	void Update()
	{
		if (HP <= 0)
		{
			if (isPlayer && !endGame)
			{
				endGame = true;
				gm.FinishGame();
				Destroy(gameObject);
			}

			if (!endGame)
			{
				endGame = true;
				gm.NextLevel(levelName);
				Destroy(gameObject);
			}
		}
		if (blinking)
		{
			SpriteBlinkingEffect();
		}
	}

	public void TakeDamage(int damage)
	{
		if (!canTakeDamage) return;

		HP -= damage;
		if (hasIframes)
		{
			canTakeDamage = false;
			StartCoroutine(RestartIframes());
		}
		if (blinkOnDamage) blinking = true;
		if(camShakeOnDamage)
        {
			gm.ShakeCamera(2.5f);
        }
        if (isPlayer)
        {
			if(playerHitFx != null)
            {
				GameObject fx = Instantiate(playerHitFx, transform.position, Quaternion.identity);
				Destroy(fx, 1f);
				audioSource.PlayOneShot(sfxHit);
			}
		}
		else
		{
			if (enemyHitFx != null)
			{
				GameObject fx = Instantiate(enemyHitFx, fxPoint.position, Quaternion.identity);
				Destroy(fx, 1f);
			}
		}
	}

	public void Heal(int amount)
    {
		HP += amount;
    }

	public void ApplyKnockback(float force, bool facingRight)
	{
		var knockbackX = gameObject.transform.right * force * -1;
		if (!facingRight) knockbackX *= -1;
		var knockbackY = gameObject.transform.up * force;
		var knockback = knockbackX + knockbackY;
		if (rb != null)
		{
			rb.AddForce(knockback, ForceMode2D.Impulse);
		}
	}

	IEnumerator RestartIframes()
	{
		yield return new WaitForSeconds(iframeDuration);
		canTakeDamage = true;
	}

	private void SpriteBlinkingEffect()
	{
		spriteBlinkingTotalTimer += Time.deltaTime;
		if (spriteBlinkingTotalTimer >= iframeDuration)
		{
			blinking = false;
			spriteBlinkingTotalTimer = 0.0f;
			sprite.enabled = true;
			return;
		}

		spriteBlinkingTimer += Time.deltaTime;
		if (spriteBlinkingTimer >= spriteBlinkingMiniDuration)
		{
			spriteBlinkingTimer = 0.0f;
			if (sprite.enabled) sprite.enabled = false; 
			else sprite.enabled = true;
		}
	}
}
