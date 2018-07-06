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
        #region Variables

        GraphicsDeviceManager graphics;
		SpriteBatch spriteBatch;

		KeyboardState keyboardState;
		KeyboardState oldKeyboardState;

		// Screens
		GameScreen activeScreen;
		StartScreen startScreen;
		ActionScreen actionScreen;

		// Player
		private Player player;
		private float playerMoveSpeed;

		// Enemy
		private Texture2D enemyTexture;
		private List<Enemy> enemies;
		private TimeSpan enemySpawnTime;
		private TimeSpan previousEnemySpawnTime;

		// Random number generator
		private Random random;

		// Keyboard/Gamepad states
		private KeyboardState currentKeyboardState;
		private KeyboardState previousKeyboardState;
		private GamePadState currentGamePadState;
		private GamePadState previousGamePadState;

		// Background Stuff
		private ParallaxingBackground bgLayer1;
		private Song gameMusic;

		// Laser
		private Texture2D marshmallowPic;
		private List<MarshmallowLaser> marshmallows;
		private TimeSpan fireTime;
		private TimeSpan previousFireTime;

		// Expolsions
		private Texture2D explosionTexture;
		private List<Animation> explosions;

		// Meteors
		private Texture2D meteorTexture;
		private List<Meteors> meteors;
		private TimeSpan meteorSpawnRate;
		private TimeSpan previousMeteorSpawnRate;

		// Fonts
		private int score;
		private SpriteFont font;

		/* Power Ups/Downs */

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
		private static Timer speedTimer;
		private int speedCountSeconds;

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
		private static Timer hyperSpaceTimer;
		private int hyperSpaceCountSeconds;

        #endregion

        #region Game Start

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
			// Initalize player
			player = new Player();
			playerMoveSpeed = 8.0f;

			// Initalize enemies
			enemies = new List<Enemy>();
			previousEnemySpawnTime = TimeSpan.Zero;
			enemySpawnTime = TimeSpan.FromSeconds(1.0f);

			// Initalize random number generator
			random = new Random();

			// Initalize background
			bgLayer1 = new ParallaxingBackground();

			// Initalize weapons
			marshmallows = new List<MarshmallowLaser>();
			fireTime = TimeSpan.FromSeconds(.15f);

			// Initalize explosions
			explosions = new List<Animation>();

			// Initalize score
			score = 0;

			// Meteors
			meteors = new List<Meteors>();
			previousMeteorSpawnRate = TimeSpan.Zero;
			meteorSpawnRate = TimeSpan.FromSeconds(random.Next(1, 30));

			/* Power ups/downs */

			// Initalize health power up
			healthy = new List<HealthBoost>();
			previousHealthSpawnTime = TimeSpan.Zero;
			healthSpawnTime = TimeSpan.FromMinutes(random.Next(1, 5));

			// Initalize speed Power
			speed = new List<SpeedPower>();
			previousSpeedSpawnTime = TimeSpan.Zero;
			speedSpawnTime = TimeSpan.FromMinutes(random.Next(1, 3));
			speedTimer = new Timer();
			speedTimer.Interval = 1;
			speedTimer.Elapsed += OnSpeedTimedEvent;
			speedTimer.Enabled = false;
			speedCountSeconds = 10000;

			// Initalize invasion
			isInvading = false;

			// Initalize slow motion
			isSlowingDown = false;

			// Initalize hyper space
			hyperSpace = new List<HyperSpace>();
			previousHyperSpaceSpawnTime = TimeSpan.Zero;
			hyperSpaceSpawnTime = TimeSpan.FromMinutes(random.Next(1, 1));
			isJumping = false;
			hyperSpaceTimer = new Timer();
			hyperSpaceTimer.Interval = 1;
			hyperSpaceTimer.Elapsed += OnHyperSpaceTimedEvent;
			hyperSpaceTimer.Enabled = false;
			hyperSpaceCountSeconds = 3000;

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

			// Menu
			string[] menuItems = { "Start Game", "High Score", "End Game" };

			startScreen = new StartScreen(this, spriteBatch, Content.Load<SpriteFont>("Fonts/gameFont"), Content.Load<Texture2D>("Background/startScreen"));
			Components.Add(startScreen);
			startScreen.Hide();

			actionScreen = new ActionScreen(this, spriteBatch, Content.Load<Texture2D>("Background/spaceBackground"));
			Components.Add(actionScreen);
			actionScreen.Hide();

			activeScreen = startScreen;
			activeScreen.Show();

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

			// Load meteors
			meteorTexture = Content.Load<Texture2D>("Animation/meteor");

			// Load powers
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
			keyboardState = Keyboard.GetState();

			if (activeScreen == startScreen)
			{
				if (CheckKey(Keys.Enter))
				{
					if (startScreen.SelectedIndex == 0)
					{
						activeScreen.Hide();
						activeScreen = actionScreen;
						activeScreen.Show();
					}
					if (startScreen.SelectedIndex == 1)
					{
						this.Exit();
					}
				}
			}

			oldKeyboardState = keyboardState;

			if (activeScreen == actionScreen)
			{
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

				//Update meteors
				UpdateMeteors(gameTime);

				// Update powers
				UpdateHealthBoost(gameTime);
				UpdateSpeed(gameTime);
				UpdateHyperSpace(gameTime);
			}

			base.Update(gameTime);
		}

		private bool CheckKey(Keys theKey)
		{
			return keyboardState.IsKeyUp(theKey) &&
				oldKeyboardState.IsKeyDown(theKey);
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

			base.Draw(gameTime);

			if (activeScreen == actionScreen)
			{
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

				// Draw the meteors
				for (int i = 0; i < meteors.Count; i++)
				{
					meteors[i].Draw(spriteBatch);
				}

				// Draw health boost
				for (int i = 0; i < healthy.Count; i++)
				{
					healthy[i].Draw(spriteBatch);
				}

				// Draw speed icon
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
			}

			//Stop drawing
			spriteBatch.End();
		}

        #endregion

        #region Player

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

				activeScreen.Hide();
				activeScreen = startScreen;
				activeScreen.Show();
			}
		}

        #endregion

        #region Music

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

        #endregion

        #region Enemy

        private void AddEnemy()
		{
			// Initalize enemy animation
			Animation enemyAnimation = new Animation();
			enemyAnimation.Initialize(enemyTexture, Vector2.Zero, 80, 40, 6, 50, Color.White, 1f, true);
			Vector2 position = new Vector2(GraphicsDevice.Viewport.Width + enemyTexture.Width / 2, random.Next(50, GraphicsDevice.Viewport.Height - 50));

			// Initalize enemy
			Enemy enemy = new Enemy();
			enemy.Initialize(enemyAnimation, position);

			// Add enemy
			enemies.Add(enemy);
		}

		private void UpdateEnemies(GameTime gameTime)
		{
			// Adds enemy every enemySpawnTime
			if (gameTime.TotalGameTime - previousEnemySpawnTime > enemySpawnTime)
			{
				previousEnemySpawnTime = gameTime.TotalGameTime;

				AddEnemy();
			}

			// If hyper space is true
			if (isJumping == true)
			{
				for (int j = 0; j < enemies.Count; j++)
				{
					// Increase spawn time and move speed
					enemies[j].EnemyMoveSpeed = 50f;
					enemySpawnTime = TimeSpan.FromSeconds(.000001);
				}
			}

			// If invading is true
			if (isInvading == true)
			{
				// Increase enemy and marshmallow spawn time
				enemySpawnTime = TimeSpan.FromSeconds(.15f);
				fireTime = TimeSpan.FromSeconds(.00001f);
			}

			// If slow motion is true
			if (isSlowingDown == true)
			{
				for (int i = 0; i < enemies.Count; i++)
				{
					// Decrease move speed
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

						// If enemies leave the screen still active
						if (enemies[i].Position.X <= 0)
						{
							// Subtract scorevalue
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

        #endregion

        #region Meteors

        private void AddMeteors()
		{
			Animation meteorAnimation = new Animation();
			meteorAnimation.Initialize(meteorTexture, Vector2.Zero, 100, 100, 9, 50, Color.White, 1f, true);
			Vector2 postion = new Vector2(GraphicsDevice.Viewport.Width + meteorTexture.Width / 2, random.Next(50, GraphicsDevice.Viewport.Height - 50));

			Meteors meteor = new Meteors();
			meteor.Initialize(meteorAnimation, postion);

			meteors.Add(meteor);
		}

		private void UpdateMeteors(GameTime gameTime)
		{
			// Adds meteor every meteorSpawnTime
			if (gameTime.TotalGameTime - previousMeteorSpawnRate > meteorSpawnRate)
			{
				previousMeteorSpawnRate = gameTime.TotalGameTime;

				AddMeteors();
			}

            // Loop through list of meteors
			for (int i = meteors.Count - 1; i >= 0; i--)
			{
				meteors[i].Update(gameTime);

				if (meteors[i].Active == false)
				{
					if (meteors[i].Health <= 0)
					{
						AddExplosion(meteors[i].Position);
					}
					meteors.RemoveAt(i);
				}
			}
		}

        #endregion

        #region Marshmallows

        private void AddMarshmallow(Vector2 position)
		{
			// Initalize marshmallows
			MarshmallowLaser marshmallow = new MarshmallowLaser();
			marshmallow.Initialize(GraphicsDevice.Viewport, marshmallowPic, position);

            // Add mashmallow
			marshmallows.Add(marshmallow);
		}

		private void UpdateMarshallows()
		{
			for (int i = marshmallows.Count - 1; i >= 0; i--)
			{
				marshmallows[i].Update();

				// If marshmallows hit something or leave the screen
				if (marshmallows[i].Active == false)
				{
                    // Remove marshmallows
				    marshmallows.RemoveAt(i);
				}
			}
		}

        #endregion

        #region Health Boost

        private void AddHealthBoost()
		{
            // Initalize health boost
		    HealthBoost health = new HealthBoost();
		    health.Initialize(GraphicsDevice.Viewport, healthBoostIcon,
		        new Vector2((GraphicsDevice.Viewport.Width + enemyTexture.Width / 2),
		            random.Next(50, GraphicsDevice.Viewport.Height - 50)));

            // Add health boost
			healthy.Add(health);
		}

		private void UpdateHealthBoost(GameTime gameTime)
		{
            // Adds helath boost every healthSpawnTime
		    if (gameTime.TotalGameTime - previousHealthSpawnTime > healthSpawnTime)
			{
				previousHealthSpawnTime = gameTime.TotalGameTime;

				AddHealthBoost();
			}

            // Loop through health boost list
			for (int i = healthy.Count - 1; i >= 0; i--)
			{
                // Update each boost
			    healthy[i].Update(gameTime);

			    if (healthy[i].Active == false)
				{
				    healthy.RemoveAt(i);
				}
			}
		}

        #endregion

        #region Speed

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

        private void OnSpeedTimedEvent(object sender, ElapsedEventArgs e)
        {
            speedCountSeconds--;

            var power = random.Next(1, 100);

            if (power > 50)
            {
                isInvading = true;
            }
            else
            {
                isSlowingDown = true;
            }

            if (speedCountSeconds == 0)
            {
                isInvading = false;
                isSlowingDown = false;

                speedTimer.Stop();
                enemySpawnTime = TimeSpan.FromSeconds(1.0f);
                fireTime = TimeSpan.FromSeconds(.15f);
            }
        }
  
        #endregion

        #region Hyper Space

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

	    private void OnHyperSpaceTimedEvent(object sender, ElapsedEventArgs e)
	    {
	        hyperSpaceCountSeconds--;

	        isJumping = true;
	        if (hyperSpaceCountSeconds == 0)
	        {
	            hyperSpaceTimer.Stop();
	            hyperSpaceTimer.Close();
	            isJumping = false;
	            enemySpawnTime = TimeSpan.FromSeconds(1.0f);
	        }
	    }

        #endregion

        #region Collisions

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

			// Player vs Meteor
			for (int i = 0; i < meteors.Count; i++)
			{
				rectangle2 = new Rectangle((int)meteors[i].Position.X, (int)meteors[i].Position.Y, meteors[i].Width, meteors[i].Height);

				if (rectangle1.Intersects(rectangle2))
				{
					player.Health -= meteors[i].Damage;

					meteors[i].Health = 0;

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
					healthy.RemoveAt(i);
				}
			}

			//Player vs Speed Power
			for (int i = 0; i < speed.Count; i++)
			{
				// Create slow motion power rectangle
				rectangle2 = new Rectangle((int)speed[i].Position.X, (int)speed[i].Position.Y, speed[i].Width, speed[i].Height);

				if (rectangle1.Intersects(rectangle2))
				{
					speedTimer.Stop();
					speedTimer.Start();
					speed.RemoveAt(i);
				}
			}

			// Player vs Hyper Space
			for (int i = 0; i < hyperSpace.Count; i++)
			{
				rectangle2 = new Rectangle((int)hyperSpace[i].Position.X, (int)hyperSpace[i].Position.Y, hyperSpace[i].Width, hyperSpace[i].Height);

				if (rectangle1.Intersects(rectangle2))
				{
					hyperSpaceTimer.Stop();
					hyperSpaceTimer.Start();
					hyperSpace.RemoveAt(i);
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
			explosion.Initialize(explosionTexture, position, 134, 134, 12, 35, Color.White, 1f, false);
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

        #endregion 
     }
}

		