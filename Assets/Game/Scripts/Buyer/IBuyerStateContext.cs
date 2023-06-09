﻿using System;
using UnityEngine;

namespace Buyers
{
	public interface IBuyerStateContext
	{
		public event Action<IBuyerStateContext> OnArrived;
		public event Action<IBuyerStateContext> OnReceiveAllProducts;
		public event Action<IBuyerStateContext> OnEndPay;

		public Vector3 ShelvingPosition { get; }
		public Vector3 BoxOfficePosition { get; }
		public Vector3 ExitPosition { get; }
		public bool ReadyToPay { get; }

		public void RouteTo(Vector3 position);
		public void NextState();
		void StartPay();
		void WaitQueue();
		void SetActiveStack(bool active);
	}
}