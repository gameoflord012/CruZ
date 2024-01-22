using CruZ.Editor.Bindings;
using CruZ.Editor.Utility;
using System;
using System.Collections.Generic;

namespace CruZ.Editor.Binding
{
    class PropertyBinding<T>
    {
        public PropertyBinding(IProvideBinding bindingTarget, string propertyName)
        {
            _propertyName = propertyName;
            _bindingTarget = bindingTarget;
            _propertyType = typeof(T);

            _getter = Helper.GetPropertyGetter(_bindingTarget, propertyName, _propertyType);
            _setter = Helper.GetPropertySetter(_bindingTarget, propertyName, _propertyType);
        }

        public T Get()
        {
            return (T)_getter.Invoke();
        }

        public void Set(T value)
        {
            _setter.Invoke(value);
        }

        public void RegisterChangeEvent(Action<object> register)
        {
            _registedListeners[register] = 
                _bindingTarget.GetRegistedChangeEvent(_propertyName, _propertyType, register);
        }

        public void UnregisterChangeEvent(Action<object> unregister)
        {
            _bindingTarget.RemoveRegistedChangeEvent(
                _propertyName, _propertyType, _registedListeners[unregister]);
        }

        Type             _propertyType;
        string           _propertyName;
        IProvideBinding  _bindingTarget;

        Func<object>    _getter;
        Action<object>  _setter;

        Dictionary<Action<object>, object> _registedListeners = [];
    }

    class Binding
    {
        public Binding(IProvideBinding binding)
        {
            _binding = binding;
        }

        public PropertyBinding<T> Property<T>(string propertyName)
        {
            try
            {
                return new PropertyBinding<T>(_binding, propertyName);
            }
            catch (System.Exception e)
            {

                throw new System.Exception($"Property {propertyName} is invalid in target {_binding}", e);
            }
        }

        IProvideBinding _binding;
    }
}