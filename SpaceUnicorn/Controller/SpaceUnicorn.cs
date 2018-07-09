using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Media;
using System.Timers;
using SpaceUnicorn.Model;
using SpaceUnicorn.Model.Powers;
using SpaceUnicorn.View;

namespace SpaceUnicorn
{
	/// <summary>
	/// This is the main type for your game.
	/// </summary>
	public class SpaceUnicorn : Game
	{
        #region Variables

        GraphicsDeviceManager _graphics;
		SpriteBatch _spriteBatch;

		KeyboardState _keyboardState;
		KeyboardState _oldKeyboardState;

		// Screens
		GameScreen _activeScreen;
		StartScreen _startScreen;
		ActionScreen _actionScreen;

		// Player
		private Player _player;
		private float _playerMoveSpeed;

		// Enemy
		private Texture2D _enemyTexture;
		private List<Enemy> _enemies;
		private TimeSpan _enemySpawnTime;
		private TimeSpan _previousEnemySpawnTime;

		// Random number generator
		private Random _random;

		// Keyboard/Gamepad states
		private KeyboardState _currentKeyboardState;
		private KeyboardState _previousKeyboardState;
		private GamePadState _currentGamePadState;
		private GamePadState _previousGamePadState;

		// Background Stuff
		private ParallaxingBackground _bgLayer1;
		private Song _gameMusic;

		// Laser
		private Texture2D _marshmallowPic;
		private List<MarshmallowLaser> _marshmallows;
		private TimeSpan _fireTime;
		private TimeSpan _previousFireTime;

		// Expolsions
		private Texture2D _explosionTexture;
		private List<Animation> _explosions;

		// Meteors
		private Texture2D _meteorTexture;
		private List<Meteors> _meteors;
		private TimeSpan _meteorSpawnRate;
		private TimeSpan _previousMeteorSpawnRate;

		// Fonts
		private int _score;
		private SpriteFont _font;

        // GameTime Variables
	    private TimeSpan _gameUpdate;
	    private TimeSpan _previousGameRate;
	    private TimeSpan _increase;

		/* Power Ups/Downs */

		// Health power up
		private Texture2D _healthBoostIcon;
		private List<HealthBoost> _healthy;
		private TimeSpan _healthSpawnTime;
		private TimeSpan _previousHealthSpawnTime;

		//  Speed power
		private Texture2D _speedIcon;
		private List<SpeedPower> _speed;
		private TimeSpan _speedSpawnTime;
		private TimeSpan _previousSpeedSpawnTime;
		private static Timer _speedTimer;
		private int _speedCountSeconds;

		// Enemy Spawn Rate power down
		private bool _isInvading;

		// Slow motion power up
		private bool _isSlowingDown;

		// Hyper space
		private Texture2D _hyperSpaceIcon;
		private List<HyperSpace> _hyperSpace;
		private TimeSpan _hyperSpaceSpawnTime;
		private TimeSpan _previousHyperSpaceSpawnTime;
		private bool _isJumping;
		private static Timer _hyperSpaceTimer;
		private int _hyperSpaceCountSeconds;

        #endregion

        #region Game Start

        public SpaceUnicorn()
		{
			_graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";
		}

        #region Initialize

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
		{
			// Initalize _player
			_player = new Player();
			_playerMoveSpeed = 8.0f;

			// Initalize _enemies
			_enemies = new List<Enemy>();
			_previousEnemySpawnTime = TimeSpan.Zero;
			_enemySpawnTime = TimeSpan.FromSeconds(1.0f);

			// Initalize _random number generator
			_random = new Random();

			// Initalize background
			_bgLayer1 = new ParallaxingBackground();

			// Initalize weapons
			_marshmallows = new List<MarshmallowLaser>();
			_fireTime = TimeSpan.FromSeconds(.15f);

			// Initalize _explosions
			_explosions = new List<Animation>();

			// Initalize _score
			_score = 0;

			// Meteors
			_meteors = new List<Meteors>();
			_previousMeteorSpawnRate = TimeSpan.Zero;
			_meteorSpawnRate = TimeSpan.FromSeconds(_random.Next(1, 30));

            // GameTime Variables
		    _gameUpdate = TimeSpan.FromSeconds(20f);
            _previousGameRate = TimeSpan.Zero;
            _increase = TimeSpan.FromSeconds(0.01f);

			/* Power ups/downs */

			// Initalize health power up
			_healthy = new List<HealthBoost>();
			_previousHealthSpawnTime = TimeSpan.Zero;
			_healthSpawnTime = TimeSpan.FromMinutes(_random.Next(1, 5));

			// Initalize _speed Power
			_speed = new List<SpeedPower>();
			_previousSpeedSpawnTime = TimeSpan.Zero;
			_speedSpawnTime = TimeSpan.FromMinutes(_random.Next(1, 3));
			_speedTimer = new Timer();
			_speedTimer.Interval = 1;
			_speedTimer.Elapsed += OnSpeedTimedEvent;
			_speedTimer.Enabled = false;
			_speedCountSeconds = 10000;

			// Initalize invasion
			_isInvading = false;

			// Initalize slow motion
			_isSlowingDown = false;

			// Initalize hyper space
			_hyperSpace = new List<HyperSpace>();
			_previousHyperSpaceSpawnTime = TimeSpan.Zero;
			_hyperSpaceSpawnTime = TimeSpan.FromMinutes(_random.Next(1, 1));
			_isJumping = false;
			_hyperSpaceTimer = new Timer();
			_hyperSpaceTimer.Interval = 1;
			_hyperSpaceTimer.Elapsed += OnHyperSpaceTimedEvent;
			_hyperSpaceTimer.Enabled = false;
			_hyperSpaceCountSeconds = 3000;

			base.Initialize();
		}

        #endregion

        #region Load Content

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
		{
			// Create a new SpriteBatch, which can be used to draw textures.
			_spriteBatch = new SpriteBatch(GraphicsDevice);

			// Menu
			string[] menuItems = { "Start Game", "High Score", "End Game" };

			_startScreen = new StartScreen(this, _spriteBatch, Content.Load<SpriteFont>("Fonts/gameFont"), Content.Load<Texture2D>("Background/startScreen"));
			Components.Add(_startScreen);
			_startScreen.Hide();

			_actionScreen = new ActionScreen(this, _spriteBatch, Content.Load<Texture2D>("Background/spaceBackground"));
			Components.Add(_actionScreen);
			_actionScreen.Hide();

			_activeScreen = _startScreen;
			_activeScreen.Show();

			// Load _player
			Animation playerAnimation = new Animation();
			Texture2D playerTexture = Content.Load<Texture2D>("Animation/Space Unicorn");
			playerAnimation.Initialize(playerTexture, Vector2.Zero, 166, 100, 1, 5, Color.White, 1f, true);
			Vector2 playerPosition = new Vector2(GraphicsDevice.Viewport.Width / 4, GraphicsDevice.Viewport.Height / 2);
			_player.Initialize(playerAnimation, playerPosition);

			// Load enemy
			_enemyTexture = Content.Load<Texture2D>("Animation/enemy");

			// Load background
			_bgLayer1.Initialize(Content, "Background/spaceBackground", GraphicsDevice.Viewport.Width, -1);

			// Load _marshmallows
			_marshmallowPic = Content.Load<Texture2D>("Animation/marshmallowLaser");

			// Load music
			_gameMusic = Content.Load<Song>("Music/Space Unicorn");

			// Load explosion
			_explosionTexture = Content.Load<Texture2D>("Animation/explosion");

			// Load _font
			_font = Content.Load<SpriteFont>("Fonts/gameFont");

			// Load _meteors
			_meteorTexture = Content.Load<Texture2D>("Animation/meteor");

			// Load powers
			_healthBoostIcon = Content.Load<Texture2D>("Powers/healthPowerUp");
			_speedIcon = Content.Load<Texture2D>("Powers/speedIncrease");
			_hyperSpaceIcon = Content.Load<Texture2D>("Powers/hyperSpace");

			// Play music
			PlayMusic(_gameMusic);
		}

        #endregion

        #region Update

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
			_keyboardState = Keyboard.GetState();

			if (_activeScreen == _startScreen)
			{
				if (CheckKey(Keys.Enter))
				{
					if (_startScreen._SelectedIndex == 0)
					{
						_activeScreen.Hide();
						_activeScreen = _actionScreen;
						_activeScreen.Show();
					}
					if (_startScreen._SelectedIndex == 1)
					{
						this.Exit();
					}
				}
			}

			_oldKeyboardState = _keyboardState;

			if (_activeScreen == _actionScreen)
			{
				// Save the previous state of the keyboard and game pad so we can determinesingle key/button presses
				_previousGamePadState = _currentGamePadState;
				_previousKeyboardState = _currentKeyboardState;

				// Read the current state of the keyboard and gamepad and store it
				_currentKeyboardState = Keyboard.GetState();
				_currentGamePadState = GamePad.GetState(PlayerIndex.One);

				// Update _player
				UpdatePlayer(gameTime);

				// Update _enemies
				UpdateEnemies(gameTime);

				// Update collisions
				UpdateCollisions();

				// Update background
				_bgLayer1.Update();

				// Update _marshmallows
				UpdateMarshallows();

				// Update the _explosions
				UpdateExplosions(gameTime);

				//Update _meteors
		//		UpdateMeteors(gameTime);

				// Update powers
				UpdateHealthBoost(gameTime);
				UpdateSpeed(gameTime);
				UpdateHyperSpace(gameTime);

			    if (gameTime.TotalGameTime - _previousGameRate > _gameUpdate)
			    {
			        _previousGameRate = gameTime.TotalGameTime;

			        _enemySpawnTime = _enemySpawnTime - (_increase + TimeSpan.FromSeconds(0.1f));
			        _fireTime = _fireTime - _increase;

                    Console.WriteLine(_enemySpawnTime + "  " + _fireTime);
			    }
			}

			base.Update(gameTime);
		}

        #endregion

        #region Draw

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
		{
			_graphics.GraphicsDevice.Clear(Color.CornflowerBlue);

			//Start drawing
			_spriteBatch.Begin();

			base.Draw(gameTime);

			if (_activeScreen == _actionScreen)
			{
				// Draw background
				_bgLayer1.Draw(_spriteBatch);

				//Draw the _player
				_player.Draw(_spriteBatch);

				// Draw _enemies
				for (int i = 0; i < _enemies.Count; i++)
				{
					_enemies[i].Draw(_spriteBatch);
				}

				// Draw mashmallows
				for (int i = 0; i < _marshmallows.Count; i++)
				{
					_marshmallows[i].Draw(_spriteBatch);
				}

				// Draw the _explosions
				for (int i = 0; i < _explosions.Count; i++)
				{
					_explosions[i].Draw(_spriteBatch);
				}

				// Draw the _meteors
				for (int i = 0; i < _meteors.Count; i++)
				{
					_meteors[i].Draw(_spriteBatch);
				}

				// Draw health boost
				for (int i = 0; i < _healthy.Count; i++)
				{
					_healthy[i].Draw(_spriteBatch);
				}

				// Draw _speed icon
				for (int i = 0; i < _speed.Count; i++)
				{
					_speed[i].Draw(_spriteBatch);
				}

				// Draw hyper space power
				for (int i = 0; i < _hyperSpace.Count; i++)
				{
					_hyperSpace[i].Draw(_spriteBatch);
				}

				// Draw the _score
				_spriteBatch.DrawString(_font, "Score: " + _score, new Vector2(GraphicsDevice.Viewport.TitleSafeArea.X + 2, GraphicsDevice.Viewport.TitleSafeArea.Y), Color.Yellow);
				// Draw the _player health
				_spriteBatch.DrawString(_font, "Health: " + _player._Health, new Vector2(GraphicsDevice.Viewport.TitleSafeArea.Width - 145, GraphicsDevice.Viewport.TitleSafeArea.Y), Color.Yellow);
			}

			//Stop drawing
			_spriteBatch.End();
		}

        #endregion

        #endregion

        #region Helper Methods

        private bool CheckKey(Keys theKey)
	    {
	        return _keyboardState.IsKeyUp(theKey) &&
	               _oldKeyboardState.IsKeyDown(theKey);
	    }

	    #endregion

        #region Player

        public void UpdatePlayer(GameTime gameTime)
		{
			_player.Update(gameTime);

			// Get Thumbstick Controls
			_player._Position.X += _currentGamePadState.ThumbSticks.Left.X * _playerMoveSpeed;
			_player._Position.Y -= _currentGamePadState.ThumbSticks.Left.Y * _playerMoveSpeed;

			// Use the Keyboard / Dpad

			// Move left
			if (_currentKeyboardState.IsKeyDown(Keys.Left) || _currentGamePadState.DPad.Left == ButtonState.Pressed)
			{
				_player._Position.X -= _playerMoveSpeed;
			}

			// Move right
			if (_currentKeyboardState.IsKeyDown(Keys.Right) || _currentGamePadState.DPad.Right == ButtonState.Pressed)
			{
				_player._Position.X += _playerMoveSpeed;
			}

			// Move up
			if (_currentKeyboardState.IsKeyDown(Keys.Up) || _currentGamePadState.DPad.Up == ButtonState.Pressed)
			{
				_player._Position.Y -= _playerMoveSpeed;
			}

			// Move down
			if (_currentKeyboardState.IsKeyDown(Keys.Down) || _currentGamePadState.DPad.Down == ButtonState.Pressed)
			{
				_player._Position.Y += _playerMoveSpeed;
			}

			// Fire _marshmallows
			if (_currentKeyboardState.IsKeyDown(Keys.Space))
			{
				if (gameTime.TotalGameTime - _previousFireTime > _fireTime)
				{
					_previousFireTime = gameTime.TotalGameTime;

					AddMarshmallow(_player._Position - new Vector2(-1 * (_player._Width / 2), _player._Height / 3));
				}
			}

			// Make sure that the _player does not go out of bounds
			_player._Position.X = MathHelper.Clamp(_player._Position.X, 10, 790);
			_player._Position.Y = MathHelper.Clamp(_player._Position.Y, 10, 470);

			if (_player._Health <= 0)
			{
				_player._Health = 100;
				_score = 0;

				_activeScreen.Hide();
				_activeScreen = _startScreen;
				_activeScreen.Show();
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
			enemyAnimation.Initialize(_enemyTexture, Vector2.Zero, 80, 40, 6, 50, Color.White, 1f, true);
			Vector2 position = new Vector2(GraphicsDevice.Viewport.Width + _enemyTexture.Width / 2, _random.Next(50, GraphicsDevice.Viewport.Height - 50));

			// Initalize enemy
			Enemy enemy = new Enemy();
			enemy.Initialize(enemyAnimation, position);

			// Add enemy
			_enemies.Add(enemy);
		}

		private void UpdateEnemies(GameTime gameTime)
		{
			// Adds enemy every _enemySpawnTime
			if (gameTime.TotalGameTime - _previousEnemySpawnTime > _enemySpawnTime)
			{
				_previousEnemySpawnTime = gameTime.TotalGameTime;

				AddEnemy();
			}

			// If hyper space is true
			if (_isJumping == true)
			{
				for (int j = 0; j < _enemies.Count; j++)
				{
					// Increase spawn time and move _speed
					_enemies[j]._EnemyMoveSpeed = 50f;
					_enemySpawnTime = TimeSpan.FromSeconds(.000001);
				}
			}

			// If invading is true
			if (_isInvading == true)
			{
				// Increase enemy and marshmallow spawn time
				_enemySpawnTime = TimeSpan.FromSeconds(.15f);
				_fireTime = TimeSpan.FromSeconds(.00001f);
			}

			// If slow motion is true
			if (_isSlowingDown == true)
			{
				for (int i = 0; i < _enemies.Count; i++)
				{
					// Decrease move _speed
					_enemies[i]._EnemyMoveSpeed = 3f;
				}
			}

			for (int i = _enemies.Count - 1; i >= 0; i--)
			{
				_enemies[i].Update(gameTime);

				if (_enemies[i]._Active == false)
				{
					// If not active and health <= 0
					if (_enemies[i]._Health <= 0)
					{
						// Add an explosion
						AddExplosion(_enemies[i]._Position);

						// If _enemies leave the screen still active
						if (_enemies[i]._Position.X <= 0)
						{
							// Subtract scorevalue
							_score -= _enemies[i]._ScoreValue;
						}
						else
						{
							_score += _enemies[i]._ScoreValue;
						}
					}

					_enemies.RemoveAt(i);
				}
			}
		}

        #endregion

        #region Meteors

        private void AddMeteors()
		{
			Animation meteorAnimation = new Animation();
			meteorAnimation.Initialize(_meteorTexture, Vector2.Zero, 100, 100, 9, 50, Color.White, 1f, true);
			Vector2 postion = new Vector2(GraphicsDevice.Viewport.Width + _meteorTexture.Width / 2, _random.Next(50, GraphicsDevice.Viewport.Height - 50));

			Meteors meteor = new Meteors();
			meteor.Initialize(meteorAnimation, postion);

			_meteors.Add(meteor);
		}

		private void UpdateMeteors(GameTime gameTime)
		{
			// Adds meteor every meteorSpawnTime
			if (gameTime.TotalGameTime - _previousMeteorSpawnRate > _meteorSpawnRate)
			{
				_previousMeteorSpawnRate = gameTime.TotalGameTime;

				AddMeteors();
			}

            // Loop through list of _meteors
			for (int i = _meteors.Count - 1; i >= 0; i--)
			{
				_meteors[i].Update(gameTime);

				if (_meteors[i]._Active == false)
				{
					if (_meteors[i]._Health <= 0)
					{
						AddExplosion(_meteors[i]._Position);
					}
					_meteors.RemoveAt(i);
				}
			}
		}

        #endregion

        #region Marshmallows

        private void AddMarshmallow(Vector2 position)
		{
			// Initalize _marshmallows
			MarshmallowLaser marshmallow = new MarshmallowLaser();
			marshmallow.Initialize(GraphicsDevice.Viewport, _marshmallowPic, position);

            // Add mashmallow
			_marshmallows.Add(marshmallow);
		}

		private void UpdateMarshallows()
		{
			for (int i = _marshmallows.Count - 1; i >= 0; i--)
			{
				_marshmallows[i].Update();

				// If _marshmallows hit something or leave the screen
				if (_marshmallows[i]._Active == false)
				{
                    // Remove _marshmallows
				    _marshmallows.RemoveAt(i);
				}
			}
		}

        #endregion

        #region Health Boost

        private void AddHealthBoost()
		{
            // Initalize health boost
		    HealthBoost health = new HealthBoost();
		    health.Initialize(GraphicsDevice.Viewport, _healthBoostIcon,
		        new Vector2((GraphicsDevice.Viewport.Width + _enemyTexture.Width / 2),
		            _random.Next(50, GraphicsDevice.Viewport.Height - 50)));

            // Add health boost
			_healthy.Add(health);
		}

		private void UpdateHealthBoost(GameTime gameTime)
		{
            // Adds helath boost every _healthSpawnTime
		    if (gameTime.TotalGameTime - _previousHealthSpawnTime > _healthSpawnTime)
			{
				_previousHealthSpawnTime = gameTime.TotalGameTime;

				AddHealthBoost();
			}

            // Loop through health boost list
			for (int i = _healthy.Count - 1; i >= 0; i--)
			{
                // Update each boost
			    _healthy[i].Update(gameTime);

			    if (_healthy[i]._Active == false)
				{
				    _healthy.RemoveAt(i);
				}
			}
		}

        #endregion

        #region Speed

        private void AddSpeed()
		{
			Animation speedAnimation = new Animation();
			speedAnimation.Initialize(_speedIcon, Vector2.Zero, 40, 40, 10, 30, Color.White, 1f, true);

			Vector2 position = new Vector2(GraphicsDevice.Viewport.Width + _enemyTexture.Width / 2, _random.Next(50, GraphicsDevice.Viewport.Height - 50));

			SpeedPower speedPower = new SpeedPower();
			speedPower.Initialize(speedAnimation, position);
			_speed.Add(speedPower);
		}

		private void UpdateSpeed(GameTime gameTime)
		{
			if (gameTime.TotalGameTime - _previousSpeedSpawnTime > _speedSpawnTime)
			{
				_previousSpeedSpawnTime = gameTime.TotalGameTime;

				AddSpeed();
			}

			for (int i = _speed.Count - 1; i >= 0; i--)
			{
				_speed[i].Update(gameTime);

				if (_speed[i]._Active == false || _hyperSpace.Count > 0)
				{
					_speed.RemoveAt(i);
				}
			}
		}

        private void OnSpeedTimedEvent(object sender, ElapsedEventArgs e)
        {
            _speedCountSeconds--;

            var power = _random.Next(1, 100);

            if (power > 50)
            {
                _isInvading = true;
            }
            else
            {
                _isSlowingDown = true;
            }

            if (_speedCountSeconds == 0)
            {
                _isInvading = false;
                _isSlowingDown = false;

                _speedTimer.Stop();
                _enemySpawnTime = TimeSpan.FromSeconds(1.0f);
                _fireTime = TimeSpan.FromSeconds(.15f);
            }
        }
  
        #endregion

        #region Hyper Space

        private void AddHyperSpace()
		{
			HyperSpace flash = new HyperSpace();
			flash.Initialize(GraphicsDevice.Viewport, _hyperSpaceIcon, new Vector2((GraphicsDevice.Viewport.Width + _enemyTexture.Width / 2), _random.Next(50, GraphicsDevice.Viewport.Height - 50)));
			_hyperSpace.Add(flash);
		}

		private void UpdateHyperSpace(GameTime gameTime)
		{
			if (gameTime.TotalGameTime - _previousHyperSpaceSpawnTime > _hyperSpaceSpawnTime)
			{
				_previousHyperSpaceSpawnTime = gameTime.TotalGameTime;

				AddHyperSpace();
			}

			for (int i = _hyperSpace.Count - 1; i >= 0; i--)
			{
				_hyperSpace[i].Update(gameTime);

				if (_hyperSpace[i]._Active == false)
				{
					_hyperSpace.RemoveAt(i);
				}
			}
		}

	    private void OnHyperSpaceTimedEvent(object sender, ElapsedEventArgs e)
	    {
	        _hyperSpaceCountSeconds--;

	        _isJumping = true;
	        if (_hyperSpaceCountSeconds == 0)
	        {
	            _hyperSpaceTimer.Stop();
	            _hyperSpaceTimer.Close();
	            _isJumping = false;
	            _enemySpawnTime = TimeSpan.FromSeconds(1.0f);
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

			// Only create the rectangle once for the _player
			rectangle1 = new Rectangle((int)_player._Position.X, (int)_player._Position.Y, _player._Width - 100, _player._Height);

			// Player vs enemy
			for (int i = 0; i < _enemies.Count; i++)
			{
				rectangle2 = new Rectangle((int)_enemies[i]._Position.X, (int)_enemies[i]._Position.Y, _enemies[i]._Width, _enemies[i]._Height);

				if (rectangle1.Intersects(rectangle2))
				{
					_player._Health -= _enemies[i]._Damage;

					_enemies[i]._Health = 0;

					if (_player._Health <= 0)
					{
						_player._Active = false;
					}
				}
			}

			// Player vs Meteor
			for (int i = 0; i < _meteors.Count; i++)
			{
				rectangle2 = new Rectangle((int)_meteors[i]._Position.X, (int)_meteors[i]._Position.Y, _meteors[i]._Width, _meteors[i]._Height);

				if (rectangle1.Intersects(rectangle2))
				{
					_player._Health -= _meteors[i]._Damage;

					_meteors[i]._Health = 0;

					if (_player._Health <= 0)
					{
						_player._Active = false;
					}
				}
			}

			// Player vs Health power up 
			for (int i = 0; i < _healthy.Count; i++)
			{
				// Create the health power up rectangle
				rectangle2 = new Rectangle((int)_healthy[i]._Position.X, (int)_healthy[i]._Position.Y, _healthy[i]._Width, _healthy[i]._Height);

				if (rectangle1.Intersects(rectangle2))
				{
					_player._Health = 100;
					_healthy.RemoveAt(i);
				}
			}

			//Player vs Speed Power
			for (int i = 0; i < _speed.Count; i++)
			{
				// Create slow motion power rectangle
				rectangle2 = new Rectangle((int)_speed[i]._Position.X, (int)_speed[i]._Position.Y, _speed[i]._Width, _speed[i]._Height);

				if (rectangle1.Intersects(rectangle2))
				{
					_speedTimer.Stop();
					_speedTimer.Start();
					_speed.RemoveAt(i);
				}
			}

			// Player vs Hyper Space
			for (int i = 0; i < _hyperSpace.Count; i++)
			{
				rectangle2 = new Rectangle((int)_hyperSpace[i]._Position.X, (int)_hyperSpace[i]._Position.Y, _hyperSpace[i]._Width, _hyperSpace[i]._Height);

				if (rectangle1.Intersects(rectangle2))
				{
					_hyperSpaceTimer.Stop();
					_hyperSpaceTimer.Start();
					_hyperSpace.RemoveAt(i);
				}
			}

			// Projectile vs Enemy Collision
			for (int i = 0; i < _marshmallows.Count; i++)
			{
				for (int j = 0; j < _enemies.Count; j++)
				{
					// Create the mashmallow rectangle
					rectangle1 = new Rectangle((int)_marshmallows[i]._Position.X - _marshmallows[i]._Width / 2, (int)_marshmallows[i]._Position.Y -
											   _marshmallows[i]._Height / 2, _marshmallows[i]._Width, _marshmallows[i]._Height);

					// Create the enemy rectangle
					rectangle2 = new Rectangle((int)_enemies[j]._Position.X - _enemies[j]._Width / 2, (int)_enemies[j]._Position.Y - _enemies[j]._Height / 2, _enemies[j]._Width, _enemies[j]._Height);

					// Determine if the two objects collided with each other
					if (rectangle1.Intersects(rectangle2))
					{
						//Enemies take damage and mashmallows disapear
						_enemies[j]._Health -= _marshmallows[i]._Damage;
						_marshmallows[i]._Active = false;
					}
				}
			}
		}

		private void AddExplosion(Vector2 position)
		{
			Animation explosion = new Animation();
			explosion.Initialize(_explosionTexture, position, 134, 134, 12, 35, Color.White, 1f, false);
			_explosions.Add(explosion);
		}

		private void UpdateExplosions(GameTime gameTime)
		{
			for (int i = _explosions.Count - 1; i >= 0; i--)
			{
				_explosions[i].Update(gameTime);
				if (_explosions[i]._Active == false)
				{
					_explosions.RemoveAt(i);
				}
			}
		}

        #endregion 
     }
}	