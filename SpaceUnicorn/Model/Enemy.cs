using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceUnicorn.View;

namespace SpaceUnicorn.Model
{
	public class Enemy
	{
        #region Variables

        // Animation representing the enemy
        private Animation _enemyAnimation;
		public Animation EnemyAnimation
		{
			get { return _enemyAnimation; }
			set { _enemyAnimation = value; }
		}

		// The position of the enemy ship relative to the top left corner of the screen
		public Vector2 Position;

		// The state of the Enemy Ships
		private bool _active;
		public bool Active
		{
			get { return _active; }
			set { _active = value; }
		}

		// Health of the enemy
		private int _health;
		public int Health
		{
			get { return _health; }
			set { _health = value; }
		}

		// The amount of damage the enemy inflicts on the player
		private int _damage;
		public int Damage
		{
			get { return _damage; }
			set { _damage = value; }
		}

		// The amount of score the enemy will give to the player
		private int _scoreValue;
		public int ScoreValue
		{
			get { return _scoreValue; }
			set { _scoreValue = value; }
		}

		// Get the width of the enemy ship
		public int Width
		{
			get { return EnemyAnimation.FrameWidth; }
		}

		// Get the height of the enemy ship
		public int Height
		{
			get { return EnemyAnimation.FrameHeight; }
		}

		// The speed at which the enemy moves
		private float _enemyMoveSpeed;
		public float EnemyMoveSpeed
		{
			get { return _enemyMoveSpeed; }
			set { _enemyMoveSpeed = value; }
		}

	    public Boolean Reverse { get; set; }

        public Boolean Lift { get; set; }

        #endregion

        #region Initailize

        public void Initialize(Animation animation, Vector2 position)
		{
			_enemyAnimation = animation;
			Position = position;
			_active = true;
			_health = 10;
			_damage = 10;
			_enemyMoveSpeed = 6f;
			_scoreValue = 100;
		    Reverse = false;
            Lift = false;
		}

        #endregion

        #region Update

        public void Update(GameTime gameTime)
        {
		    _enemyAnimation.Position = Position;

            _enemyAnimation.Update(gameTime);

            if (Reverse == false)
            {
                Position.X -= _enemyMoveSpeed;
            }

            if (Reverse)
            {
                Position.X += _enemyMoveSpeed;
            }

            if (Lift)
            {
                Position.Y += _enemyMoveSpeed;
            }
            
            if (Lift == false)
            {
                Position.Y -= EnemyMoveSpeed;
            }

            if (Health <= 0)
			{
				Active = false;
				_health = 0;
			}
		}

        #endregion

        #region Draw

        public void Draw(SpriteBatch spriteBatch)
		{
			_enemyAnimation.Draw(spriteBatch);
		}

        #endregion
    }
}