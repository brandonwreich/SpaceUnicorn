﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;


namespace SpaceUnicorn
{
	public abstract class GameScreen : Microsoft.Xna.Framework.DrawableGameComponent
	{
        #region Variables

        List<GameComponent> components = new List<GameComponent>();
		protected Game game;
		protected SpriteBatch spriteBatch;

        public List<GameComponent> Components
		{
			get{ return components; }
		}

        #endregion

        public GameScreen(Game game, SpriteBatch spriteBatch) : base(game)
		{
			this.game = game;
			this.spriteBatch = spriteBatch;
		}

		public override void Initialize()
		{
			base.Initialize();
		}

        #region Update

        public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);

			foreach (GameComponent component in components)
			{
				if (component.Enabled == true)
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
			foreach (GameComponent component in components)
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
			this.Visible = true;
			this.Enabled = true;

			foreach (GameComponent component in components)
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
			this.Visible = false;
			this.Enabled = false;

			foreach (GameComponent component in components)
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