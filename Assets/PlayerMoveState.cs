using UnityEngine;

public class PlayerMoveState : EntityState
{
	public PlayerMoveState(Player player, StateMachine stateMachine, string stateName) : base(player, stateMachine, stateName)
	{
	}

	public override void Update()
	{
		base.Update();

		if(player.moveInput.x == 0)
		{
			stateMachine.ChangeState(player.idleState);
		}
	}
}