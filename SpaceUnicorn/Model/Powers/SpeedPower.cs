using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceUnicorn.View;

namespace SpaceUnicorn.Model.Powers {
	public class SpeedPower {
        #region Variables

        // Animation representing the power
        private Animation _speedAnimation;
		public Animation SpeedAnimation {
			get { return _speedAnimation; }
			set { _speedAnimation = value; }
		}

		// Position of the power relative to the upper left side of the screen
		public Vector2 Position;

		// State of the power
		private bool _active;
		public bool Active {
			get { return _active; }
			set { _active = value; }
		}

		// Get the width of the power
		public int Width {
			get { return SpeedAnimation.FrameWidth; }
		}

		// Get the height of the power
		public int Height {
			get { return SpeedAnimation.FrameHeight; }
		}

		// Determines how fast the power moves
		private float _speedPowerMoveSpeed;

        #endregion

        #region Initialize

        public void Initialize(Animation animation, Vector2 position) {
			_speedAnimation = animation;
			Position = position;
			_active = true;
			_speedPowerMoveSpeed = 6f;
		}

        #endregion

        #region Update

        public void Update(GameTime gameTime) {
			Position.X -= _speedPowerMoveSpeed;
			_speedAnimation.Position = Position;
			_speedAnimation.Update(gameTime);

			if (Position.X < -Width) {
				Active = false;
			}
		}

        #endregion

        #region Draw

        public void Draw(SpriteBatch spriteBatch) {
			_speedAnimation.Draw(spriteBatch);
		}

        #endregion
    }
}