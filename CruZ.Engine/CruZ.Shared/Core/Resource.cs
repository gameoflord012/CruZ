using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CruZ
{
    public static class Content
    {
        private Content() {}
        private static Content _instance;

        public static Content Instance()
        {
            if(_instance == null)
            {
                _instance = new Content();
            }
            return _instance;
        }

        public static ContentManager GetManager()
        {
            return MGWrapper.Instance().Content;
        }

        public static T LoadResource<T>(string resourceName)
        {
            return GetManager()().Load<T>(resourceName);
        }
    }
}
