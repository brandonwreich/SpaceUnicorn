using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceUnicorn.View;

namespace SpaceUnicorn.Model
{
	public class Meteors
	{
        #region Variables

        // Meteor animation
        private Animation _meteorAnimation;
		public Animation _MeteorAnimation
		{
			get { return _meteorAnimation; }
			set { _meteorAnimation = value; }
		}

		// Meteor position
		public Vector2 _Position;

		// Meteor is on screen
		private bool _active;
		public bool _Active
		{
			get { return _active; }
			set { _active = value; }
		}

		// Meteor health
		private int _health;
		public int _Health
		{
			get { return _health; }
			set { _health = value; }
		}

		// Meteor damage
		private int _damage;
		public int _Damage
		{
			get { return _damage; }
			set { _damage = value; }
		}

		// Width of animation
		public int _Width
		{
			get { return _MeteorAnimation._FrameWidth; }
		}

		// Height of animation
		public int _Height
		{
			get { return _MeteorAnimation._FrameHeight; }
		}

		// Meteor speed
		private float _meteorMoveSpeed;
		public float _MeteorMoveSpeed
		{
			get { return _meteorMoveSpeed; }
			set { _meteorMoveSpeed = value; }
		}

		// Random number generator
		private Random _random;

        #endregion

        public Meteors()
		{
			
		}

        #region Initialize

        public void Initialize(Animation animation, Vector2 position)
		{
			_meteorAnimation = animation;
			_Position = position;
			_active = true;
			_health = 20;
			_damage = 69;
			_meteorMoveSpeed = 5f;
			_random = new Random();
		}
        #endregion

        #region Update

        public void Update(GameTime gameTime)
		{
			_Position.X -= _meteorMoveSpeed;

			_meteorAnimation._Position = _Position;
			_meteorAnimation.Update(gameTime);

			if (_Position.X < -_Width || _Health <= 0)
			{
				_Active = false;
				_health = 0;
			}
		}

        #endregion

        #region Draw

        public void Draw(SpriteBatch spriteBatch)
		{
			_meteorAnimation.Draw(spriteBatch);
		}

        #endregion
    }
}