using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceUnicorn.Model
{
	public class HealthBoost
	{
        #region Variables

        // Image representing the power
        private Texture2D texture;
		public Texture2D Texture
		{
			get { return texture; }
			set { texture = value; }
		}
		// Position of the power relative to the upper left side of the screen
		public Vector2 Position;

		// State of the power
		private bool active;
		public bool Active
		{
			get { return active; }
			set { active = value; }
		}

		// Represents the viewable boundary of the game
		private Viewport viewport;

		// Get the width of the power
		public int Width
		{
			get { return Texture.Width; }
		}

		// Get the height of the power
		public int Height
		{
			get { return Texture.Height; }
		}

		// Determines how fast the power moves
		private float healthPowerUpMoveSpeed;

        #endregion

        public HealthBoost()
		{

		}

        #region Initialize

        public void Initialize(Viewport viewport, Texture2D texture, Vector2 position)
		{
			this.texture = texture;
			Position = position;
			this.viewport = viewport;

			active = true;

			healthPowerUpMoveSpeed = 6f;
		}

        #endregion

        #region Update

        public void Update(GameTime gameTime)
		{
			Position.X -= healthPowerUpMoveSpeed;

			if(Position.X < -Width)
			{
				Active = false;
			}
		}

        #endregion

        #region Draw

        public void Draw(SpriteBatch spriteBatch)
		{
			spriteBatch.Draw(Texture, Position, null, Color.White, 0f, new Vector2(Width / 2, Height / 2), 1f, SpriteEffects.None, 0f);
		}

        #endregion
    }
}