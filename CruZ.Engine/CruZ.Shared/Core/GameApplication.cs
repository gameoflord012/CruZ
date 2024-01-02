using CruZ.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Entities;
using System;
using System.Collections.Generic;

namespace CruZ
{
    public abstract class GameApplication
    {
        public ContentManager       Content { get => Core.Instance.Content; }
        public GraphicsDevice       GraphicsDevice { get => Core.Instance.GraphicsDevice; }

        public GameApplication()
        {
            Core.OnInitialize   += Initialize;
            Core.OnUpdate       += Update;
            Core.OnLoadContent  += LoadContent;
            Core.OnEndRun       += EndRun;
            Core.OnExit         += Exit;
            Core.OnDraw         += Draw;

            Core.Instance.Run();
        }

        public virtual void Initialize() { }
        public virtual void Update(GameTime gameTime) { }

        protected virtual void  Draw(GameTime gameTime) { }
        protected virtual void  Exit(object sender, EventArgs args) { }
        protected virtual void  EndRun() { }
        protected virtual void  LoadContent() { }

        //public void SetActiveTemplate(string templateFilePath)
        //{
        //    if (!_avaiableTemplates.ContainsKey(templateFilePath))
        //        throw new(string.Format("There is is {0} template", templateFilePath));

        //    _activeTemplate = _avaiableTemplates[templateFilePath];
        //}

        //public void AddAvaiableTemplate(string filePath, EntityTemplate template)
        //{
        //    _avaiableTemplates[filePath] = template;
        //}

        //private EntityTemplate                      _activeTemplate;
        //private Dictionary<string, GameScene>  _sceneCaches;
    }
}
