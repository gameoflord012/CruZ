using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;

using CruZ.GameEngine;
using CruZ.GameEngine.GameSystem.Scene;

namespace CruZ.Editor.Scene
{
    public static class SceneManager
    {
        static SceneManager()
        {
            g_sceneDecorators = [];
        }

        static void FindUserAssemblyScene()
        {
            Assembly userAssembly = EditorContext.GameAssembly;
            Type[] types = userAssembly.GetTypes();

            foreach (var type in types)
            {
                if(type.GetCustomAttribute(typeof(GameSceneDecoratorAttribute)) is not GameSceneDecoratorAttribute decoratorAttr)
                {
                    continue;
                }

                Trace.Assert(type.IsAssignableTo(typeof(GameSceneDecorator)), "Make sure it is a decorator");
                g_sceneDecorators[Path.Combine(decoratorAttr.Id, type.Name)] = type;
            }
        }

        public static GameSceneDecorator CreateDecorator(string sceneName)
        {
            FindUserAssemblyScene();

            if (g_sceneDecorators.TryGetValue(sceneName, out Type? sceneType))
            {
                return (GameSceneDecorator)Activator.CreateInstance(sceneType, [GameScene.Create()])!;
            }
            else
            {
                throw new ArgumentException($"{sceneName} not available");
            }
        }

        public static IReadOnlyCollection<string> GetAvalableSceneName()
        {
            FindUserAssemblyScene();

            return g_sceneDecorators.Keys;
        }

        private static Dictionary<string, Type> g_sceneDecorators;
    }
}
