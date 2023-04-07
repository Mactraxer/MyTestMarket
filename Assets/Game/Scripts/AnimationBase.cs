using DG.Tweening;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace Animation
{
	[RequireComponent(typeof(Animator))]
	public abstract class AnimationBase : MonoBehaviour
	{
		[SerializeField] private Rig _rig;

		private Animator _animator;

		private void Start()
		{
			_animator = GetComponent<Animator>();
		}

		protected void SetFloat(int parameterHash, float value)
		{
			_animator.SetFloat(parameterHash, value);
		}

		public void PickUp()
		{
			DOTween.To(value => _rig.weight = value, _rig.weight, 1f, 0.3f);
		}

		public void PickDown()
		{
			DOTween.To(value => _rig.weight = value, _rig.weight, 0f, 0.3f);
		}
	}
}