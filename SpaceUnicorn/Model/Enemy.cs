using System;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OpenGL;
using SpaceUnicorn.View;

namespace SpaceUnicorn.Model
{
	public class Enemy
	{
        #region Variables

        // Animation representing the enemy
        public Animation _enemyAnimation;
		public Animation _EnemyAnimation
		{
			get { return _enemyAnimation; }
			set { _enemyAnimation = value; }
		}

		// The position of the enemy ship relative to the top left corner of the screen
		public Vector2 _Position;

		// The state of the Enemy Ships
		private bool _active;
		public bool _Active
		{
			get { return _active; }
			set { _active = value; }
		}

		// Health of the enemy
		private int _health;
		public int _Health
		{
			get { return _health; }
			set { _health = value; }
		}

		// The amount of damage the enemy inflicts on the player
		private int _damage;
		public int _Damage
		{
			get { return _damage; }
			set { _damage = value; }
		}

		// The amount of score the enemy will give to the player
		private int _scoreValue;
		public int _ScoreValue
		{
			get { return _scoreValue; }
			set { _scoreValue = value; }
		}

		// Get the width of the enemy ship
		public int _Width
		{
			get { return _EnemyAnimation._FrameWidth; }
		}

		// Get the height of the enemy ship
		public int _Height
		{
			get { return _EnemyAnimation._FrameHeight; }
		}

		// The speed at which the enemy moves
		private float _enemyMoveSpeed;
		public float _EnemyMoveSpeed
		{
			get { return _enemyMoveSpeed; }
			set { _enemyMoveSpeed = value; }
		}

	    public Boolean _reverse { get; set; }

        #endregion

        public Enemy()
		{

		}

        #region Initailize

        public void Initialize(Animation animation, Vector2 position)
		{
			_enemyAnimation = animation;
			_Position = position;
			_active = true;
			_health = 10;
			_damage = 10;
			_enemyMoveSpeed = 6f;
			_scoreValue = 100;
		    _reverse = false;
		}

        #endregion

        #region Update

        public void Update(GameTime gameTime)
        {
		    _enemyAnimation._Position = _Position;

            _enemyAnimation.Update(gameTime);

            if (_reverse == false)
            {
                _Position.X -= _enemyMoveSpeed;
            }

            if (_reverse)
            {
                _Position.X += _enemyMoveSpeed;
               Console.WriteLine(_Position.X);
            }

            if (_Health <= 0)
			{
				_Active = false;
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