using Animation;
using UnityEngine;

namespace Player
{
	public class PlayerAnimation : AnimationBase
	{
		private static readonly int speedParamenter = Animator.StringToHash("Speed");

		public void SetSpeed(float speed)
		{
			SetFloat(speedParamenter, speed);
		}
	}
}