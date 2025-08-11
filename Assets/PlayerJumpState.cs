public class PlayerJumpState : PlayerAiredState
{
	public PlayerJumpState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
	{
	}

	public override void Enter()
	{
		base.Enter();

		_player.SetVelocity(_rb.linearVelocityX, _player.JumpForce);
	}

	public override void Update()
	{
		base.Update();

		if(_rb.linearVelocityY < 0)
		{
			_stateMachine.ChangeState(_player.FallState);
		}
	}
}