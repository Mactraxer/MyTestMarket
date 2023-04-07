using System;
using System.Collections;
using UnityEngine;

namespace MyExtensions
{
	public static class MyExtenstions
	{
		public static void ActionWithDelay(this MonoBehaviour monoBehaviour, float delay, Action action)
		{
			monoBehaviour.StartCoroutine(CoroutineDelay(delay, action));
		}

		private static IEnumerator CoroutineDelay(float delay, Action action)
		{
			yield return new WaitForSeconds(delay);
			action?.Invoke();
		}
	}
}