﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceUnicorn.Model
{
	public class MarshmallowLaser
	{
		// Image representing the Marhmallow
		private Texture2D texture;
		public Texture2D Texture
		{
			get { return texture; }
			set { texture = value; }
		}
		// Position of the marshmallow relative to the upper left side of the screen
		public Vector2 Position;

		// State of the marshmallow
		private bool active;
		public bool Active
		{
			get { return active; }
			set { active = value; }
		}

		// The amount of damage the marshmallow can inflict to an enemy
		public int damage;
		public int Damage
		{
			get { return damage; }
			set { damage = value; }
		}

		// Represents the viewable boundary of the game
		private Viewport viewport;


		// Get the width of the unicorn
		public int Width
		{
			get { return Texture.Width; }
		}

		// Get the height of the unicorn
		public int Height
		{
			get { return Texture.Height; }
		}

		// Determines how fast the marshmallow moves
		private float marshmallowMoveSpeed;

		public MarshmallowLaser()
		{

		}

		public void Initialize(Viewport viewport, Texture2D texture, Vector2 position)
		{
			this.texture = texture;
			this.Position = position;
			this.viewport = viewport;

			active = true;

			damage = 7;

			marshmallowMoveSpeed = 20f;
		}

		public void Update()
		{
			// Projectiles always move to the right
			Position.X += marshmallowMoveSpeed;

			// Deactivate the bullet if it goes out of screen
			if (Position.X + Texture.Width / 2 > viewport.Width)
			{
				active = false;
			}
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			spriteBatch.Draw(Texture, Position, null, Color.White, 0f, new Vector2(Width / 2, Height / 2), 1f, SpriteEffects.None, 0f);
		}
	}
}