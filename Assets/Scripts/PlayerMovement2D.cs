using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement2D : MonoBehaviour
{
	public CharacterController2D controller;

	float horizontalMove = 0f;
	public float speed = 40f;
	bool jump = false;

    private void Start()
    {
		if (Upgrades.speed != 0)
		{
			speed = speed - (2.5f * Upgrades.speed);
		}
	}

    void Update()
    {
		if (UiController.isPaused) return;
		horizontalMove = Input.GetAxisRaw("Horizontal") * speed;

		if (Input.GetButtonDown("Jump"))
		{
			jump = true;
		}
		if (Input.GetButton("Attack"))
		{
			controller.Attack();
		}
	}

	private void FixedUpdate()
	{
		controller.Move(horizontalMove * Time.fixedDeltaTime, false, jump);
		jump = false;
	}
}
