using UnityEngine;

public abstract class EntityState
{
	protected Player player; // TODO: Cross-Reference aufl�sen
	protected StateMachine stateMachine;
	protected string animBoolName;

	protected Animator anim;
	protected Rigidbody2D rb;
	protected PlayerInputSet input;

	protected float stateTimer;

	public EntityState(Player player, StateMachine stateMachine, string animBoolName)
	{
		this.player = player;
		this.stateMachine = stateMachine;
		this.animBoolName = animBoolName;

		anim = player.anim;
		rb = player.rb;
		input = player.input;
	}

	public virtual void Enter()
	{
		anim.SetBool(animBoolName, true);
	}

	public virtual void Update()
	{
		stateTimer -= Time.deltaTime;
		anim.SetFloat("yVelocity", rb.linearVelocityY); // TODO: Magic String to Enum

		if(input.Player.Dash.WasPressedThisFrame() && CanDash())
		{
			stateMachine.ChangeState(player.dashState);
		}
	}

	public virtual void Exit()
	{
		anim.SetBool(animBoolName, false);
	}

	private bool CanDash()
	{
		if(player.wallDetected || stateMachine.currentState == player.dashState)
		{
			return false;
		}

		return true;
	}
}