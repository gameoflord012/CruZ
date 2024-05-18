namespace CruZ.GameEngine.GameSystem.Scene
{
    public class GameSceneDecorator
    {
        public GameSceneDecorator(GameScene gameScene)
        {
            _gameScene = gameScene;
            OnInitialize();
        }

        protected virtual void OnInitialize()
        {

        }

        public GameScene GameScene
        {
            get => _gameScene;
        }

        public TransformEntity RootEntity
        {
            get => _gameScene.RootEntity;
        }

        private GameScene _gameScene;
    }
}
