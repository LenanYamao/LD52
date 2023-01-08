using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
	public Transform playerTransform;
	public LayerMask playerLayer;

	Animator animator;
	private void Start()
	{
		animator = GetComponent<Animator>();
	}

	public void GetHit()
	{

	}

}
