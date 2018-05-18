using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceUnicorn
{
	public class HyperSpace
	{
		// Image representing the powerup
		private Texture2D texture;
		public Texture2D Texture
		{
			get { return texture; }
			set { texture = value; }
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
		private Viewport viewport;

		// Get the width of the powerup
		public int Width
		{
			get { return Texture.Width; }
		}

		// Get the height of the powerup
		public int Height
		{
			get { return Texture.Height; }
		}

		// Determines how fast the powerup moves
		private float hyperSpaceMoveSpeed;

		public HyperSpace()
		{

		}

		public void Initialize(Viewport viewport, Texture2D texture, Vector2 position)
		{
			this.texture = texture;
			this.Position = position;
			this.viewport = viewport;
			this.Health = 1;

			active = true;

			hyperSpaceMoveSpeed = 6f;
		}

		public void Update(GameTime gameTime)
		{
			Position.X -= hyperSpaceMoveSpeed;

			if (Position.X < -Width || Health <= 0)
			{
				Active = false;
			}
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			spriteBatch.Draw(Texture, Position, null, Color.White, 0f, new Vector2(Width / 2, Height / 2), 1f, SpriteEffects.None, 0f);
		}
	}
}
