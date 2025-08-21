public class PlayerFallState : PlayerAiredState
{
	public PlayerFallState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
	{
	}

	public override void Update()
	{
		base.Update();

		if(_player.GroundDetected)
		{
			_stateMachine.ChangeState(_player.IdleState);
		}

		if(_player.WallDetected)
		{
			_stateMachine.ChangeState(_player.WallSlideState);
		}
	}
}