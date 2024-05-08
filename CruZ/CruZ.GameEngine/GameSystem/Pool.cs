using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CruZ.GameEngine.GameSystem
{
    public abstract class Pool
    {
        public abstract void ReturnPoolObject(IPoolObject obj);
    }

    public class Pool<T> : Pool
    {
        Func<IPoolObject> _creationFunc;

        public Pool(Func<IPoolObject> creationFunc, int capacity = 100)
        {
            _pool = new Stack<T>(capacity);
            _creationFunc = creationFunc;
        }

        public override void ReturnPoolObject(IPoolObject obj)
        {
            _pool.Push((T)obj);
            obj.Pool = this;
            obj.OnReturnToPool();
        }

        public T Pop()
        {
            if (_pool.Count == 0)
            {
                IPoolObject obj = _creationFunc.Invoke();
                ReturnPoolObject(obj);
            }

            return _pool.Pop();
        }

        Stack<T> _pool;
    }
}
