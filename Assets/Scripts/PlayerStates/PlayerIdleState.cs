namespace Rpg2dSidescroller
{
	public class PlayerIdleState : PlayerGroundedState
	{
		public PlayerIdleState(Player player, StateMachine stateMachine, string stateName) : base(player, stateMachine, stateName)
		{
		}

		public override void Enter()
		{
			base.Enter();

			_player.SetVelocity(0, _rb.linearVelocityY);
		}

		public override void Update()
		{
			base.Update();

			if(_player.MoveInput.x == _player.FacingDir && _player.WallDetected)
			{
				return;
			}

			if(_player.MoveInput.x != 0)
			{
				_stateMachine.ChangeState(_player.MoveState);
			}
		}
	}
}