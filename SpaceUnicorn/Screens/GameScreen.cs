using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceUnicorn.Screens
{
	public abstract class GameScreen : DrawableGameComponent
    {
        #region Variables

        List<GameComponent> _components = new List<GameComponent>();
		protected Game _game;
		protected SpriteBatch SpriteBatch;

        public List<GameComponent> Components
		{
			get{ return _components; }
		}

        #endregion

        public GameScreen(Game game, SpriteBatch spriteBatch) : base(game)
		{
			_game = game;
			SpriteBatch = spriteBatch;
		}

        #region Update

        public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);

			foreach (GameComponent component in _components)
			{
				if (component.Enabled)
				{
					component.Update(gameTime);
				}
			}
		}

        #endregion

        #region Draw

        public override void Draw(GameTime gameTime)
		{
			base.Draw(gameTime);
			foreach (GameComponent component in _components)
			{
				if (component is DrawableGameComponent && ((DrawableGameComponent)component).Visible)
				{
					((DrawableGameComponent)component).Draw(gameTime);
				}
			}
		}

        #endregion

        #region Show/Hide

        public virtual void Show()
		{
			Visible = true;
			Enabled = true;

			foreach (GameComponent component in _components)
			{
				component.Enabled = true;
				if (component is DrawableGameComponent)
				{
					((DrawableGameComponent)component).Visible = true;
				}
			}
		}

		public virtual void Hide()
		{
			Visible = false;
			Enabled = false;

			foreach (GameComponent component in _components)
			{
				component.Enabled = false;
				if (component is DrawableGameComponent)
				{
					((DrawableGameComponent)component).Visible = false;
				}
			}
		}

        #endregion
    }
}