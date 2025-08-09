public class PlayerWallSlideState : EntityState
{
	public PlayerWallSlideState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
	{
	}

	public override void Update()
	{
		base.Update();

		HandleWallSlide();

		if(input.Player.Jump.WasPressedThisFrame())
		{
			stateMachine.ChangeState(player.wallJumpState);
		}

		if(!player.wallDetected)
		{
			stateMachine.ChangeState(player.fallState);
		}

		if(player.groundDetected)
		{
			stateMachine.ChangeState(player.idleState);
			player.Flip();
		}
	}

	private void HandleWallSlide()
	{
		player.SetVelocity(
			player.moveInput.x, 
			player.moveInput.y < 0 ? rb.linearVelocityY : rb.linearVelocity.y * player.wallSlideSlowMultiplier);
	}
}
