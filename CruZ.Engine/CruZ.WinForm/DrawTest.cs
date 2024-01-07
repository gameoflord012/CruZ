using Microsoft.Xna.Framework;

namespace CruZ.WinForm
{
    internal class DrawTest : MonoGame.Forms.NET.Controls.MonoGameControl
    {
        protected override void Initialize()
        {
        }

        protected override void Update(GameTime gameTime)
        {

        }

        protected override void Draw()
        {
            Editor.spriteBatch.Begin();

            Editor.spriteBatch.DrawString(Editor.Font, WelcomeMessage, new Vector2(
                (500 / 2) - (Editor.Font.MeasureString(WelcomeMessage).X / 2),
                (500 / 2) - (Editor.FontHeight / 2)),
                Microsoft.Xna.Framework.Color.White);

            Editor.spriteBatch.End();
        }

        string WelcomeMessage = "Hello MonoGame.Forms!";
    }
}
