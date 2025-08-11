public class PlayerGroundedState : EntityState
{
	public PlayerGroundedState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
	{
	}

	public override void Update()
	{
		base.Update();

		if(_rb.linearVelocityY < 0 && !_player.GroundDetected)
		{
			_stateMachine.ChangeState(_player.FallState);
		}

		if(_input.Player.Jump.WasPerformedThisFrame())
		{
			_stateMachine.ChangeState(_player.JumpState);
		}
	}
}