using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceUnicorn.View;

namespace SpaceUnicorn
{
	public class SpeedPower
	{
        #region Variables

        // Animation representing the power
        public Animation speedAnimation;
		public Animation SpeedAnimation
		{
			get { return speedAnimation; }
			set { speedAnimation = value; }
		}

		// Position of the power relative to the upper left side of the screen
		public Vector2 Position;

		// State of the power
		private bool active;
		public bool Active
		{
			get { return active; }
			set { active = value; }
		}

		// Represents the viewable boundary of the game
		private Viewport viewport;

		// Get the width of the power
		public int Width
		{
			get { return SpeedAnimation.FrameWidth; }
		}

		// Get the height of the power
		public int Height
		{
			get { return SpeedAnimation.FrameHeight; }
		}

		// Determines how fast the power moves
		private float speedPowerMoveSpeed;

        #endregion

        public SpeedPower()
		{

		}

        #region Initialize

        public void Initialize(Animation animation, Vector2 position)
		{
			speedAnimation = animation;

			Position = position;

			active = true;

			speedPowerMoveSpeed = 6f;
		}

        #endregion

        #region Update

        public void Update(GameTime gameTime)
		{
			Position.X -= speedPowerMoveSpeed;

			speedAnimation.Position = Position;

			speedAnimation.Update(gameTime);

			if (Position.X < -Width)
			{
				Active = false;
			}
		}

        #endregion

        #region Draw

        public void Draw(SpriteBatch spriteBatch)
		{
			speedAnimation.Draw(spriteBatch);
		}

        #endregion
    }
}
