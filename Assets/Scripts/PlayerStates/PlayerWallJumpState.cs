using UnityEngine;

namespace Rpg2dSidescroller
{
	public class PlayerWallJumpState : EntityState
	{
		public PlayerWallJumpState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
		{
		}

		public override void Enter()
		{
			base.Enter();

			_player.SetVelocity(_player.WallJumpForce.x * -_player.FacingDir, _player.WallJumpForce.y);
		}

		public override void Update()
		{
			base.Update();

			if(_rb.linearVelocityY < 0)
			{
				_stateMachine.ChangeState(_player.FallState);
			}

			if(_player.WallDetected)
			{
				_stateMachine.ChangeState(_player.WallSlideState);
			}
		}
	}
}
