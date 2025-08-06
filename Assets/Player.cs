using UnityEngine;

public class Player : MonoBehaviour
{
	private StateMachine stateMachine;

	public PlayerIdleState idleState { get; private set; }
	public PlayerMoveState moveState { get; private set; }

	private void Awake()
	{
		stateMachine = new StateMachine();
		
		idleState = new PlayerIdleState(this, stateMachine, "idle"); // TODO: Magic String to enum
		moveState = new PlayerMoveState(this, stateMachine, "move"); // TODO: Magic String to enum
	}

	private void Start()
	{
		stateMachine.Initialize(idleState);
	}

	private void Update()
	{
		stateMachine.currentState.Update();
	}
}