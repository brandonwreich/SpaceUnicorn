using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceUnicorn.View;

namespace SpaceUnicorn.Model
{
	public class Meteors
	{
		public Animation meteorAnimation;
		public Animation MeteorAnimation
		{
			get { return meteorAnimation; }
			set { meteorAnimation = value; }
		}

		public Vector2 Position;

		private bool active;
		public bool Active
		{
			get { return active; }
			set { active = value; }
		}

		private int health;
		public int Health
		{
			get { return health; }
			set { health = value; }
		}

		private int damage;
		public int Damage
		{
			get { return damage; }
			set { damage = value; }
		}

		public int Width
		{
			get { return MeteorAnimation.FrameWidth; }
		}

		public int Height
		{
			get { return MeteorAnimation.FrameHeight; }
		}

		private float meteorMoveSpeed;
		public float MeteorMoveSpeed
		{
			get { return MeteorMoveSpeed; }
			set { MeteorMoveSpeed = value; }
		}

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

			damage = random.Next(1, 100);

			meteorMoveSpeed = 5f;

			random = new Random();
		}

		public void Update(GameTime gameTime)
		{
			Position.X -= meteorMoveSpeed;
			Position.Y += meteorMoveSpeed;

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
