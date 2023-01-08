using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CharacterController2D : MonoBehaviour
{
	[SerializeField] public float m_JumpForce = 400f;							
	[Range(0, 1)] [SerializeField] private float m_CrouchSpeed = .36f;		
	[Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;
	[SerializeField] private bool m_AirControl = false;						
	[SerializeField] private LayerMask m_WhatIsGround;						
	[SerializeField] private List<Transform> m_GroundCheck;					
	[SerializeField] private Transform m_CeilingCheck;
	[SerializeField] private Collider2D m_CrouchDisableCollider;

	const float k_GroundedRadius = .2f;
	private bool m_Grounded;
	const float k_CeilingRadius = .2f;
	private Rigidbody2D m_Rigidbody2D;
	private bool m_FacingRight = true; 
	private Vector3 m_Velocity = Vector3.zero;
	private float _timeLeftGrounded;
	private float _lastJumpPressed;

	Animator animator;

	[Header("Events")]
	[Space]

	public UnityEvent OnLandEvent;

	[System.Serializable]
	public class BoolEvent : UnityEvent<bool> { }

	public BoolEvent OnCrouchEvent;
	private bool m_wasCrouching = false;

	public AudioSource audioSource;
	public AudioClip sfxShoot;
	public AudioClip sfxJump;

	private void Awake()
	{
		m_Rigidbody2D = GetComponent<Rigidbody2D>();
		animator = GetComponentInChildren<Animator>();

		if (OnLandEvent == null)
			OnLandEvent = new UnityEvent();

		if (OnCrouchEvent == null)
			OnCrouchEvent = new BoolEvent();
	}
    private void Start()
    {
        if(Upgrades.attackSpeed != 0)
        {
			attackSpeed = attackSpeed - (0.2f * Upgrades.attackSpeed);
			if(attackSpeed <= 0.2f)
            {
				attackSpeed = 0.2f;
            }
        }
    }

    private void Update()
	{
		if (Input.GetAxisRaw("Horizontal") >= 0.01f || Input.GetAxisRaw("Horizontal") <= -0.01f)
		{
			animator.SetBool("moving", true);
		}
		else
		{
			animator.SetBool("moving", false);
		}

        if (Input.GetButtonDown("Jump") && m_Grounded)
        {
			if (!playingJumpSfx)
			{
				playingJumpSfx = true;
				audioSource.pitch = 1;
				audioSource.PlayOneShot(sfxJump);
			}
		}
	}

	private void FixedUpdate()
	{
		bool wasGrounded = m_Grounded;
		m_Grounded = false;

		// The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
		// This can be done using layers instead but Sample Assets will not overwrite your project settings.
		foreach (var check in m_GroundCheck)
        {
			Collider2D[] colliders = Physics2D.OverlapCircleAll(check.position, k_GroundedRadius, m_WhatIsGround);
			for (int i = 0; i < colliders.Length; i++)
			{
				if (colliders[i].gameObject != gameObject)
				{
					m_Grounded = true;
					if (wasGrounded) _timeLeftGrounded = Time.time; // Only trigger when first leaving
					else if (!wasGrounded)
                    {
						_coyoteUsable = true;
						OnLandEvent.Invoke();
					}
				}
			}
		}
	}

	[Header("JUMPING")]
	[SerializeField] private float _coyoteTimeThreshold = 0.1f;
	[SerializeField] private float _jumpBuffer = 0.1f;
	private bool _coyoteUsable;
	private bool CanUseCoyote => _coyoteUsable && !m_Grounded && _timeLeftGrounded + _coyoteTimeThreshold > Time.time;
	private bool HasBufferedJump => m_Grounded && _lastJumpPressed + _jumpBuffer > Time.time;
	bool playingJumpSfx = false;
	public void Move(float move, bool crouch, bool jump)
	{
		if(jump) _lastJumpPressed = Time.time;
		// If crouching, check to see if the character can stand up
		if (!crouch)
		{
			// If the character has a ceiling preventing them from standing up, keep them crouching
			if (Physics2D.OverlapCircle(m_CeilingCheck.position, k_CeilingRadius, m_WhatIsGround))
			{
				crouch = true;
			}
		}

		//only control the player if grounded or airControl is turned on
		if (m_Grounded || m_AirControl)
		{

			// If crouching
			if (crouch)
			{
				if (!m_wasCrouching)
				{
					m_wasCrouching = true;
					OnCrouchEvent.Invoke(true);
				}

				// Reduce the speed by the crouchSpeed multiplier
				move *= m_CrouchSpeed;

				// Disable one of the colliders when crouching
				if (m_CrouchDisableCollider != null)
					m_CrouchDisableCollider.enabled = false;
			} else
			{
				// Enable the collider when not crouching
				if (m_CrouchDisableCollider != null)
					m_CrouchDisableCollider.enabled = true;

				if (m_wasCrouching)
				{
					m_wasCrouching = false;
					OnCrouchEvent.Invoke(false);
				}
			}

			// Move the character by finding the target velocity
			Vector3 targetVelocity = new Vector2(move * 10f, m_Rigidbody2D.velocity.y);
			// And then smoothing it out and applying it to the character
			m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);

			// If the input is moving the player right and the player is facing left...
			if (move > 0 && !m_FacingRight)
			{
				// ... flip the player.
				Flip();
			}
			// Otherwise if the input is moving the player left and the player is facing right...
			else if (move < 0 && m_FacingRight)
			{
				// ... flip the player.
				Flip();
			}
		}
		// If the player should jump...
		if (jump && CanUseCoyote || HasBufferedJump)
		{
			// Add a vertical force to the player.
			m_Grounded = false;
			m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
		}
	}

	private void Flip()
	{
		// Switch the way the player is labelled as facing.
		m_FacingRight = !m_FacingRight;

		// Multiply the player's x local scale by -1.
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

	[Header("Attack")]
	[Space]
	public float attackSpeed = 1f;
	public float shotSpeed = 1f;
	public bool canShoot = true;
	public GameObject bullet;
	public Transform gunTransform;

	public void Attack()
	{
		if (!canShoot) return;
		canShoot = false;
		var pitchChange = Random.Range(-0.2f, 0.2f);

		audioSource.pitch = 1 + pitchChange;
		audioSource.PlayOneShot(sfxShoot);
		Vector2 direction = Input.mousePosition - gunTransform.position;
		Vector2 bulletVelocity = direction * shotSpeed;
		GameObject bulletInstance = Instantiate(bullet, gunTransform.position, gunTransform.rotation);
		var bulletRb = bulletInstance.GetComponent<Rigidbody2D>();
		bulletRb.AddForce(gunTransform.up * shotSpeed);
		Destroy(bulletInstance, 3f);

		StartCoroutine(ShootDelay());
	}
	public void Landed()
	{
		playingJumpSfx = false;
	}
	IEnumerator ShootDelay()
	{
		yield return new WaitForSeconds(attackSpeed);
		canShoot = true;
	}

	private void OnDrawGizmos()
	{
		foreach(var check in m_GroundCheck)
        {
			Gizmos.color = Color.blue;
			Gizmos.DrawWireSphere(check.position, k_GroundedRadius);
		}
		
		
		if(m_CeilingCheck != null)
        {
			Gizmos.color = Color.yellow;
			Gizmos.DrawWireSphere(m_CeilingCheck.position, k_CeilingRadius);
		}
	}
}
