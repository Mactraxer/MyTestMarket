using UnityEngine;

namespace Animation
{
	[RequireComponent(typeof(Animator))]
	public abstract class AnimationBase : MonoBehaviour
	{
		private Animator _animator;

		private void Start () 
		{
			_animator = GetComponent<Animator>();
		}

		public void SetFloat(int parameterHash, float value)
		{
			_animator.SetFloat(parameterHash, value);
		}
	}
}