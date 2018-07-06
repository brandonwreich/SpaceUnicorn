using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpaceUnicorn
{
	public class ActionScreen : GameScreen
	{
        #region Variables

        KeyboardState keyboardState;
		Texture2D image;
		Rectangle imageRectangle;

        #endregion

        public ActionScreen(Game game, SpriteBatch spriteBatch, Texture2D image) : base(game, spriteBatch)
		{
			this.image = image;
			imageRectangle = new Rectangle(0, 0, Game.Window.ClientBounds.Width, Game.Window.ClientBounds.Height);
		}

        #region Update

        public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);

			keyboardState = Keyboard.GetState();

			if (keyboardState.IsKeyDown(Keys.Escape))
			{
				game.Exit();
			}
		}

        #endregion

        #region Draw

        public override void Draw(GameTime gameTime)
		{
			spriteBatch.Draw(image, imageRectangle, Color.White);
			base.Draw(gameTime);
		}

        #endregion
    }
}