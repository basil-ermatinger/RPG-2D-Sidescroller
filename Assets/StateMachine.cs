public class StateMachine
{
	public EntityState _currentState { get; private set; }

	public void Initialize(EntityState startState)
	{
		_currentState = startState;
		_currentState.Enter();
	}

	public void ChangeState(EntityState newState)
	{
		_currentState.Exit();
		_currentState = newState;
		_currentState.Enter();
	}

	public void UpdateActiveState()
	{
		_currentState.Update();
	}
}