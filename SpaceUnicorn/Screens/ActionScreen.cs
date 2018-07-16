using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpaceUnicorn.Screens
{
	public class ActionScreen : GameScreen
	{
        #region Variables

        KeyboardState _keyboardState;
		Texture2D _image;
		Rectangle _imageRectangle;

        #endregion

        public ActionScreen(Game game, SpriteBatch spriteBatch, Texture2D image) : base(game, spriteBatch)
		{
			_image = image;
			_imageRectangle = new Rectangle(0, 0, Game.Window.ClientBounds.Width, Game.Window.ClientBounds.Height);
		}

        #region Update

        public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);

			_keyboardState = Keyboard.GetState();

			if (_keyboardState.IsKeyDown(Keys.Escape))
			{
				_game.Exit();
			}
		}

        #endregion

        #region Draw

        public override void Draw(GameTime gameTime)
		{
			_spriteBatch.Draw(_image, _imageRectangle, Color.White);
			base.Draw(gameTime);
		}

        #endregion
    }
}