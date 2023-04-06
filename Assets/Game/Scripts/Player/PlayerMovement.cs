using Input;
using UnityEngine;

namespace Player
{
	public class PlayerMovement : MonoBehaviour
	{
		[SerializeField] private Transform _target;
		[SerializeField] private float _speed;
		[SerializeField] private float _rotateSpeed = 10f;
		[SerializeField] private PlayerAnimation _playerAnimation;

		private void Start()
		{
			InputHandler.OnChangeDirection += InputHandlerOnChangeDirectionHandler;
		}

		private void InputHandlerOnChangeDirectionHandler(Vector2 direction)
		{
			var movement = new Vector3(direction.x, 0.0f, direction.y);

			if(movement.magnitude > 0)
			{
				var newRotation = Quaternion.LookRotation(movement);
				_target.rotation = Quaternion.Slerp(transform.rotation, newRotation, Time.deltaTime * _rotateSpeed);
				//_target.LookAt(_target.position + movement);
			}
			_playerAnimation.SetSpeed(movement.magnitude);
			_target.position += movement * _speed * Time.deltaTime;
		}
	}
}