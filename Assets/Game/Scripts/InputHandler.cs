using System;
using UnityEngine;

namespace Input
{
	public class InputHandler : MonoBehaviour
	{
		public static event Action<Vector2> OnChangeDirection;

		[SerializeField] private Joystick _joystick;

		private void Update()
		{
			OnChangeDirection?.Invoke(_joystick.Direction);
		}
	}
}