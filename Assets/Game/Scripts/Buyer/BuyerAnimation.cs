using Animation;
using UnityEngine;

namespace Buyers
{
	public class BuyerAnimation : AnimationBase
	{
		private static readonly int speedParamenter = Animator.StringToHash("Speed");

		public void SetSpeed(float value)
		{
			SetFloat(speedParamenter, value);
		}
	}
}