using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using SpaceUnicorn.View;

namespace SpaceUnicorn.Model
{
	public class Player
	{
		// Animation representing the player
		private Animation playerAnimation;
		public Animation PlayerAnimation
		{
			get { return playerAnimation; }
			set { playerAnimation = value; }
		}

		// Position of player
		public Vector2 Position;

		// State of player
		private bool active;
		public bool Active
		{
			get { return active; }
			set { active = value; }
		}

		//Player health
		private int health;
		public int Health
		{
			get { return health; }
			set { health = value; }
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

		public Player()
		{

		}

		public void Initialize(Animation animation, Vector2 position)
		{
			playerAnimation = animation;

			Position = position;

			Active = true;

			Health = 100;
		}

		public void Update(GameTime gameTime)
		{
			playerAnimation.Position = Position;
			playerAnimation.Update(gameTime);
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			playerAnimation.Draw(spriteBatch);
		}
	}
}