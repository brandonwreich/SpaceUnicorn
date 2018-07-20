using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceUnicorn.Screens
{
	public class StartScreen : GameScreen
	{
        #region Variables 

        MenuComponent _menuComponent;
		Texture2D _image;
		Rectangle _imageRectangle;

		public int SelectedIndex
		{
			get{ return _menuComponent.SelectedIndex; }
			set{ _menuComponent.SelectedIndex = value; }
		}

        #endregion

        public StartScreen(Game game, SpriteBatch spriteBatch, SpriteFont spriteFont, Texture2D image) : base(game, spriteBatch)
		{
			string[] menuItems = { "End Game", "Press Enter to Start", "Press Space For Game Conrols" };
			_menuComponent = new MenuComponent(game, spriteBatch, spriteFont, menuItems);
			Components.Add(_menuComponent);
			_image = image;
			_imageRectangle = new Rectangle(0, 0, Game.Window.ClientBounds.Width, Game.Window.ClientBounds.Height);
		}

        #region Draw

        public override void Draw(GameTime gameTime)
		{
			SpriteBatch.Draw(_image, _imageRectangle, Color.White);
			base.Draw(gameTime);
		}

        #endregion
    }
}