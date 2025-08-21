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

		_dashDir = _player.MoveInput.x != 0 ? ((int)_player.MoveInput.x) : _player.FacingDir; ;
		_stateTimer = _player.DashDuration;

		_originalGravityScale = _rb.gravityScale;
		_rb.gravityScale = 0;
	}

	public override void Update()
	{
		base.Update();

		CancelDashIfNeeded();

		_player.SetVelocity(_player.DashSpeed * _dashDir, 0);

		if(_stateTimer < 0)
		{
			_stateMachine.ChangeState(_player.GroundDetected ? _player.IdleState : _player.FallState);
		}
	}

	public override void Exit()
	{
		base.Exit();
		_player.SetVelocity(0, 0);
		_rb.gravityScale = _originalGravityScale;
	}

	private void CancelDashIfNeeded()
	{
		if(_player.WallDetected)
		{
			_stateMachine.ChangeState(_player.GroundDetected ? _player.IdleState : _player.WallSlideState);
		}
	}
}
