using UnityEngine;

public class Player : MonoBehaviour
{
	// Component References
	public Animator anim { get; private set; }
	public Rigidbody2D rb { get; private set; }

	public PlayerInputSet input { get; private set; }
	private StateMachine stateMachine;

	// Player States
	public PlayerIdleState idleState { get; private set; }
	public PlayerMoveState moveState { get; private set; }
	public PlayerJumpState jumpState { get; private set; }
	public PlayerFallState fallState { get; private set; }
	public PlayerWallSlideState wallSlideState { get; private set; }
	public PlayerWallJumpState wallJumpState { get; private set; }

	// Movement Settings
	[field: Header("Movement details")]
	[field: SerializeField] public float moveSpeed { get; private set; }
	[field: SerializeField] public float jumpForce { get; private set; }
	[field: SerializeField] public Vector2 wallJumpForce { get; private set; }

	[field: SerializeField, Range(0, 1)] public float inAirMoveMultiplier { get; private set; }
	[field: SerializeField, Range(0, 1)] public float wallSlideSlowMultiplier { get; private set; }
	private bool facingRight = true;
	public int facingDir { get; private set; } = 1; // TODO: Better make this an enum with a value of right = 1 and left = -1 / other alternative would be to use the facingRight variable instead
	public Vector2 moveInput { get; private set; }

	// Collision Settings
	[Header("Collision detection")]
	[SerializeField] private float groundCheckDistance;
	[SerializeField] private float wallCheckDistance;
	[SerializeField] private LayerMask whatIsGround;
	public bool groundDetected { get; private set; }
	public bool wallDetected { get; private set; }

	#region Unity Lifecycle
	/// Unity's event functions: Awake, Start, Update...

	private void Awake()
	{
		anim = GetComponentInChildren<Animator>();
		rb = GetComponent<Rigidbody2D>();

		stateMachine = new StateMachine();
		input = new PlayerInputSet();

		idleState = new PlayerIdleState(this, stateMachine, "idle"); // TODO: Magic String to enum
		moveState = new PlayerMoveState(this, stateMachine, "move"); // TODO: Magic String to enum
		jumpState = new PlayerJumpState(this, stateMachine, "jumpFall"); // TODO: Magic String to enum
		fallState = new PlayerFallState(this, stateMachine, "jumpFall"); // TODO: Magic String to enum
		wallSlideState = new PlayerWallSlideState(this, stateMachine, "wallSlide"); // TODO: Magic String to enum
		wallJumpState = new PlayerWallJumpState(this, stateMachine, "jumpFall"); // TODO: Magic String to enum
	}

	private void OnEnable()
	{
		input.Enable();

		input.Player.Movement.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
		input.Player.Movement.canceled += ctx => moveInput = Vector2.zero;
	}

	private void Start()
	{
		stateMachine.Initialize(idleState);
	}

	private void Update()
	{
		HandleCollisionDetection();
		stateMachine.UpdateActiveState();
	}

	private void OnDisable()
	{
		input.Disable();
	}

	private void OnDrawGizmos()
	{
		Gizmos.DrawLine(transform.position, transform.position + new Vector3(0, -groundCheckDistance));
		Gizmos.DrawLine(transform.position, transform.position + new Vector3(wallCheckDistance * facingDir, 0));
	}

	#endregion

	#region Public Methods

	public void SetVelocity(float xVelocity, float yVelocity)
	{
		rb.linearVelocity = new Vector2(xVelocity, yVelocity);
		HandleFlip(xVelocity);
	}

	public void Flip()
	{
		transform.Rotate(0, 180, 0);
		facingRight = !facingRight;
		facingDir *= -1;
	}

	#endregion

	#region Private Helpers

	private void HandleFlip(float xVelocity)
	{
		if(xVelocity > 0 && !facingRight)
		{
			Flip();
		}
		else if(xVelocity < 0 && facingRight)
		{
			Flip();
		}
	}

	private void HandleCollisionDetection()
	{
		groundDetected = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, whatIsGround);
		wallDetected = Physics2D.Raycast(transform.position, Vector2.right * facingDir, wallCheckDistance, whatIsGround);
	}

	#endregion
}