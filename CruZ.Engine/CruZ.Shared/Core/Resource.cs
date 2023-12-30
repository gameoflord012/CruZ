using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CruZ
{
    internal class Resource
    {
        private Resource() {}
        private static Resource _instance;

        public static Resource Instance()
        {
            if(_instance == null)
            {
                _instance = new Resource();
            }
            return _instance;
        }

        private ContentManager ContentManager()
        {
            return CruZ.Instance().Content;
        }

        public T LoadResource<T>(string resourceName)
        {
            return ContentManager().Load<T>(resourceName);
        }
    }
}
