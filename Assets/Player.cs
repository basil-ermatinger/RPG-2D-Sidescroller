using UnityEngine;

public class Player : MonoBehaviour
{
	public Animator anim { get; private set; }
	public Rigidbody2D rb { get; private set; }

	public PlayerInputSet input { get; private set; }
	private StateMachine stateMachine;

	public PlayerIdleState idleState { get; private set; }
	public PlayerMoveState moveState { get; private set; }
	public PlayerJumpState jumpState { get; private set; }
	public PlayerFallState fallState { get; private set; }

	[field: Header("Movement details")]
	[field: SerializeField] public float moveSpeed { get; private set; }
	[field: SerializeField] public float jumpForce { get; private set; }
	public Vector2 moveInput { get; private set; }

	private bool facingRight = true;

	public void SetVelocity(float xVelocity, float yVelocity)
	{
		rb.linearVelocity = new Vector2(xVelocity, yVelocity);
		HandleFlip(xVelocity);
	}

	private void HandleFlip(float xVelocity)
	{
		if((xVelocity > 0) != facingRight)
		{
			Flip();
		}
	}

	private void Flip()
	{
		transform.Rotate(0, 180, 0);
		facingRight = !facingRight;
	}

	private void OnEnable()
	{
		input.Enable();

		input.Player.Movement.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
		input.Player.Movement.canceled += ctx => moveInput = Vector2.zero;
	}

	private void OnDisable()
	{
		input.Disable();
	}

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
	}

	private void Start()
	{
		stateMachine.Initialize(idleState);
	}

	private void Update()
	{
		stateMachine.UpdateActiveState();
	}
}