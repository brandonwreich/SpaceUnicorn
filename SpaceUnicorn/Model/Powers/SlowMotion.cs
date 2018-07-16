using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceUnicorn.Model.Powers
{
	public class SlowMotion
	{
        #region Variables

        // Image representing the power
        private Texture2D _texture;
		public Texture2D Texture
		{
			get { return _texture; }
			set { _texture = value; }
		}
		// Position of the power relative to the upper left side of the screen
		public Vector2 Position;

		// State of the power
		private bool _active;
		public bool Active
		{
			get { return _active; }
			set { _active = value; }
		}

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
		private float _slowMotionMoveSpeed;

        #endregion

        public SlowMotion()
		{

		}

        #region Initialize

        public void Initialize(Viewport viewport, Texture2D texture, Vector2 position)
		{
			this._texture = texture;
			this.Position = position;
			this._viewport = viewport;

			_active = true;

			_slowMotionMoveSpeed = 6f;
		}

        #endregion

        #region Update

        public void Update(GameTime gameTime)
		{
			Position.X -= _slowMotionMoveSpeed;

			if (Position.X < -Width)
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
