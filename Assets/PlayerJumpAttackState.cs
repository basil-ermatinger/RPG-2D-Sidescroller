using UnityEditor;

public class PlayerJumpAttackState : EntityState
{
	private bool _touchedGround;

	public PlayerJumpAttackState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
	{
	}

	public override void Enter()
	{
		base.Enter();

		_touchedGround = false;

		_player.SetVelocity(_player.JumpAttackVelocity.x * _player.FacingDir, _player.JumpAttackVelocity.y);
	}

	public override void Update()
	{
		base.Update();

		if(_player.GroundDetected && _touchedGround == false)
		{
			_touchedGround = true;
			_anim.SetTrigger("jumpAttackTrigger");
			_player.SetVelocity(0, _rb.linearVelocityY);
		}

		if(_triggerCalled && _player.GroundDetected)
		{
			_stateMachine.ChangeState(_player.IdleState);
		}
	}
}
