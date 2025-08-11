using UnityEngine;

public class PlayerBasicAttackState : EntityState
{
	private float _attackVelocityTimer;

	public PlayerBasicAttackState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
	{
	}

	public override void Enter()
	{
		base.Enter();

		GenerateAttackVelocity();
	}

	public override void Update()
	{
		base.Update();

		HandleAttackVelocity();

		if(_triggerCalled)
		{
			_stateMachine.ChangeState(_player.IdleState);
		}
	}

	private void HandleAttackVelocity()
	{
		_attackVelocityTimer -= Time.deltaTime;

		if(_attackVelocityTimer < 0)
		{
			_player.SetVelocity(0, _rb.linearVelocityY);
		}
	}

	private void GenerateAttackVelocity()
	{
		_attackVelocityTimer = _player.AttackVelocityDuration;
		_player.SetVelocity(_player.AttackVelocity.x * _player.FacingDir, _player.AttackVelocity.y);
	}
}
