public class PlayerMoveState : PlayerGroundedState
{
	public PlayerMoveState(Player player, StateMachine stateMachine, string stateName) : base(player, stateMachine, stateName)
	{
	}

	public override void Update()
	{
		base.Update();

		if(_player.MoveInput.x == 0)
		{
			_stateMachine.ChangeState(_player.IdleState);
		}

		_player.SetVelocity(_player.MoveInput.x * _player.MoveSpeed, _rb.linearVelocityY);
	}
}