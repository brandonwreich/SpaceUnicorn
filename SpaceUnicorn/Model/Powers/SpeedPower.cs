using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceUnicorn.View;

namespace SpaceUnicorn.Model.Powers
{
	public class SpeedPower
	{
        #region Variables

        // Animation representing the power
        public Animation _speedAnimation;
		public Animation _SpeedAnimation
		{
			get { return _speedAnimation; }
			set { _speedAnimation = value; }
		}

		// Position of the power relative to the upper left side of the screen
		public Vector2 _Position;

		// State of the power
		private bool _active;
		public bool _Active
		{
			get { return _active; }
			set { _active = value; }
		}

		// Represents the viewable boundary of the game
		private Viewport _viewport;

		// Get the width of the power
		public int _Width
		{
			get { return _SpeedAnimation._FrameWidth; }
		}

		// Get the height of the power
		public int _Height
		{
			get { return _SpeedAnimation._FrameHeight; }
		}

		// Determines how fast the power moves
		private float _speedPowerMoveSpeed;

        #endregion

        public SpeedPower()
		{

		}

        #region Initialize

        public void Initialize(Animation animation, Vector2 position)
		{
			_speedAnimation = animation;

			_Position = position;

			_active = true;

			_speedPowerMoveSpeed = 6f;
		}

        #endregion

        #region Update

        public void Update(GameTime gameTime)
		{
			_Position.X -= _speedPowerMoveSpeed;

			_speedAnimation._Position = _Position;

			_speedAnimation.Update(gameTime);

			if (_Position.X < -_Width)
			{
				_Active = false;
			}
		}

        #endregion

        #region Draw

        public void Draw(SpriteBatch spriteBatch)
		{
			_speedAnimation.Draw(spriteBatch);
		}

        #endregion
    }
}
