using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Media;
using System.Timers;

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
		private List<Enemy> enemies;

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

		// Fonts
		private int score;
		private SpriteFont font;

		/* Power Ups/Downs */

		private static Timer powerTimer;
		private int countSeconds;

		// Health power up
		private Texture2D healthBoostIcon;
		private List<HealthBoost> healthy;
		private TimeSpan healthSpawnTime;
		private TimeSpan previousHealthSpawnTime;

		//  Speed power
		private Texture2D speedIcon;
		private List<SpeedPower> speed;
		private TimeSpan speedSpawnTime;
		private TimeSpan previousSpeedSpawnTime;

		// Enemy Spawn Rate power down
		private bool isInvading;

		// Slow motion power up
		private bool isSlowingDown;

		// Hyper space
		private Texture2D hyperSpaceIcon;
		private List<HyperSpace> hyperSpace;
		private TimeSpan hyperSpaceSpawnTime;
		private TimeSpan previousHyperSpaceSpawnTime;
		private bool isJumping;

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
			enemies = new List<Enemy>();

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

			// Score
			score = 0;

			/* Power ups/downs */

			powerTimer = new Timer();
			powerTimer.Interval = 1;
			powerTimer.Elapsed += OnTimedEvent;
			powerTimer.Enabled = false;
			powerTimer.AutoReset = true;
			countSeconds = 10000;

			// Time power
			powerTime = TimeSpan.FromSeconds(10f);
			endPowerTime = TimeSpan.Zero;

			// Health power up
			healthy = new List<HealthBoost>();
			previousHealthSpawnTime = TimeSpan.Zero;
			healthSpawnTime = TimeSpan.FromMinutes(random.Next(1, 5));

			// Speed Power
			speed = new List<SpeedPower>();
			previousSpeedSpawnTime = TimeSpan.Zero;
			speedSpawnTime = TimeSpan.FromMinutes(random.Next(1, 3));

			// Enemy spawn power down
			isInvading = false;

			// Slow motion
			isSlowingDown = false;

			// Hyper space
			hyperSpace = new List<HyperSpace>();
			previousHyperSpaceSpawnTime = TimeSpan.Zero;
			hyperSpaceSpawnTime = TimeSpan.FromMinutes(random.Next(1, 1));
			isJumping = false;

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

			// Load font
			font = Content.Load<SpriteFont>("Fonts/gameFont");

			// Powers
			healthBoostIcon = Content.Load<Texture2D>("Powers/healthPowerUp");
			speedIcon = Content.Load<Texture2D>("Powers/speedIncrease");
			hyperSpaceIcon = Content.Load<Texture2D>("Powers/hyperSpace");

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
			UpdateCollisions();

			// Update background
			bgLayer1.Update();

			// Update marshmallows
			UpdateMarshallows();

			// Update the explosions
			UpdateExplosions(gameTime);

			// Update powers
			UpdateHealthBoost(gameTime);
			UpdateSpeed(gameTime);
			UpdateHyperSpace(gameTime);

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

			// Draw health boost
			for (int i = 0; i < healthy.Count; i++)
			{
				healthy[i].Draw(spriteBatch);
			}

			for (int i = 0; i < speed.Count; i++)
			{
				speed[i].Draw(spriteBatch);
			}

			// Draw hyper space power
			for (int i = 0; i < hyperSpace.Count; i++)
			{
				hyperSpace[i].Draw(spriteBatch);
			}

			// Draw the score
			spriteBatch.DrawString(font, "Score: " + score, new Vector2(GraphicsDevice.Viewport.TitleSafeArea.X + 2, GraphicsDevice.Viewport.TitleSafeArea.Y), Color.Yellow);
			// Draw the player health
			spriteBatch.DrawString(font, "Health: " + player.Health, new Vector2(GraphicsDevice.Viewport.TitleSafeArea.Width - 145, GraphicsDevice.Viewport.TitleSafeArea.Y), Color.Yellow);

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

			// Move left
			if (currentKeyboardState.IsKeyDown(Keys.Left) || currentGamePadState.DPad.Left == ButtonState.Pressed)
			{
				player.Position.X -= playerMoveSpeed;
			}

			// Move right
			if (currentKeyboardState.IsKeyDown(Keys.Right) || currentGamePadState.DPad.Right == ButtonState.Pressed)
			{
				player.Position.X += playerMoveSpeed;
			}

			// Move up
			if (currentKeyboardState.IsKeyDown(Keys.Up) || currentGamePadState.DPad.Up == ButtonState.Pressed)
			{
				player.Position.Y -= playerMoveSpeed;
			}

			// Move down
			if (currentKeyboardState.IsKeyDown(Keys.Down) || currentGamePadState.DPad.Down == ButtonState.Pressed)
			{
				player.Position.Y += playerMoveSpeed;
			}

			// Fire marshmallows
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

			if (player.Health <= 0)
			{
				player.Health = 100;
				score = 0;
			}
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

			enemyAnimation.Initialize(enemyTexture, Vector2.Zero, 80, 40, 6, 50, Color.White, 1f, true);

			Vector2 position = new Vector2(GraphicsDevice.Viewport.Width + enemyTexture.Width / 2, random.Next(50, GraphicsDevice.Viewport.Height - 50));

			Enemy enemy = new Enemy();

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

			if (isJumping == true)
			{
				for (int j = 0; j < enemies.Count; j++)
				{
					enemies[j].EnemyMoveSpeed = 50f;
					enemySpawnTime = TimeSpan.FromSeconds(.000001);
				}
			}

			if (isInvading == true)
			{
				enemySpawnTime = TimeSpan.FromSeconds(.15f);

				fireTime = TimeSpan.FromSeconds(.00001f);
			}

			if (isSlowingDown == true)
			{
				for (int i = 0; i < enemies.Count; i++)
				{
					enemies[i].EnemyMoveSpeed = 3f;
				}
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
						if (enemies[i].Position.X <= 0)
						{
							score -= enemies[i].ScoreValue;
						}
						else
						{
							score += enemies[i].ScoreValue;
						}
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

		private void AddHealthBoost()
		{
			HealthBoost health = new HealthBoost();
			health.Initialize(GraphicsDevice.Viewport, healthBoostIcon, new Vector2((GraphicsDevice.Viewport.Width + enemyTexture.Width / 2), random.Next(50, GraphicsDevice.Viewport.Height - 50)));
			healthy.Add(health);
		}

		private void UpdateHealthBoost(GameTime gameTime)
		{
			if (gameTime.TotalGameTime - previousHealthSpawnTime > healthSpawnTime)
			{
				previousHealthSpawnTime = gameTime.TotalGameTime;

				AddHealthBoost();
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

		private void AddSpeed()
		{
			Animation speedAnimation = new Animation();
			speedAnimation.Initialize(speedIcon, Vector2.Zero, 40, 40, 10, 30, Color.White, 1f, true);

			Vector2 position = new Vector2(GraphicsDevice.Viewport.Width + enemyTexture.Width / 2, random.Next(50, GraphicsDevice.Viewport.Height - 50));

			SpeedPower speedPower = new SpeedPower();
			speedPower.Initialize(speedAnimation, position);
			speed.Add(speedPower);
		}

		private void UpdateSpeed(GameTime gameTime)
		{
			if (gameTime.TotalGameTime - previousSpeedSpawnTime > speedSpawnTime)
			{
				previousSpeedSpawnTime = gameTime.TotalGameTime;

				AddSpeed();
			}

			for (int i = speed.Count - 1; i >= 0; i--)
			{
				speed[i].Update(gameTime);

				if (speed[i].Active == false)
				{
					speed.RemoveAt(i);
				}
			}
		}

		private void AddHyperSpace()
		{
			HyperSpace flash = new HyperSpace();
			flash.Initialize(GraphicsDevice.Viewport, hyperSpaceIcon, new Vector2((GraphicsDevice.Viewport.Width + enemyTexture.Width / 2), random.Next(50, GraphicsDevice.Viewport.Height - 50)));
			hyperSpace.Add(flash);
		}

		private void UpdateHyperSpace(GameTime gameTime)
		{
			if (gameTime.TotalGameTime - previousHyperSpaceSpawnTime > hyperSpaceSpawnTime)
			{
				previousHyperSpaceSpawnTime = gameTime.TotalGameTime;

				AddHyperSpace();
			}

			for (int i = hyperSpace.Count - 1; i >= 0; i--)
			{
				hyperSpace[i].Update(gameTime);

				if (hyperSpace[i].Active == false)
				{
					hyperSpace.RemoveAt(i);
				}
			}
		}

		private void UpdateCollisions()
		{
			// Use the Rectangle's built-in intersect function to 
			// determine if two objects are overlapping
			Rectangle rectangle1;
			Rectangle rectangle2;

			// Only create the rectangle once for the player
			rectangle1 = new Rectangle((int)player.Position.X, (int)player.Position.Y, player.Width - 100, player.Height);

			// Player vs enemy
			for (int i = 0; i < enemies.Count; i++)
			{
				rectangle2 = new Rectangle((int)enemies[i].Position.X, (int)enemies[i].Position.Y, enemies[i].Width, enemies[i].Height);


				if (rectangle1.Intersects(rectangle2))
				{
					player.Health -= enemies[i].Damage;

					enemies[i].Health = 0;

					if (player.Health <= 0)
					{
						player.Active = false;
					}
				}
			}

			// Player vs Health power up 
			for (int i = 0; i < healthy.Count; i++)
			{
				// Create the health power up rectangle
				rectangle2 = new Rectangle((int)healthy[i].Position.X, (int)healthy[i].Position.Y, healthy[i].Width, healthy[i].Height);

				if (rectangle1.Intersects(rectangle2))
				{
					player.Health = 100;

					healthy[i].Health = 0;
				}
			}

			//Player vs Speed Power
			for (int i = 0; i < speed.Count; i++)
			{
				// Create slow motion power rectangle
				rectangle2 = new Rectangle((int)speed[i].Position.X, (int)speed[i].Position.Y, speed[i].Width, speed[i].Height);

				if (rectangle1.Intersects(rectangle2))
				{
					var power = random.Next(1, 100);

					if (power > 50)
					{
						isInvading = true;
					}
					else
					{
						isSlowingDown = true;
					}

					speed[i].Health = 0;
				}

			}

			// Player vs Hyper Space
			for (int i = 0; i < hyperSpace.Count; i++)
			{
				rectangle2 = new Rectangle((int)hyperSpace[i].Position.X, (int)hyperSpace[i].Position.Y, hyperSpace[i].Width, hyperSpace[i].Height);

				if (rectangle1.Intersects(rectangle2))
				{
					powerTimer.Stop();
					powerTimer.Start();

					hyperSpace[i].Health = 0;
				}
			}

			// Projectile vs Enemy Collision
			for (int i = 0; i < marshmallows.Count; i++)
			{
				for (int j = 0; j < enemies.Count; j++)
				{
					// Create the mashmallow rectangle
					rectangle1 = new Rectangle((int)marshmallows[i].Position.X - marshmallows[i].Width / 2, (int)marshmallows[i].Position.Y -
											   marshmallows[i].Height / 2, marshmallows[i].Width, marshmallows[i].Height);

					// Create the enemy rectangle
					rectangle2 = new Rectangle((int)enemies[j].Position.X - enemies[j].Width / 2, (int)enemies[j].Position.Y - enemies[j].Height / 2, enemies[j].Width, enemies[j].Height);

					// Determine if the two objects collided with each other
					if (rectangle1.Intersects(rectangle2))
					{
						//Enemies take damage and mashmallows disapear
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

		private void OnTimedEvent(object sender, ElapsedEventArgs e)
		{
			countSeconds--;

			isJumping = true;
			if (countSeconds == 0)
			{
				powerTimer.Stop();
				powerTimer.Close();
				isJumping = false;
				enemySpawnTime = TimeSpan.FromSeconds(1.0f);
			}
		}
	}
}