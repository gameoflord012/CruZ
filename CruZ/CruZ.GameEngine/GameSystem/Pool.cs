using System;
using System.Collections.Generic;

namespace CruZ.GameEngine.GameSystem
{
    public abstract class Pool
    {
        public abstract void ReturnPoolObject(IPoolObject obj);

        public abstract bool Contains(IPoolObject obj);
    }

    public class Pool<T> : Pool, IDisposable where T : IPoolObject
    {
        private readonly Func<T> _creationFunc;

        public Pool(Func<T> creationFunc, int capacity = 100)
        {
            _pool = new(capacity);
            _pops = new();
            _creationFunc = creationFunc;
        }

        public override void ReturnPoolObject(IPoolObject poolObject)
        {
            if(Contains(poolObject)) return;

            poolObject.OnDisabled();

            _pops.Remove((T)poolObject);
            _pool.Push(poolObject);
        }

        public T Pop()
        {
            IPoolObject pop = _pool.Count > 0 ? _pool.Pop() : Create();
            _pops.Add((T)pop);

            return (T)pop;
        }

        public override bool Contains(IPoolObject obj)
        {
            return _pool.Contains(obj);
        }

        private T Create()
        {
            T value = _creationFunc.Invoke();
            value.Pool = this;
            value.OnDisabled();

            return value;
        }

        public int PopCount
        {
            get => _pops.Count;
        }

        public IReadOnlyCollection<T> Pops
        {
            get => _pops;
        }

        private Stack<IPoolObject> _pool;
        private HashSet<T> _pops;

        public void Dispose()
        {
            _pool.Clear();
        }
    }
}
