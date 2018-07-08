using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceUnicorn.Model
{
	public class MarshmallowLaser
	{
        #region Variables

        // Image representing the Marshmallow
        private Texture2D _texture;
		public Texture2D _Texture
		{
			get { return _texture; }
			set { _texture = value; }
		}
		// Position of the marshmallow relative to the upper left side of the screen
		public Vector2 _Position;

		// State of the marshmallow
		private bool _active;
		public bool _Active
		{
			get { return _active; }
			set { _active = value; }
		}

		// The amount of damage the marshmallow can inflict
		private int _damage;
		public int _Damage
		{
			get { return _damage; }
			set { _damage = value; }
		}

		// Represents the viewable boundary of the game
		private Viewport _viewport;


		// Get the width of the marshmallow
		public int _Width
		{
			get { return _Texture.Width; }
		}

		// Get the height of the marshmallow
		public int _Height
		{
			get { return _Texture.Height; }
		}

		// Determines how fast the marshmallow moves
		private float _marshmallowMoveSpeed;

        #endregion

        public MarshmallowLaser()
		{

		}

        #region Initialize

        public void Initialize(Viewport viewport, Texture2D texture, Vector2 position)
		{
			this._texture = texture;
			this._Position = position;
			this._viewport = viewport;

			_active = true;

			_damage = 7;

			_marshmallowMoveSpeed = 20f;
		}

        #endregion

        #region Update

        public void Update()
		{
			// Projectiles always move to the right
			_Position.X += _marshmallowMoveSpeed;

			// Deactivate the bullet if it goes out of screen
			if (_Position.X + _Texture.Width / 2 > _viewport.Width)
			{
				_active = false;
			}
		}

        #endregion

        #region Draw

        public void Draw(SpriteBatch spriteBatch)
		{
			spriteBatch.Draw(_Texture, _Position, null, Color.White, 0f, new Vector2(_Width / 2, _Height / 2), 1f, SpriteEffects.None, 0f);
		}

        #endregion
    }
}