public class PlayerAiredState : EntityState
{
	public PlayerAiredState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
	{
	}

	public override void Update()
	{
		base.Update();

		if(_player.MoveInput.x != 0)
		{
			_player.SetVelocity(_player.MoveInput.x * (_player.MoveSpeed * _player.InAirMoveMultiplier), _rb.linearVelocityY);
		}
	}
}
