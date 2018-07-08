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
		public Animation _PlayerAnimation
		{
			get { return _playerAnimation; }
			set { _playerAnimation = value; }
		}

		// Position of player
		public Vector2 _Position;

		// State of player
		private bool _active;
		public bool _Active
		{
			get { return _active; }
			set { _active = value; }
		}

		// Player health
		private int _health;
		public int _Health
		{
			get { return _health; }
			set { _health = value; }
		}

		// Width of player
		public int _Width
		{
			get { return _PlayerAnimation._FrameWidth; }
		}

		// Height of player
		public int _Height
		{
			get { return _PlayerAnimation._FrameHeight; }
		}

        #endregion

        public Player()
		{

		}

        #region Initialize

        public void Initialize(Animation animation, Vector2 position)
		{
			_playerAnimation = animation;
			_Position = position;
			_Active = true;
			_Health = 100;
		}

        #endregion

        #region Update

        public void Update(GameTime gameTime)
		{
			_playerAnimation._Position = _Position;
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