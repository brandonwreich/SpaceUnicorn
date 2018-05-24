using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceUnicorn.View;

namespace SpaceUnicorn
{
	public class SpeedPower
	{
		// Animation representing the enemy
		public Animation speedAnimation;
		public Animation SpeedAnimation
		{
			get { return speedAnimation; }
			set { speedAnimation = value; }
		}

		// Position of the powerup relative to the upper left side of the screen
		public Vector2 Position;

		// State of the powerup
		private bool active;
		public bool Active
		{
			get { return active; }
			set { active = value; }
		}

		// Health of power up
		private int health;
		public int Health
		{
			get { return health; }
			set { health = value; }
		}

		// Represents the viewable boundary of the game
		Viewport viewport;

		// Get the width of the powerup
		public int Width
		{
			get { return SpeedAnimation.FrameWidth; }
		}

		// Get the height of the powerup
		public int Height
		{
			get { return SpeedAnimation.FrameHeight; }
		}

		// Determines how fast the powerup moves
		private float speedPowerMoveSpeed;

		public SpeedPower()
		{

		}

		public void Initialize(Animation animation, Vector2 position)
		{
			speedAnimation = animation;

			Position = position;

			active = true;

			health = 10;

			speedPowerMoveSpeed = 6f;
		}

		public void Update(GameTime gameTime)
		{
			Position.X -= speedPowerMoveSpeed;

			speedAnimation.Position = Position;

			speedAnimation.Update(gameTime);

			if (Position.X < -Width || Health <= 0)
			{
				Active = false;
				health = 0;
			}
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			speedAnimation.Draw(spriteBatch);
		}
	}
}
