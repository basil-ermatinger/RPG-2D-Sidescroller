namespace Rpg2dSidescroller
{
	public class PlayerWallSlideState : EntityState
	{
		public PlayerWallSlideState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
		{
		}

		public override void Update()
		{
			base.Update();

			HandleWallSlide();

			if(_input.Player.Jump.WasPressedThisFrame())
			{
				_stateMachine.ChangeState(_player.WallJumpState);
			}

			if(!_player.WallDetected)
			{
				_stateMachine.ChangeState(_player.FallState);
			}

			if(_player.GroundDetected)
			{
				_stateMachine.ChangeState(_player.IdleState);

				if(_player.FacingDir != _player.MoveInput.x)
				{
					_player.Flip();
				}
			}
		}

		private void HandleWallSlide()
		{
			_player.SetVelocity(
				_player.MoveInput.x,
				_player.MoveInput.y < 0 ? _rb.linearVelocityY : _rb.linearVelocity.y * _player.WallSlideSlowMultiplier);
		}
	}
}