using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceUnicorn.Model
{
	public class MarshmallowLaser
	{
        #region Variables

        // Image representing the Marshmallow
        private Texture2D _texture;
		public Texture2D Texture
		{
			get { return _texture; }
			set { _texture = value; }
		}
		// Position of the marshmallow relative to the upper left side of the screen
		public Vector2 Position;

		// State of the marshmallow
		private bool _active;
		public bool Active
		{
			get { return _active; }
			set { _active = value; }
		}

		// The amount of damage the marshmallow can inflict
		private int _damage;
		public int Damage
		{
			get { return _damage; }
			set { _damage = value; }
		}

		// Represents the viewable boundary of the game
		private Viewport _viewport;


		// Get the width of the marshmallow
		public int Width
		{
			get { return Texture.Width; }
		}

		// Get the height of the marshmallow
		public int Height
		{
			get { return Texture.Height; }
		}

		// Determines how fast the marshmallow moves
		private float _marshmallowMoveSpeed;

        #endregion

        #region Initialize

        public void Initialize(Viewport viewport, Texture2D texture, Vector2 position)
		{
			_texture = texture;
			Position = position;
			_viewport = viewport;

			_active = true;

			_damage = 7;

			_marshmallowMoveSpeed = 20f;
		}

        #endregion

        #region Update

        public void Update()
		{
			// Projectiles always move to the right
			Position.X += _marshmallowMoveSpeed;

			// Deactivate the bullet if it goes out of screen
			if (Position.X + Texture.Width / 2 > _viewport.Width)
			{
				_active = false;
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