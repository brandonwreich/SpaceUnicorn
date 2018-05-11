using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;

using SpaceUnicorn.Model;
using SpaceUnicorn.View;

namespace SpaceUnicorn
{
	/// <summary>
	/// This is the main type for your game.
	/// </summary>
	public class SpaceUnicorn : Game
	{
		GraphicsDeviceManager graphics;
		SpriteBatch spriteBatch;

		// Init player
		private Player player;

		// Init enemy
		private Texture2D enemyTexture;
		private List<Enemies> enemies;

		// Enemy spawn rate
		private TimeSpan enemySpawnTime;
		private TimeSpan previousEnemySpawnTime;

		// Random number generator
		private Random random;

		// Keyboard states
		private KeyboardState currentKeyboardState;
		private KeyboardState previousKeyboardState;

		// Gamepad states
		private GamePadState currentGamePadState;
		private GamePadState previousGamePadState;

		// Speed of player
		private float playerMoveSpeed;

		// Parallaxing Layer
		private ParallaxingBackground bgLayer1;

		private TimeSpan powerTime;
		private TimeSpan endPowerTime;

		// Background Song
		private Song gameMusic;

		// Laser
		private Texture2D marshmallowPic;
		private List<MarshmallowLaser> marshmallows;
		private TimeSpan fireTime;
		private TimeSpan previousFireTime;

		// Expolsions
		private Texture2D explosionTexture;
		private List<Animation> explosions;

		/* Power Ups/Downs */

		// Health power up
		private Texture2D healthPowerPic;
		private List<HealthPowerUp> healthy;
		private TimeSpan healthSpawnTime;
		private TimeSpan previousHealthSpawnTime;

		// Enemy Spawn Rate power down
		private Texture2D enemySpawnPowerDown;
		private List<EnemySpawnRatePowerDown> invasion ;
		private TimeSpan invasionSpawnTime;
		private TimeSpan previousInvasionSpawnTime;

		public SpaceUnicorn()
		{
			graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";
		}

		/// <summary>
		/// Allows the game to perform any initialization it needs to before starting to run.
		/// This is where it can query for any required services and load any non-graphic
		/// related content.  Calling base.Initialize will enumerate through any components
		/// and initialize them as well.
		/// </summary>
		protected override void Initialize()
		{
			// Init player class
			player = new Player();

			// Constant player speed
			playerMoveSpeed = 8.0f;

			// Init enemy class
			enemies = new List<Enemies>();

			// Enemy spawn time
			previousEnemySpawnTime = TimeSpan.Zero;
			enemySpawnTime = TimeSpan.FromSeconds(1.0f);

			// Random number generator
			random = new Random();

			// Init background
			bgLayer1 = new ParallaxingBackground();

			// Init marshmallows
			marshmallows = new List<MarshmallowLaser>();

			// Fire time
			fireTime = TimeSpan.FromSeconds(.15f);

			// Explosions
			explosions = new List<Animation>();

			powerTime = TimeSpan.FromSeconds(10);
			endPowerTime = TimeSpan.Zero;

			// Health power up
			healthy = new List<HealthPowerUp>();
			previousHealthSpawnTime = TimeSpan.Zero;
			healthSpawnTime = TimeSpan.FromMinutes(random.Next(1, 5));

			// Enemy spawn power down
			invasion = new List<EnemySpawnRatePowerDown>();
			previousInvasionSpawnTime = TimeSpan.Zero;
			invasionSpawnTime = TimeSpan.FromMinutes(random.Next(1, 2));

			base.Initialize();
		}

		/// <summary>
		/// LoadContent will be called once per game and is the place to load
		/// all of your content.
		/// </summary>
		protected override void LoadContent()
		{
			// Create a new SpriteBatch, which can be used to draw textures.
			spriteBatch = new SpriteBatch(GraphicsDevice);

			// Load player
			Animation playerAnimation = new Animation();
			Texture2D playerTexture = Content.Load<Texture2D>("Animation/Space Unicorn");
			playerAnimation.Initialize(playerTexture, Vector2.Zero, 166, 100, 1, 5, Color.White, 1f, true);

			Vector2 playerPosition = new Vector2(GraphicsDevice.Viewport.Width / 4, GraphicsDevice.Viewport.Height / 2);
			player.Initialize(playerAnimation, playerPosition);

			// Load enemy
			enemyTexture = Content.Load<Texture2D>("Animation/enemy");

			// Load background
			bgLayer1.Initialize(Content, "Background/spaceBackground", GraphicsDevice.Viewport.Width, -1);

			// Load marshmallows
			marshmallowPic = Content.Load<Texture2D>("Animation/marshmallowLaser");

			// Load music
			gameMusic = Content.Load<Song>("Music/Space Unicorn");

			// Load explosion
			explosionTexture = Content.Load<Texture2D>("Animation/explosion");

			// Load health powerup
			healthPowerPic = Content.Load<Texture2D>("Powers/healthPowerUp");

			// Load enemy spawn power down
			enemySpawnPowerDown = Content.Load<Texture2D>("Powers/enemySpawnRateIncrease");

			// Play music
			PlayMusic(gameMusic);
		}

		/// <summary>
		/// Allows the game to run logic such as updating the world,
		/// checking for collisions, gathering input, and playing audio.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Update(GameTime gameTime)
		{
			// For Mobile devices, this logic will close the Game when the Back button is pressed
			// Exit() is obsolete on iOS
#if !__IOS__ && !__TVOS__
			if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
				Exit();
#endif
			// Save the previous state of the keyboard and game pad so we can determinesingle key/button presses
			previousGamePadState = currentGamePadState;
			previousKeyboardState = currentKeyboardState;

			// Read the current state of the keyboard and gamepad and store it
			currentKeyboardState = Keyboard.GetState();
			currentGamePadState = GamePad.GetState(PlayerIndex.One);

			// Update player
			UpdatePlayer(gameTime);

			// Update enemies
			UpdateEnemies(gameTime);

			// Update collisions
			UpdateCollisions(gameTime);

			// Update background
			bgLayer1.Update();

			// Update marshmallows
			UpdateMarshallows();

			// Update the explosions
			UpdateExplosions(gameTime);

			// Update Health powerup
			UpdateHealthPowerUp(gameTime);

			// Update Enemy spawn power down
			UpdateEnemySpawnPowerDown(gameTime);

			base.Update(gameTime);
		}

		/// <summary>
		/// This is called when the game should draw itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw(GameTime gameTime)
		{
			graphics.GraphicsDevice.Clear(Color.CornflowerBlue);

			//Start drawing
			spriteBatch.Begin();

			// Draw background
			bgLayer1.Draw(spriteBatch);

			//Draw the player
			player.Draw(spriteBatch);

			// Draw enemies
			for (int i = 0; i < enemies.Count; i++)
			{
				enemies[i].Draw(spriteBatch);
			}

			// Draw mashmallows
			for (int i = 0; i < marshmallows.Count; i++)
			{
				marshmallows[i].Draw(spriteBatch);
			}

			// Draw the explosions
			for (int i = 0; i < explosions.Count; i++)
			{
				explosions[i].Draw(spriteBatch);
			}

			// Draw health powerups
			for (int i = 0; i < healthy.Count; i++)
			{
				healthy[i].Draw(spriteBatch);
			}

			// Draw enemy spawn power downs
			for (int i = 0; i < invasion.Count; i++)
			{
				invasion[i].Draw(spriteBatch);
			}

			//Stop drawing
			spriteBatch.End();

			base.Draw(gameTime);
		}

		public void UpdatePlayer(GameTime gameTime)
		{
			player.Update(gameTime);

			// Get Thumbstick Controls
			player.Position.X += currentGamePadState.ThumbSticks.Left.X * playerMoveSpeed;
			player.Position.Y -= currentGamePadState.ThumbSticks.Left.Y * playerMoveSpeed;

			// Use the Keyboard / Dpad
			if (currentKeyboardState.IsKeyDown(Keys.Left) || currentGamePadState.DPad.Left == ButtonState.Pressed)
			{
				player.Position.X -= playerMoveSpeed;
			}
			if (currentKeyboardState.IsKeyDown(Keys.Right) || currentGamePadState.DPad.Right == ButtonState.Pressed)
			{
				player.Position.X += playerMoveSpeed;
			}
			if (currentKeyboardState.IsKeyDown(Keys.Up) || currentGamePadState.DPad.Up == ButtonState.Pressed)
			{
				player.Position.Y -= playerMoveSpeed;
			}
			if (currentKeyboardState.IsKeyDown(Keys.Down) || currentGamePadState.DPad.Down == ButtonState.Pressed)
			{
				player.Position.Y += playerMoveSpeed;
			}
			if (currentKeyboardState.IsKeyDown(Keys.Space))
			{
				if (gameTime.TotalGameTime - previousFireTime > fireTime)
				{
					previousFireTime = gameTime.TotalGameTime;

					AddMarshmallow(player.Position - new Vector2(-1 * (player.Width / 2), player.Height / 3));
				}
			}

			// Make sure that the player does not go out of bounds
			player.Position.X = MathHelper.Clamp(player.Position.X, 10, 790);
			player.Position.Y = MathHelper.Clamp(player.Position.Y, 10, 470);
		}

		private void PlayMusic(Song song)
		{
			try
			{
				// Play the song
				MediaPlayer.Play(song);

				// Repeat the song
				MediaPlayer.IsRepeating = true;
			}
			catch { }
		}

		private void AddEnemy()
		{
			Animation enemyAnimation = new Animation();

			enemyAnimation.Initialize(enemyTexture, Vector2.Zero, 80, 40, 1, 5, Color.White, 1f, true);

			Vector2 position = new Vector2(GraphicsDevice.Viewport.Width + enemyTexture.Width / 2, random.Next(50, GraphicsDevice.Viewport.Height - 50));

			Enemies enemy = new Enemies();

			enemy.Initialize(enemyAnimation, position);

			enemies.Add(enemy);
		}

		private void UpdateEnemies(GameTime gameTime)
		{
			if (gameTime.TotalGameTime - previousEnemySpawnTime > enemySpawnTime)
			{
				previousEnemySpawnTime = gameTime.TotalGameTime;

				AddEnemy();
			}

			for (int i = enemies.Count - 1; i >= 0; i--)
			{
				enemies[i].Update(gameTime);

				if (enemies[i].Active == false)
				{
					// If not active and health <= 0
					if (enemies[i].Health <= 0)
					{
						// Add an explosion
						AddExplosion(enemies[i].Position);
					}

					enemies.RemoveAt(i);
				}
			}
		}

		private void AddMarshmallow(Vector2 position)
		{
			MarshmallowLaser marshmallow = new MarshmallowLaser();
			marshmallow.Initialize(GraphicsDevice.Viewport, marshmallowPic, position);
			marshmallows.Add(marshmallow);
		}

		private void UpdateMarshallows()
		{
			for (int i = marshmallows.Count - 1; i >= 0; i--)
			{
				marshmallows[i].Update();

				if (marshmallows[i].Active == false)
				{
					marshmallows.RemoveAt(i);
				}
			}
		}

		private void AddHealthPowerUp()
		{
			HealthPowerUp health = new HealthPowerUp();
			health.Initialize(GraphicsDevice.Viewport, healthPowerPic, new Vector2((GraphicsDevice.Viewport.Width + enemyTexture.Width / 2), random.Next(50, GraphicsDevice.Viewport.Height - 50)));
			healthy.Add(health);
		}

		private void UpdateHealthPowerUp(GameTime gameTime)
		{
			if (gameTime.TotalGameTime - previousHealthSpawnTime > healthSpawnTime)
			{
				previousHealthSpawnTime = gameTime.TotalGameTime;

				AddHealthPowerUp();
			}

			for (int i = healthy.Count - 1; i >= 0; i--)
			{
				healthy[i].Update(gameTime);

				if (healthy[i].Active == false)
				{
					healthy.RemoveAt(i);
				}
			}
		}

		private void AddEnemySpawnPowerDown()
		{
			EnemySpawnRatePowerDown spawn = new EnemySpawnRatePowerDown();
			spawn.Initialize(GraphicsDevice.Viewport, enemySpawnPowerDown, new Vector2((GraphicsDevice.Viewport.Width + enemyTexture.Width / 2), random.Next(50, GraphicsDevice.Viewport.Height - 50)));
			invasion.Add(spawn);
		}

		private void UpdateEnemySpawnPowerDown(GameTime gameTime)
		{
			if (gameTime.TotalGameTime - previousInvasionSpawnTime > invasionSpawnTime)
			{
				previousInvasionSpawnTime = gameTime.TotalGameTime;

				AddEnemySpawnPowerDown();
			}

			for (int i = invasion.Count - 1; i >= 0; i--)
			{
				invasion[i].Update(gameTime);

				if (invasion[i].Active == false)
				{
					invasion.RemoveAt(i);
				}
			}
		}

		private void UpdateCollisions(GameTime gameTime)
		{
			// Use the Rectangle's built-in intersect function to 
			// determine if two objects are overlapping
			Rectangle rectangle1;
			Rectangle rectangle2;

			// Only create the rectangle once for the player
			rectangle1 = new Rectangle((int)player.Position.X, (int)player.Position.Y, player.Width - 100, player.Height);

			// Do the collision between the player and the enemies
			for (int i = 0; i < enemies.Count; i++)
			{
				rectangle2 = new Rectangle((int)enemies[i].Position.X, (int)enemies[i].Position.Y, enemies[i].Width, enemies[i].Height);

				// Determine if the two objects collided with each other
				if (rectangle1.Intersects(rectangle2))
				{
					// Subtract the health from the player based on
					// the enemy damage
					player.Health -= enemies[i].Damage;

					// Since the enemy collided with the player
					// destroy it
					enemies[i].Health = 0;

					// If the player health is less than zero we died
					if (player.Health <= 0)
					{
						player.Active = false;
					}
				}
			}

			// Player vs Enemy Spawn Rate power down collision
			for (int i = 0; i < invasion.Count; i++)
			{
				rectangle2 = new Rectangle((int)invasion[i].Position.X, (int)invasion[i].Position.Y, invasion[i].Width, invasion[i].Height);

				if (rectangle1.Intersects(rectangle2))
				{
					if (gameTime.TotalGameTime - endPowerTime > powerTime)
					{
						endPowerTime = gameTime.TotalGameTime;

						enemySpawnTime = TimeSpan.FromSeconds(.15f);

						fireTime = TimeSpan.FromSeconds(.00001f);
					}
						
					invasion[i].Health = 0;
				}
			}

			// Player vs Health power up Collision 
			for (int i = 0; i < healthy.Count; i++)
			{
				rectangle2 = new Rectangle((int)healthy[i].Position.X, (int)healthy[i].Position.Y, healthy[i].Width, healthy[i].Height);

				if (rectangle1.Intersects(rectangle2))
				{
					player.Health = 100;

					healthy[i].Health = 0;
				}
			}

			// Projectile vs Enemy Collision
			for (int i = 0; i < marshmallows.Count; i++)
			{
				for (int j = 0; j < enemies.Count; j++)
				{
					// Create the rectangles we need to determine if we collided with each other
					rectangle1 = new Rectangle((int)marshmallows[i].Position.X - marshmallows[i].Width / 2, (int)marshmallows[i].Position.Y -
											   marshmallows[i].Height / 2, marshmallows[i].Width, marshmallows[i].Height);

					rectangle2 = new Rectangle((int)enemies[j].Position.X - enemies[j].Width / 2, (int)enemies[j].Position.Y - enemies[j].Height / 2, enemies[j].Width, enemies[j].Height);

					// Determine if the two objects collided with each other
					if (rectangle1.Intersects(rectangle2))
					{
						enemies[j].Health -= marshmallows[i].Damage;
						marshmallows[i].Active = false;
					}
				}
			}
		}

		private void AddExplosion(Vector2 position)
		{
			Animation explosion = new Animation();
			explosion.Initialize(explosionTexture, position, 134, 134, 12, 45, Color.White, 1f, false);
			explosions.Add(explosion);
		}

		private void UpdateExplosions(GameTime gameTime)
		{
			for (int i = explosions.Count - 1; i >= 0; i--)
			{
				explosions[i].Update(gameTime);
				if (explosions[i].Active == false)
				{
					explosions.RemoveAt(i);
				}
			}
		}
	}
}