using UnityEngine;

public abstract class EntityState
{
	protected Player _player; // TODO: Cross-Reference auflösen
	protected StateMachine _stateMachine;
	protected string _animBoolName;

	protected Animator _anim;
	protected Rigidbody2D _rb;
	protected PlayerInputSet _input;

	protected float _stateTimer;

	public EntityState(Player player, StateMachine stateMachine, string animBoolName)
	{
		this._player = player;
		this._stateMachine = stateMachine;
		this._animBoolName = animBoolName;

		_anim = player.Anim;
		_rb = player.Rb;
		_input = player._input;
	}

	public virtual void Enter()
	{
		_anim.SetBool(_animBoolName, true);
	}

	public virtual void Update()
	{
		_stateTimer -= Time.deltaTime;
		_anim.SetFloat("yVelocity", _rb.linearVelocityY); // TODO: Magic String to Enum

		if(_input.Player.Dash.WasPressedThisFrame() && CanDash())
		{
			_stateMachine.ChangeState(_player.DashState);
		}
	}

	public virtual void Exit()
	{
		_anim.SetBool(_animBoolName, false);
	}

	private bool CanDash()
	{
		if(_player.WallDetected || _stateMachine._currentState == _player.DashState)
		{
			return false;
		}

		return true;
	}
}