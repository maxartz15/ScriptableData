using System;
using UnityEngine;

namespace ScriptableData
{
	public class ScriptableData<T0> : ScriptableObject
	{
		public event Action<T0> OnValueChangedEvent;

		private T0 _value;
		public T0 Value
		{
			get
			{
				return _value;
			}
			set
			{
				_value = value;
				OnValueChangedEvent?.Invoke(_value);
			}
		}

		public void SetWithoutNotify(T0 value)
		{
			_value = value;
		}

		public void Invoke(T0 value)
		{
			_value = value;
			OnValueChangedEvent?.Invoke(value);
		}
	}

	public class ScriptableData<T0, T1> : ScriptableObject
	{
		public event Action<T0, T1> OnValueChangedEvent;

		private T0 _value;
		public T0 Value
		{
			get
			{
				return _value;
			}
			set
			{
				_value = value;
				OnValueChangedEvent?.Invoke(_value, _value1);
			}
		}

		private T1 _value1;
		public T1 Value1
		{
			get
			{
				return _value1;
			}
			set
			{
				_value1 = value;
				OnValueChangedEvent?.Invoke(_value, _value1);
			}
		}

		public void SetWithoutNotify(T0 value)
		{
			_value = value;
		}

		public void SetWithoutNotify(T0 value, T1 value1)
		{
			_value = value;
			_value1 = value1;
		}

		public void Invoke(T0 value, T1 value1)
		{
			Value = value;
			Value1 = value1;
			OnValueChangedEvent?.Invoke(value, value1);
		}
	}

	public class ScriptableData<T0, T1, T2> : ScriptableObject
	{
		public event Action<T0, T1, T2> OnValueChangedEvent;

		private T0 _value;
		public T0 Value
		{
			get
			{
				return _value;
			}
			set
			{
				_value = value;
				OnValueChangedEvent?.Invoke(_value, _value1, _value2);
			}
		}

		private T1 _value1;
		public T1 Value1
		{
			get
			{
				return _value1;
			}
			set
			{
				_value1 = value;
				OnValueChangedEvent?.Invoke(_value, _value1, _value2);
			}
		}

		private T2 _value2;
		public T2 Value2
		{
			get
			{
				return _value2;
			}
			set
			{
				_value2 = value;
				OnValueChangedEvent?.Invoke(_value, _value1, _value2);
			}
		}

		public void SetWithoutNotify(T0 value)
		{
			_value = value;
		}

		public void SetWithoutNotify(T0 value, T1 value1)
		{
			_value = value;
			_value1 = value1;
		}

		public void SetWithoutNotify(T0 value, T1 value1, T2 value2)
		{
			_value = value;
			_value1 = value1;
			_value2 = value2;
		}

		public void Invoke(T0 value, T1 value1, T2 value2)
		{
			Value = value;
			Value1 = value1;
			Value2 = value2;
			OnValueChangedEvent?.Invoke(value, value1, value2);
		}
	}

	public class ScriptableData<T0, T1, T2, T3> : ScriptableObject
	{
		public event Action<T0, T1, T2, T3> OnValueChangedEvent;

		private T0 _value;
		public T0 Value
		{
			get
			{
				return _value;
			}
			set
			{
				_value = value;
				OnValueChangedEvent?.Invoke(_value, _value1, _value2, _value3);
			}
		}

		private T1 _value1;
		public T1 Value1
		{
			get
			{
				return _value1;
			}
			set
			{
				_value1 = value;
				OnValueChangedEvent?.Invoke(_value, _value1, _value2, _value3);
			}
		}

		private T2 _value2;
		public T2 Value2
		{
			get
			{
				return _value2;
			}
			set
			{
				_value2 = value;
				OnValueChangedEvent?.Invoke(_value, _value1, _value2, _value3);
			}
		}

		private T3 _value3;
		public T3 Value3
		{
			get
			{
				return _value3;
			}
			set
			{
				_value3 = value;
				OnValueChangedEvent?.Invoke(_value, _value1, _value2, _value3);
			}
		}

		public void SetWithoutNotify(T0 value)
		{
			_value = value;
		}

		public void SetWithoutNotify(T0 value, T1 value1)
		{
			_value = value;
			_value1 = value1;
		}

		public void SetWithoutNotify(T0 value, T1 value1, T2 value2)
		{
			_value = value;
			_value1 = value1;
			_value2 = value2;
		}

		public void SetWithoutNotify(T0 value, T1 value1, T2 value2, T3 value3)
		{
			_value = value;
			_value1 = value1;
			_value2 = value2;
			_value3 = value3;
		}

		public void Invoke(T0 value, T1 value1, T2 value2, T3 value3)
		{
			Value = value;
			Value1 = value1;
			Value2 = value2;
			Value3 = value3;
			OnValueChangedEvent?.Invoke(value, value1, value2, value3);
		}
	}
}