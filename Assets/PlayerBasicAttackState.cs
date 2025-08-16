using UnityEngine;

public class PlayerBasicAttackState : EntityState
{
	private float _attackVelocityTimer;
	private float _lastTimeAttacked;

	private bool _comboAttackQueued;
	private int _attackDir;
	private int _comboIndex = 1;
	private int _comboLimit = 3;	
	private const int FirstComboIndex = 1; // Start combo index with number 1, this parameter is used in the Animator

	public PlayerBasicAttackState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
	{
		if(_comboLimit != _player.AttackVelocity.Length)
		{
			Debug.LogWarning("Adjusted combo limi to match attack velocity array!");
			_comboLimit = _player.AttackVelocity.Length;
		}
	}

	public override void Enter()
	{
		base.Enter();

		_comboAttackQueued = false;

		ResetComboIndexIfNeeded();

		// Define attack direction according to input
		_attackDir = _player.MoveInput.x != 0 ? ((int)_player.MoveInput.x) : _player.FacingDir;

		_anim.SetInteger("basicAttackIndex", _comboIndex);
		ApplyAttackVelocity();
	}

	public override void Update()
	{
		base.Update();

		HandleAttackVelocity();

		if(_input.Player.Attack.WasPressedThisFrame())
		{
			QueueNextAttack();
		}

		if(_triggerCalled)
		{
			HandleStateExit();
		}
	}

	public override void Exit()
	{
		base.Exit();

		_comboIndex++;
		_lastTimeAttacked = Time.time;
	}

	private void HandleStateExit()
	{
		if(_comboAttackQueued)
		{
			_anim.SetBool(_animBoolName, false);
			_player.EnterAttackStateWithDelay();
		}
		else
		{
			_stateMachine.ChangeState(_player.IdleState);
		}
	}

	private void QueueNextAttack()
	{
		if(_comboIndex < _comboLimit)
		{
			_comboAttackQueued = true;
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

	private void ApplyAttackVelocity()
	{
		Vector2 attackVelocity = _player.AttackVelocity[_comboIndex - 1];

		_attackVelocityTimer = _player.AttackVelocityDuration;
		_player.SetVelocity(attackVelocity.x * _attackDir, attackVelocity.y);
	}

	private void ResetComboIndexIfNeeded()
	{
		if(_comboIndex > _comboLimit || Time.time > _lastTimeAttacked + _player.comboResetTime)
		{
			_comboIndex = FirstComboIndex;
		}
	}
}
