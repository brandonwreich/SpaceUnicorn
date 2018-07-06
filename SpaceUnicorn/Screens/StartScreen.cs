using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceUnicorn
{
	public class StartScreen : GameScreen
	{
        #region Variables 

        MenuComponent menuComponent;
		Texture2D image;
		Rectangle imageRectangle;

		public int SelectedIndex
		{
			get{ return menuComponent.SelectedIndex; }
			set{ menuComponent.SelectedIndex = value; }
		}

        #endregion

        public StartScreen(Game game, SpriteBatch spriteBatch, SpriteFont spriteFont, Texture2D image) : base(game, spriteBatch)
		{
			string[] menuItems = { "End Game", "Press Enter to Start" };
			menuComponent = new MenuComponent(game, spriteBatch, spriteFont, menuItems);
			Components.Add(menuComponent);
			this.image = image;
			imageRectangle = new Rectangle(0, 0, Game.Window.ClientBounds.Width, Game.Window.ClientBounds.Height);
		}

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);
		}

        #region Draw

        public override void Draw(GameTime gameTime)
		{
			spriteBatch.Draw(image, imageRectangle, Color.White);
			base.Draw(gameTime);
		}

        #endregion
    }
}