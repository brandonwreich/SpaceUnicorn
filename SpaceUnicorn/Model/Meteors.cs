using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceUnicorn.View;

namespace SpaceUnicorn.Model
{
	public class Meteors
	{
		/* Initalize data members */

		// Meteor animation
		public Animation meteorAnimation;
		public Animation MeteorAnimation
		{
			get { return meteorAnimation; }
			set { meteorAnimation = value; }
		}

		// Meteor position
		public Vector2 Position;

		// Meteor is on screen
		private bool active;
		public bool Active
		{
			get { return active; }
			set { active = value; }
		}

		// Meteor health
		private int health;
		public int Health
		{
			get { return health; }
			set { health = value; }
		}

		// Meteor damage
		private int damage;
		public int Damage
		{
			get { return damage; }
			set { damage = value; }
		}

		// Width of animation
		public int Width
		{
			get { return MeteorAnimation.FrameWidth; }
		}

		// Height of animation
		public int Height
		{
			get { return MeteorAnimation.FrameHeight; }
		}

		// Meteor speed
		private float meteorMoveSpeed;
		public float MeteorMoveSpeed
		{
			get { return meteorMoveSpeed; }
			set { meteorMoveSpeed = value; }
		}

		// Random number generator
		private Random random;

		public Meteors()
		{
			
		}

		public void Initialize(Animation animation, Vector2 position)
		{
			meteorAnimation = animation;
			Position = position;
			active = true;
			health = 20;
			damage = 69;
			meteorMoveSpeed = 5f;
			random = new Random();
		}

		public void Update(GameTime gameTime)
		{
			Position.X -= meteorMoveSpeed;

			meteorAnimation.Position = Position;
			meteorAnimation.Update(gameTime);

			if (Position.X < -Width || Health <= 0)
			{
				Active = false;
				health = 0;
			}
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			meteorAnimation.Draw(spriteBatch);
		}
	}
}