using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
	// Component References
	public Animator Anim { get; private set; }
	public Rigidbody2D Rb { get; private set; }

	public PlayerInputSet Input { get; private set; }
	private StateMachine _stateMachine;

	// Player States
	public PlayerIdleState IdleState { get; private set; }
	public PlayerMoveState MoveState { get; private set; }
	public PlayerJumpState JumpState { get; private set; }
	public PlayerFallState FallState { get; private set; }
	public PlayerWallSlideState WallSlideState { get; private set; }
	public PlayerWallJumpState WallJumpState { get; private set; }
	public PlayerDashState DashState { get; private set; }
	public PlayerBasicAttackState BasicAttackState { get; private set; }
	public PlayerJumpAttackState JumpAttackState { get; private set; }

	// Attack Settings
	[field: Header("Attack details")]
	
	[field: SerializeField] 
	public Vector2[] AttackVelocity { get; private set; }

	[field: SerializeField]
	public Vector2 JumpAttackVelocity { get; private set; }
	
	[field: SerializeField] 
	public float AttackVelocityDuration { get; private set; }
	
	[field: SerializeField] 
	public float comboResetTime { get; private set; }

	private Coroutine _queuedAttackCo;

	// Movement Settings
	[field: Header("Movement details")]
	
	[field: SerializeField] 
	public float MoveSpeed { get; private set; }
	
	[field: SerializeField] 
	public float JumpForce { get; private set; }
	
	[field: SerializeField] 
	public Vector2 WallJumpForce { get; private set; }

	[field: SerializeField]
	[field: Range(0, 1)]
	public float InAirMoveMultiplier { get; private set; }
	
	[field: SerializeField]
	[field: Range(0, 1)]
	public float WallSlideSlowMultiplier { get; private set; }
	
	[field: SerializeField, Space]
	public float DashDuration { get; private set; }
	
	[field: SerializeField] 
	public float DashSpeed { get; private set; }

	private bool _facingRight = true;
	public int FacingDir { get; private set; } = 1; // TODO: Better make this an enum with a value of right = 1 and left = -1 / other alternative would be to use the facingRight variable instead
	public Vector2 MoveInput { get; private set; }

	// Collision Settings
	[Header("Collision detection")]
	
	[SerializeField] 
	private float _groundCheckDistance;
	
	[SerializeField] 
	private float _wallCheckDistance;
	
	[SerializeField] 
	private LayerMask _whatIsGround;

	[SerializeField]
	private Transform _primaryWallCheck;

	[SerializeField]
	private Transform _secondaryWallCheck;
	
	public bool GroundDetected { get; private set; }
	public bool WallDetected { get; private set; }

	#region Unity Lifecycle

	private void Awake()
	{
		Anim = GetComponentInChildren<Animator>();
		Rb = GetComponent<Rigidbody2D>();

		_stateMachine = new StateMachine();
		Input = new PlayerInputSet();

		IdleState = new PlayerIdleState(this, _stateMachine, "idle");
		MoveState = new PlayerMoveState(this, _stateMachine, "move");
		JumpState = new PlayerJumpState(this, _stateMachine, "jumpFall");
		FallState = new PlayerFallState(this, _stateMachine, "jumpFall");
		WallSlideState = new PlayerWallSlideState(this, _stateMachine, "wallSlide");
		WallJumpState = new PlayerWallJumpState(this, _stateMachine, "jumpFall");
		DashState = new PlayerDashState(this, _stateMachine, "dash");
		BasicAttackState = new PlayerBasicAttackState(this, _stateMachine, "basicAttack");
		JumpAttackState = new PlayerJumpAttackState(this, _stateMachine, "jumpAttack");
	}

	private void OnEnable()
	{
		Input.Enable();

		Input.Player.Movement.performed += ctx => MoveInput = ctx.ReadValue<Vector2>();
		Input.Player.Movement.canceled += ctx => MoveInput = Vector2.zero;
	}

	private void Start()
	{
		_stateMachine.Initialize(IdleState);
	}

	private void Update()
	{
		HandleCollisionDetection();
		_stateMachine.UpdateActiveState();
	}

	private void OnDisable()
	{
		Input.Disable();
	}

	private void OnDrawGizmos()
	{
		// Gizmo for ground check
		Gizmos.DrawLine(transform.position, transform.position + new Vector3(0, -_groundCheckDistance));
		
		// Gizmo for wall check
		Gizmos.DrawLine(_primaryWallCheck.position, _primaryWallCheck.position + new Vector3(_wallCheckDistance * FacingDir, 0));
		Gizmos.DrawLine(_secondaryWallCheck.position, _secondaryWallCheck.position + new Vector3(_wallCheckDistance * FacingDir, 0));
	}

	#endregion

	#region Public Methods

	public void CallAnimationTrigger()
	{
		_stateMachine.CurrentState.CallAnimationTrigger();
	}

	public void SetVelocity(float xVelocity, float yVelocity)
	{
		Rb.linearVelocity = new Vector2(xVelocity, yVelocity);
		HandleFlip(xVelocity);
	}

	public void Flip()
	{
		transform.Rotate(0, 180, 0);
		_facingRight = !_facingRight;
		FacingDir *= -1;
	}

	public void EnterAttackStateWithDelay()
	{
		if(_queuedAttackCo != null)
		{
			StopCoroutine(_queuedAttackCo);
		}

		_queuedAttackCo = StartCoroutine(EnterAttackStateWithDelayCo());
	}

	#endregion

	#region Private Helpers

	private void HandleFlip(float xVelocity)
	{
		if(xVelocity > 0 && !_facingRight || xVelocity < 0 && _facingRight)
		{
			Flip();
		}
	}

	private void HandleCollisionDetection()
	{
		GroundDetected = Physics2D.Raycast(transform.position, Vector2.down, _groundCheckDistance, _whatIsGround);
		WallDetected = Physics2D.Raycast(_primaryWallCheck.position, Vector2.right * FacingDir, _wallCheckDistance, _whatIsGround) 
			&& Physics2D.Raycast(_secondaryWallCheck.position, Vector2.right * FacingDir, _wallCheckDistance, _whatIsGround);	
	}

	private IEnumerator EnterAttackStateWithDelayCo()
	{
		yield return new WaitForEndOfFrame();
		_stateMachine.ChangeState(BasicAttackState);
	}

	#endregion
}