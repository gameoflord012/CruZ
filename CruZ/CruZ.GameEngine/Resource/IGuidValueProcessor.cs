namespace CruZ.GameEngine.Resource
{
    interface IGuidValueProcessor<T>
    {
        T GetProcessedGuidValue(T value);
    }
}
