using System;
using System.Collections.Generic;

using CruZ.Exception;

namespace CruZ.Resource
{
    interface IGuidValueProcessor<T>
    {
        T GetProcessedGuidValue(T value);
    }

    class GuidManager<T>
    {
        public GuidManager(IGuidValueProcessor<T> guidProcessor)
        {
            _guidProcessor = guidProcessor;
        }

        public Guid GenerateUniqueGuid()
        {
            Guid guid;
            do
            {
                guid = Guid.NewGuid();
            } while (IsConsumed(guid));

            return guid;
        }

        public void ConsumeGuid(Guid guid, T value)
        {
            value = _guidProcessor.GetProcessedGuidValue(value);
            if (IsConsumed(guid)) return;
            _getValueFromGuid[guid] = value;
            _getGuidFromValue[value] = guid;    
        }

        public T GetValue(Guid guid)
        {
            if (!_getValueFromGuid.ContainsKey(guid))
                throw new InvalidGuidException($"Can't find value for guid {guid}");
            return _getValueFromGuid[guid];
        }

        public Guid GetGuid(T value)
        {
            value = _guidProcessor.GetProcessedGuidValue(value);
            if (!_getGuidFromValue.ContainsKey(value))
                throw new InvalidGuidValueException($"Can't find guid for value {value}");
            return _getGuidFromValue[value];
        }

        public void RemovedGuid(Guid guid)
        {
            if (!IsConsumed(guid)) throw new ArgumentException("Guid not included");

            var path = _getValueFromGuid[guid];
            _getGuidFromValue.Remove(path);
            _getValueFromGuid.Remove(guid);
        }

        public void RemoveValue(T value)
        {
            value = _guidProcessor.GetProcessedGuidValue(value);
            if (!_getGuidFromValue.ContainsKey(value)) return;
            var guid = _getGuidFromValue[value];
            _getGuidFromValue.Remove(value);
            _getValueFromGuid.Remove(guid);
        }

        public bool IsConsumed(Guid guid)
        {
            return _getValueFromGuid.ContainsKey(guid);
        }

        Dictionary<Guid, T> _getValueFromGuid = new Dictionary<Guid, T>();
        Dictionary<T, Guid> _getGuidFromValue = new Dictionary<T, Guid>();
        IGuidValueProcessor<T> _guidProcessor;
    }
}
