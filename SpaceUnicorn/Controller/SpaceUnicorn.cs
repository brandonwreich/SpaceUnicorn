using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Media;
using System.Timers;
using SpaceUnicorn.Model;
using SpaceUnicorn.Model.Powers;
using SpaceUnicorn.Screens;
using SpaceUnicorn.View;

/** TODO Think about
 * Level with Snowman
 * When killed breakes into 3 and have to rekill
 */

/* TODO NOW!!!!
 * Fairy's that come to help
 * Change the bomb icon
 */
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

        // Fairy
	    private Texture2D _fairyTexture;
        private List<Fairy> _faries;

		// Enemy
		private Texture2D _enemyTexture;
		private List<Enemy> _enemies;
		private TimeSpan _enemySpawnTime;
		private TimeSpan _previousEnemySpawnTime;

		// Random number generator
		private Random _random;

		// Keyboard/Gamepad states
		private KeyboardState _currentKeyboardState;
		private GamePadState _currentGamePadState;

		// Background Stuff
		private ParallaxingBackground _bgLayer1;
		private Song _gameMusic;

		// Laser
		private Texture2D _marshmallowIcon;
		private List<MarshmallowLaser> _marshmallows;
		private TimeSpan _fireTime;
		private TimeSpan _previousFireTime;

		// Expolsions
		private Texture2D _explosionTexture;
		private List<Animation> _explosions;

		// Fonts
		private int _score;
		private SpriteFont _font;

        // GameTime Variables
	    private TimeSpan _gameUpdate;
	    private TimeSpan _previousGameUpdate;
	    private TimeSpan _increaseSpawn;
        private TimeSpan _increaseFire;

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
		private TimeSpan _wasJumping;
		private bool _isJumping;
		private static Timer _hyperSpaceTimer;
		private int _hyperSpaceCountSeconds;

        // Savior
	    private Texture2D _saviorIcon;
	    private List<Savior> _saveMe;
	    private TimeSpan _isSaving;
	    private TimeSpan _wasSaving;

        // Add Fairy
	    private Texture2D _addFairyIcon;
	    private List<AddFairy> _addFairies;
	    private TimeSpan _isAddingFairy;
	    private TimeSpan _wasAddingFairy;

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

            // Initalize Faries
            _faries = new List<Fairy>();

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

            // GameTime Variables
		    _gameUpdate = TimeSpan.FromSeconds(10f);
            _previousGameUpdate = TimeSpan.Zero;
            _increaseSpawn = TimeSpan.FromSeconds(0.001f);
            _increaseFire = TimeSpan.FromSeconds(0.001f);

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
			_wasJumping = TimeSpan.Zero;
			_hyperSpaceSpawnTime = TimeSpan.FromMinutes(_random.Next(1, 1));
			_isJumping = false;
			_hyperSpaceTimer = new Timer();
			_hyperSpaceTimer.Interval = 1;
			_hyperSpaceTimer.Elapsed += OnHyperSpaceTimedEvent;
			_hyperSpaceTimer.Enabled = false;
			_hyperSpaceCountSeconds = 3000;

            // Savior
            _saveMe = new List<Savior>();
            _wasSaving = TimeSpan.Zero;
		    _isSaving = TimeSpan.FromSeconds(20f);

            // Add Fairy
            _addFairies = new List<AddFairy>();
            _isAddingFairy = TimeSpan.FromSeconds(40f);
            _wasAddingFairy = TimeSpan.Zero;


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
		    _startScreen = new StartScreen(this, _spriteBatch, Content.Load<SpriteFont>("Fonts/gameFont"),
		        Content.Load<Texture2D>("Background/startScreen"));
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
			_marshmallowIcon = Content.Load<Texture2D>("Animation/marshmallowLaser");

			// Load music
			_gameMusic = Content.Load<Song>("Music/Space Unicorn");

			// Load explosion
			_explosionTexture = Content.Load<Texture2D>("Animation/explosion");

			// Load _font
			_font = Content.Load<SpriteFont>("Fonts/gameFont");

			// Load powers
			_healthBoostIcon = Content.Load<Texture2D>("Powers/healthPowerUp");
			_speedIcon = Content.Load<Texture2D>("Powers/speedIncrease");
			_hyperSpaceIcon = Content.Load<Texture2D>("Powers/hyperSpace");
		    _saviorIcon = Content.Load<Texture2D>("Powers/bomb");
		    _addFairyIcon = Content.Load<Texture2D>("Powers/addFairy");

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
					if (_startScreen.SelectedIndex == 0)
					{
						_activeScreen.Hide();
						_activeScreen = _actionScreen;
						_activeScreen.Show();
					}
					if (_startScreen.SelectedIndex == 1)
					{
						Exit();
					}
				}
			}

			_oldKeyboardState = _keyboardState;

			if (_activeScreen == _actionScreen)
			{
				// Read the current state of the keyboard and gamepad
				// and store it
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

				// Update powers
				UpdateHealthBoost(gameTime);
				UpdateSpeed(gameTime);
				UpdateHyperSpace(gameTime);
                UpdateSavior(gameTime);
                UpdateFairyPower(gameTime);

			    if (gameTime.TotalGameTime - _previousGameUpdate > _gameUpdate)
			    {
			        _previousGameUpdate = gameTime.TotalGameTime;

			        _enemySpawnTime = _enemySpawnTime - _increaseSpawn;
			        _fireTime = _fireTime + _increaseFire;

                    Console.WriteLine(_enemySpawnTime + "  " + _fireTime);
			    }

                _increaseSpawn = _increaseSpawn + TimeSpan.FromSeconds(0.1f);
                _increaseFire = _increaseFire + TimeSpan.FromSeconds(0.5f);
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

                // Draw Savior power up
			    for (int i = 0; i < _saveMe.Count; i++)
			    {
                    _saveMe[i].Draw(_spriteBatch);
			    }

                // Draw the AddFairy power up
			    for (int i = 0; i < _addFairies.Count; i++)
			    {
                    _addFairies[i].Draw(_spriteBatch);
			    }

				// Draw the _score
			    _spriteBatch.DrawString(_font, "Score: " + _score,
			        new Vector2(GraphicsDevice.Viewport.TitleSafeArea.X + 2, GraphicsDevice.Viewport.TitleSafeArea.Y),
			        Color.Yellow);
				// Draw the _player health
			    _spriteBatch.DrawString(_font, "Health: " + _player.Health,
			        new Vector2(GraphicsDevice.Viewport.TitleSafeArea.Width - 145,
			            GraphicsDevice.Viewport.TitleSafeArea.Y), Color.Yellow);
			}

			//Stop drawing
			_spriteBatch.End();
		}

        #endregion

        #endregion

        #region Player

        private void UpdatePlayer(GameTime gameTime)
		{
			_player.Update(gameTime);

			// Get Thumbstick Controls
			_player.Position.X += _currentGamePadState.ThumbSticks.Left.X * _playerMoveSpeed;
			_player.Position.Y -= _currentGamePadState.ThumbSticks.Left.Y * _playerMoveSpeed;

			// Use the Keyboard / Dpad

			// Move left
			if (_currentKeyboardState.IsKeyDown(Keys.Left) || _currentGamePadState.DPad.Left == ButtonState.Pressed)
			{
				_player.Position.X -= _playerMoveSpeed;
			}

			// Move right
			if (_currentKeyboardState.IsKeyDown(Keys.Right) || _currentGamePadState.DPad.Right == ButtonState.Pressed)
			{
				_player.Position.X += _playerMoveSpeed;
			}

			// Move up
			if (_currentKeyboardState.IsKeyDown(Keys.Up) || _currentGamePadState.DPad.Up == ButtonState.Pressed)
			{
				_player.Position.Y -= _playerMoveSpeed;
			}

			// Move down
			if (_currentKeyboardState.IsKeyDown(Keys.Down) || _currentGamePadState.DPad.Down == ButtonState.Pressed)
			{
				_player.Position.Y += _playerMoveSpeed;
			}

			// Fire _marshmallows
			if (_currentKeyboardState.IsKeyDown(Keys.Space))
			{
				if (gameTime.TotalGameTime - _previousFireTime > _fireTime)
				{
					_previousFireTime = gameTime.TotalGameTime;

				    AddMarshmallow(_player.Position - new Vector2(-1 * (_player.Width / 2), _player.Height / 3));
				}
			}

			// Make sure that the _player does not go out of bounds
			_player.Position.X = MathHelper.Clamp(_player.Position.X, 10, 790);
			_player.Position.Y = MathHelper.Clamp(_player.Position.Y, 10, 470);

			if (_player.Health <= 0)
			{
                // Reset game
				_player.Health = 100;
				_score = 0;
                _enemies.Clear();
                _healthy.Clear();
                _speed.Clear();
                _hyperSpace.Clear();
                _marshmallows.Clear();
                _explosions.Clear();
                _saveMe.Clear();

			    _isJumping = false;
			    _isInvading = false;
			    _isSlowingDown = false;

                _enemySpawnTime = TimeSpan.FromSeconds(1.0f);
			    _fireTime = TimeSpan.FromSeconds(.15f);
                
                // Show menu screen
				_activeScreen.Hide();
				_activeScreen = _startScreen;
				_activeScreen.Show();
			}
		}

        #endregion

        #region Fariy

	    private void AddFairy()
	    {
	        var yPosition = 0;

            Animation fairyAnimation = new Animation();
            fairyAnimation.Initialize(_fairyTexture, Vector2.Zero, 0, 0, 0, 0, Color.White, 1f, true);

	        Vector2 position = new Vector2(_player.Position.X, yPosition);

            Fairy fairy = new Fairy();
            fairy.Initialize(fairyAnimation, position);

            _faries.Add(fairy);
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
		    Vector2 position = new Vector2(GraphicsDevice.Viewport.Width + _enemyTexture.Width / 2,
		        _random.Next(50, GraphicsDevice.Viewport.Height - 50));

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
			if (_isJumping)
			{
				for (int j = 0; j < _enemies.Count; j++)
				{
					// Increase spawn time and move _speed
					_enemies[j].EnemyMoveSpeed = 50f;
					_enemySpawnTime = TimeSpan.FromSeconds(.000001);
				}
			}

			// If invading is true
			if (_isInvading)
			{
				// Increase enemy and marshmallow spawn time
				_enemySpawnTime = TimeSpan.FromSeconds(.15f);
				_fireTime = TimeSpan.FromSeconds(.00001f);
			}

			// If slow motion is true
			if (_isSlowingDown)
			{
				for (int i = 0; i < _enemies.Count; i++)
				{
					_enemies[i].EnemyMoveSpeed = 3f;
				}
			}

			for (int i = _enemies.Count - 1; i >= 0; i--)
			{
				_enemies[i].Update(gameTime);

			    if (_enemies[i].Position.X >= GraphicsDevice.Viewport.Width)
			    {
			        _enemies[i].Reverse = false;
			    }

			    if (_enemies[i].Position.X <= 0)
			    {
			        _enemies[i].Reverse = true;
			    }

                if (gameTime.TotalGameTime > TimeSpan.FromSeconds(1f))
                {
                    if (_enemies[i].Position.Y <= 0)
                    {
                       _enemies[i].Lift = false;
                    }

                    if (_enemies[i].Position.Y > GraphicsDevice.Viewport.Height)
                    {
                        _enemies[i].Lift = true;
                    }
                }


				if (_enemies[i].Active == false)
				{
					// If not active and health <= 0
					if (_enemies[i].Health <= 0)
					{
						AddExplosion(_enemies[i].Position);

						_score += _enemies[i].ScoreValue;
					}

					_enemies.RemoveAt(i);
				}
			}
		}

        #endregion

        #region Marshmallows

        private void AddMarshmallow(Vector2 position)
		{
			// Initalize _marshmallows
			MarshmallowLaser marshmallow = new MarshmallowLaser();
			marshmallow.Initialize(GraphicsDevice.Viewport, _marshmallowIcon, position);

            // Add mashmallow
			_marshmallows.Add(marshmallow);
		}

		private void UpdateMarshallows()
		{
			for (int i = _marshmallows.Count - 1; i >= 0; i--)
			{
				_marshmallows[i].Update();

				// If _marshmallows hit something or leave the screen
				if (_marshmallows[i].Active == false)
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
		        new Vector2((GraphicsDevice.Viewport.Width + _healthBoostIcon.Width / 2),
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

			    if (_healthy[i].Active == false)
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

		    Vector2 position = new Vector2(GraphicsDevice.Viewport.Width + _speedIcon.Width / 2,
		        _random.Next(50, GraphicsDevice.Viewport.Height - 50));

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

				if (_speed[i].Active == false || _hyperSpace.Count > 0)
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
		    flash.Initialize(GraphicsDevice.Viewport, _hyperSpaceIcon,
		        new Vector2((GraphicsDevice.Viewport.Width + _hyperSpaceIcon.Width / 2),
		            _random.Next(50, GraphicsDevice.Viewport.Height - 50)));
			_hyperSpace.Add(flash);
		}

		private void UpdateHyperSpace(GameTime gameTime)
		{
			if (gameTime.TotalGameTime - _wasJumping > _hyperSpaceSpawnTime)
			{
				_wasJumping = gameTime.TotalGameTime;

				AddHyperSpace();
			}

			for (int i = _hyperSpace.Count - 1; i >= 0; i--)
			{
				_hyperSpace[i].Update(gameTime);

				if (_hyperSpace[i].Active == false)
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

        #region Savior

	    private void AddSavior()
	    {
            Animation saviorAnimation = new Animation();
            saviorAnimation.Initialize(_saviorIcon, Vector2.Zero, 47, 40, 9, 30, Color.White, 1f, true);

	        Vector2 postion = new Vector2(GraphicsDevice.Viewport.Width + _saviorIcon.Width / 2,
	            _random.Next(50, GraphicsDevice.Viewport.Height - 50));

            Savior save = new Savior();
            save.Initialize(saviorAnimation, postion);
            _saveMe.Add(save);
	    }

	    private void UpdateSavior(GameTime gameTime)
	    {
	        if (gameTime.TotalGameTime - _wasSaving > _isSaving)
            { 
	            if (_enemies.Count >= 15)
	            {
                   _wasSaving = gameTime.TotalGameTime;

                   AddSavior();
	            }
            }

	        for(int i = _saveMe.Count - 1; i >= 0; i--)
	        {
                _saveMe[i].Update(gameTime);

	            if (_saveMe[i].Active == false)
	            {
                    _saveMe.RemoveAt(i);
	            }
	        }
	    }

        #endregion

        #region Add Fairy

	    private void AddFairyPower()
	    {
	        Animation addFairyAnimation = new Animation();
	        addFairyAnimation.Initialize(_addFairyIcon, Vector2.Zero, 32, 32, 10, 16, Color.White, 1f, true);

	        Vector2 position = new Vector2(GraphicsDevice.Viewport.Width + _addFairyIcon.Width / 2,
	            _random.Next(50, GraphicsDevice.Viewport.Height - 50));

	        AddFairy addIt = new AddFairy();
	        addIt.Initialize(addFairyAnimation, position);
	        _addFairies.Add(addIt);
	    }

	    private void UpdateFairyPower(GameTime gameTime)
	    {
	        if (gameTime.TotalGameTime - _wasAddingFairy > _isAddingFairy)
	        {
	            _wasAddingFairy = gameTime.TotalGameTime;

	            AddFairyPower();
            }

	        for (int i = 0; i < _addFairies.Count; i++)
	        {
                _addFairies[i].Update(gameTime);

	            if (_addFairies[i].Active == false)
	            {
                    _addFairies.RemoveAt(i);
	            }
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
		    rectangle1 = new Rectangle((int) _player.Position.X, (int) _player.Position.Y, _player.Width - 100,
		        _player.Height);

			// Player vs enemy
			for (int i = 0; i < _enemies.Count; i++)
			{
			    rectangle2 = new Rectangle((int) _enemies[i].Position.X, (int) _enemies[i].Position.Y,
			        _enemies[i].Width, _enemies[i].Height);

				if (rectangle1.Intersects(rectangle2))
				{
					_player.Health -= _enemies[i].Damage;

					_enemies[i].Health = 0;

					if (_player.Health <= 0)
					{
						_player.Active = false;
					}
				}
			}

			// Player vs Health power up 
			for (int i = 0; i < _healthy.Count; i++)
			{
				// Create the health power up rectangle
			    rectangle2 = new Rectangle((int) _healthy[i].Position.X, (int) _healthy[i].Position.Y,
			        _healthy[i].Width, _healthy[i].Height);

				if (rectangle1.Intersects(rectangle2))
				{
					_player.Health += 50;
					_healthy.RemoveAt(i);
				}
			}

			//Player vs Speed Power
			for (int i = 0; i < _speed.Count; i++)
			{
				// Create slow motion power rectangle
			    rectangle2 = new Rectangle((int) _speed[i].Position.X, (int) _speed[i].Position.Y, _speed[i].Width,
			        _speed[i].Height);

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
			    rectangle2 = new Rectangle((int) _hyperSpace[i].Position.X, (int) _hyperSpace[i].Position.Y,
			        _hyperSpace[i].Width, _hyperSpace[i].Height);

				if (rectangle1.Intersects(rectangle2))
				{
					_hyperSpaceTimer.Stop();
					_hyperSpaceTimer.Start();
					_hyperSpace.RemoveAt(i);
				}
			}

            // Player vs Savior
		    for (int i = 0; i < _saveMe.Count; i++)
		    {
		        rectangle2 = new Rectangle((int) _saveMe[i].Position.X, (int) _saveMe[i].Position.Y,
		            _saveMe[i].Width, _saveMe[i].Height);

		        if (rectangle1.Intersects(rectangle2))
		        {
		            for (int j = 0; j < _enemies.Count; j++)
		            {
		                _enemies[j].Active = false;
                        AddExplosion(_enemies[j].Position);
		                _score += _enemies[i].ScoreValue;
		            }
                    _saveMe.Clear();
		        }
		    }

            // Player vs AddFairy
		    for (int i = 0; i < _addFairies.Count; i++)
		    {
		        rectangle2 = new Rectangle((int) _addFairies[i].Position.X, (int) _addFairies[i].Position.Y,
		            _addFairies[i].Width, _addFairies[i].Height);

		        if (rectangle1.Intersects(rectangle2))
		        {
		            AddFairy();
		        }
		    }

			// Projectile vs Enemy Collision
			for (int i = 0; i < _marshmallows.Count; i++)
			{
				for (int j = 0; j < _enemies.Count; j++)
				{
					// Create the mashmallow rectangle
					rectangle1 = new Rectangle((int)_marshmallows[i].Position.X - _marshmallows[i].Width / 2, (int)_marshmallows[i].Position.Y -
											   _marshmallows[i].Height / 2, _marshmallows[i].Width, _marshmallows[i].Height);

					// Create the enemy rectangle
				    rectangle2 = new Rectangle((int) _enemies[j].Position.X - _enemies[j].Width / 2,
				        (int) _enemies[j].Position.Y - _enemies[j].Height / 2, _enemies[j].Width,
				        _enemies[j].Height);

					// Determine if the two objects collided with each other
					if (rectangle1.Intersects(rectangle2))
					{
						//Enemies take damage and mashmallows disapear
						_enemies[j].Health -= _marshmallows[i].Damage;
						_marshmallows[i].Active = false;
					}
				}
			}
		}

        #endregion

        #region Explosions

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
				if (_explosions[i].Active == false)
				{
					_explosions.RemoveAt(i);
				}
			}
		}

        #endregion

	    #region Helper Methods

	    private bool CheckKey(Keys theKey)
	    {
	        return _keyboardState.IsKeyUp(theKey) &&
	               _oldKeyboardState.IsKeyDown(theKey);
	    }

	    #endregion
    }
}		