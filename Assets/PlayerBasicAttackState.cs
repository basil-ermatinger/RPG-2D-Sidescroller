using UnityEngine;

public class PlayerBasicAttackState : EntityState
{
	private float _attackVelocityTimer;

	private const int FirstComboIndex = 1; // Start combo index with number 1, this parameter is used in the Animator
	private int _comboIndex = 1;
	private int _comboLimit = 3;

	private float _lastTimeAttacked;

	public PlayerBasicAttackState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
	{
		if(_comboLimit != _player.AttackVelocity.Length)
		{
			_comboLimit = _player.AttackVelocity.Length;
		}
	}

	public override void Enter()
	{
		base.Enter();

		ResetComboIndexIfNeeded();

		_anim.SetInteger("basicAttackIndex", _comboIndex);
		ApplyAttackVelocity();
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

	public override void Exit()
	{
		base.Exit();
		
		_comboIndex++;
		_lastTimeAttacked = Time.time;
	}

	private void HandleAttackVelocity()
	{
		_attackVelocityTimer -= Time.deltaTime;

		if(_attackVelocityTimer < 0)
		{
			_player.SetVelocity(0, _rb.linearVelocityY);
		}
	}

	private void ApplyAttackVelocity()
	{
		Vector2 attackVelocity = _player.AttackVelocity[_comboIndex - 1];

		_attackVelocityTimer = _player.AttackVelocityDuration;
		_player.SetVelocity(attackVelocity.x * _player.FacingDir, attackVelocity.y);
	}

	private void ResetComboIndexIfNeeded()
	{
		if(_comboIndex > _comboLimit || Time.time > _lastTimeAttacked + _player.comboResetTime)
		{
			Debug.Log("True");
			_comboIndex = FirstComboIndex;
		}
		Debug.Log("False");
	}
}
