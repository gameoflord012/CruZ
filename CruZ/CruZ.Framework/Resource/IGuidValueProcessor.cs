namespace CruZ.Framework.Resource
{
    interface IGuidValueProcessor<T>
    {
        T GetProcessedGuidValue(T value);
    }
}
