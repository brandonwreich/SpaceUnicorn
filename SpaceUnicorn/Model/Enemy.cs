using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceUnicorn.View;

namespace SpaceUnicorn.Model
{
	public class Enemy
	{
		// Animation representing the enemy
		public Animation enemyAnimation;
		public Animation EnemyAnimation
		{
			get { return enemyAnimation; }
			set { enemyAnimation = value; }
		}

		// The position of the enemy ship relative to the top left corner of the screen
		public Vector2 Position;

		// The state of the Enemy Ships
		private bool active;
		public bool Active
		{
			get { return active; }
			set { active = value; }
		}

		// Health of the enemy
		private int health;
		public int Health
		{
			get { return health; }
			set { health = value; }
		}

		// The amount of damage the enemy inflicts on the player
		private int damage;
		public int Damage
		{
			get { return damage; }
			set { damage = value; }
		}

		// The amount of score the enemy will give to the player
		private int scoreValue;
		public int ScoreValue
		{
			get { return scoreValue; }
			set { scoreValue = value; }
		}

		// Get the width of the enemy ship
		public int Width
		{
			get { return EnemyAnimation.FrameWidth; }
		}

		// Get the height of the enemy ship
		public int Height
		{
			get { return EnemyAnimation.FrameHeight; }
		}

		// The speed at which the enemy moves
		private float enemyMoveSpeed;
		public float EnemyMoveSpeed
		{
			get { return enemyMoveSpeed; }
			set { enemyMoveSpeed = value; }
		}

		public Enemy()
		{

		}

		public void Initialize(Animation animation, Vector2 position)
		{
			enemyAnimation = animation;
			Position = position;
			active = true;
			health = 10;
			damage = 10;
			enemyMoveSpeed = 6f;
			scoreValue = 100;
		}

		public void Update(GameTime gameTime)
		{
			Position.X -= enemyMoveSpeed;

			enemyAnimation.Position = Position;

			enemyAnimation.Update(gameTime);

			if (Position.X < -Width || Health <= 0)
			{
				Active = false;
				health = 0;
			}
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			enemyAnimation.Draw(spriteBatch);
		}
	}
}