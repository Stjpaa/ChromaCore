using Godot;
using System;

namespace EventVariable
{
    public partial class Variable<T>
    {
        private T _value;

        public void SetValue(T value)
        {
            _value = value;

            OnValueChanged?.Invoke();
        }

        public T GetValue()
        {
            return _value;
        }

        public event Action OnValueChanged;
    }
}
