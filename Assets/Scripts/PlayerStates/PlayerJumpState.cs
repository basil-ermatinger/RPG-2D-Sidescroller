namespace Rpg2dSidescroller
{
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

			// Check if player is not in jump attack state when transfer to fall state.
			if(_rb.linearVelocityY < 0 && _stateMachine.CurrentState != _player.JumpAttackState)
			{
				_stateMachine.ChangeState(_player.FallState);
			}
		}
	}
}