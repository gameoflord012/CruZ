namespace CruZ.Common.Resource
{
    interface IGuidValueProcessor<T>
    {
        T GetProcessedGuidValue(T value);
    }
}
