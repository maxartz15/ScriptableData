using System;
using UnityEngine;

namespace ScriptableData
{
	[CreateAssetMenu(menuName = "ScriptableData/Event/Event", order = 147)]
	public class ScriptableEvent : ScriptableObject
	{
		public event Action OnScriptableEvent;

		public void Invoke()
		{
			OnScriptableEvent?.Invoke();
		}
	}

	public class ScriptableEvent<T0> : ScriptableObject
	{
		public event Action<T0> OnScriptableEvent;

		public void Invoke(T0 value)
		{
			OnScriptableEvent?.Invoke(value);
		}
	}

	public class ScriptableEvent<T0, T1> : ScriptableObject
	{
		public event Action<T0, T1> OnScriptableEvent;

		public void Invoke(T0 value, T1 value1)
		{
			OnScriptableEvent?.Invoke(value, value1);
		}
	}

	public class ScriptableEvent<T0, T1, T2> : ScriptableObject
	{
		public event Action<T0, T1, T2> OnScriptableEvent;

		public void Invoke(T0 value, T1 value1, T2 value2)
		{
			OnScriptableEvent?.Invoke(value, value1, value2);
		}
	}

	public class ScriptableEvent<T0, T1, T2, T3> : ScriptableObject
	{
		public event Action<T0, T1, T2, T3> OnScriptableEvent;

		public void Invoke(T0 value, T1 value1, T2 value2, T3 value3)
		{
			OnScriptableEvent?.Invoke(value, value1, value2, value3);
		}
	}
}