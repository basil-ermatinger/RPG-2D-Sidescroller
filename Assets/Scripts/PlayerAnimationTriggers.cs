using UnityEngine;

public class PlayerAnimationTriggers : MonoBehaviour
{
	private Player _player;

	#region Unity Lifecycle

	private void Awake()
	{
		_player = GetComponentInParent<Player>();
	}

	#endregion

	#region Public Methods
	#endregion

	#region Private Methods

	private void CurrentStateTrigger()
	{
		_player.CallAnimationTrigger();
	}

	#endregion
}