public class PlayerDashState : EntityState
{
	private float _originalGravityScale;
	private int _dashDir;

	public PlayerDashState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
	{
	}

	public override void Enter()
	{
		base.Enter();

		_dashDir = player.facingDir;
		stateTimer = player.dashDuration;

		_originalGravityScale = rb.gravityScale;
		rb.gravityScale = 0;
	}

	public override void Update()
	{
		base.Update();

		CancelDashIfNeeded();

		player.SetVelocity(player.dashSpeed * _dashDir, 0);

		if(stateTimer < 0)
		{
			stateMachine.ChangeState(player.groundDetected ? player.idleState : player.fallState);
		}
	}

	public override void Exit()
	{
		base.Exit();
		player.SetVelocity(0, 0);
		rb.gravityScale = _originalGravityScale;
	}

	private void CancelDashIfNeeded()
	{
		if(player.wallDetected)
		{
			stateMachine.ChangeState(player.groundDetected ? player.idleState : player.wallSlideState);
		}
	}
}
