using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using SpaceUnicorn.View;

namespace SpaceUnicorn.Model
{
	public class Player
	{
        #region Variables

        // Animation representing the player
        private Animation _playerAnimation;
		public Animation PlayerAnimation
		{
			get { return _playerAnimation; }
			set { _playerAnimation = value; }
		}

		// Position of player
		public Vector2 Position;

		// State of player
		private bool _active;
		public bool Active
		{
			get { return _active; }
			set { _active = value; }
		}

		// Player health
		private int _health;
		public int Health
		{
			get { return _health; }
			set { _health = value; }
		}

		// Width of player
		public int Width
		{
			get { return PlayerAnimation.FrameWidth; }
		}

		// Height of player
		public int Height
		{
			get { return PlayerAnimation.FrameHeight; }
		}

        #endregion

        public Player()
		{

		}

        #region Initialize

        public void Initialize(Animation animation, Vector2 position)
		{
			_playerAnimation = animation;
			Position = position;
			Active = true;
			Health = 100;
		}

        #endregion

        #region Update

        public void Update(GameTime gameTime)
		{
			_playerAnimation.Position = Position;
			_playerAnimation.Update(gameTime);
		}

        #endregion

        #region Draw

        public void Draw(SpriteBatch spriteBatch)
		{
			_playerAnimation.Draw(spriteBatch);
		}

        #endregion
    }
}