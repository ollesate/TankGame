using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Nugetta;
using Tanks_OlofSjöholm;

    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game, IOHandler
    {

        //For UI
        private GraphicsDeviceManager Graphics;
        private UiLayer UiLayer;

        public bool IsRunningSlowly;
        //



        SpriteBatch spriteBatch;
        public enum State { TITLE, PLAY, PAUSE, END, EXIT, OPTIONS, CREATEGAME, VIEWGAMES };
        public enum GameCommands { NOTHING, RESETGAME};
        public static GameCommands Execute;
        static public State GameState;
		public static Random Random;
        private StateManager stateManager;
		private MenuManager menuManager;
		private TanksGame game;
        private MenuScreen TitleScreen;
        private MenuScreen PauseScreen;
		private MenuScreen OptionScreen;
        private MenuScreen EndScreen;
        private MenuScreen ViewGamesScreen;
        private Controller controller;

		public Game1() {
			Graphics = new GraphicsDeviceManager(this);


            Graphics.PreferMultiSampling = true;

			Content.RootDirectory = "Content";
            Graphics.PreferredBackBufferWidth = Settings.ScreenWidth;
            Graphics.PreferredBackBufferHeight = Settings.ScreenHeight;
			GameState = State.TITLE;
			IsMouseVisible = true;
			Random = new Random();
            initializeNugetta();
		}

        private void initializeNugetta() {
            NugettaHandler nugettaHandler = MyNugettaHandler.getInstance();
            nugettaHandler.setIo(this);
            nugettaHandler.run();
        }

        public void log(String info) {
            Console.WriteLine(info);
        }

        public void connected() {
            Console.WriteLine("We have been connected to nugetta");
        }


		protected override void Initialize() {
			// TODO: Add your initialization logic here


            _G.Game = this;

            // add core components
            Components.Add(new GamerServicesComponent(this));

            // add layers
            UiLayer = new UiLayer();
            _G.UI = UiLayer;

            // add other components
            _G.GameInput = new GameInput((int)E_GameButton.Count, (int)E_GameAxis.Count);
            GameControls.Setup(); // initialise mappings

			//Highscore
			LoadHighscore();

			//State Actions
			stateManager = new StateManager();
			stateManager.InitializeAction(State.TITLE, Keys.Escape, State.EXIT);

			stateManager.InitializeAction(State.PLAY, Keys.Escape, State.PAUSE);
            stateManager.InitializeAction(State.PAUSE, Keys.Escape, State.PLAY);
			stateManager.InitializeAction(State.OPTIONS, Keys.Escape, State.TITLE);
            stateManager.InitializeAction(State.VIEWGAMES, Keys.Escape, State.TITLE);
            stateManager.InitializeAction(State.END, Keys.Escape, State.EXIT);
            stateManager.InitializeAction(State.END, Keys.Enter, State.PLAY);

            //Menu Actions
            menuManager = new MenuManager();
                //Title
            TitleScreen = new MenuScreen(GraphicsDevice.Viewport.Bounds, Game1.State.TITLE);
            TitleScreen.FontColor = Color.Yellow;
            TitleScreen.AddStateMenuItem("Play", State.PLAY);
            TitleScreen.AddStateMenuItem("Create Game", State.PLAY);
            TitleScreen.AddStateMenuItem("Join Game", State.VIEWGAMES);
			TitleScreen.AddStateMenuItem("Options", State.OPTIONS);
            TitleScreen.AddStateMenuItem("Exit", State.EXIT);
                //Pause
            PauseScreen = new MenuScreen(GraphicsDevice.Viewport.Bounds, Game1.State.PAUSE);
            PauseScreen.FontColor = Color.Red;
            PauseScreen.AddTitel("PAUSED");
            PauseScreen.AddStateMenuItem("Resume", State.PLAY);
            PauseScreen.AddStateMenuItem("Menu", State.TITLE);
            PauseScreen.AddStateMenuItem("Exit", State.EXIT);
				//Options
			OptionScreen = new MenuScreen(GraphicsDevice.Viewport.Bounds, Game1.State.OPTIONS);
			OptionScreen.FontColor = Color.Red;
			OptionScreen.AddTitel("OPTIONS");
			OptionScreen.AddSettingMenuItem("Tank speed", new ChangeableFloat(Settings.TankSpeed));
                //End
            EndScreen = new MenuScreen(GraphicsDevice.Viewport.Bounds, Game1.State.END);
            EndScreen.FontColor = Color.Red;
            EndScreen.XAlignment = 50;
            EndScreen.AddTitel("PAUSED");
                //View Games
            ViewGamesScreen = new ViewGamesGameScreen(GraphicsDevice.Viewport.Bounds, Game1.State.VIEWGAMES);
            ViewGamesScreen.FontColor = Color.Yellow;
            ViewGamesScreen.AddTitel("Current Games");

                    //Insert
            menuManager.AddMenuScreen(TitleScreen);
            menuManager.AddMenuScreen(PauseScreen);
            menuManager.AddMenuScreen(OptionScreen);
            menuManager.AddMenuScreen(EndScreen);
            menuManager.AddMenuScreen(ViewGamesScreen);
            

            //base.Initialize();
            LoadContent();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent() {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            Settings.LoadContent(Content);
			//Settings.SoundMenu.Play();

            //Guide.NotificationPosition = NotificationPosition.BottomRight;

            //Start TankGame Controller
            MyController.StartUp(game);
            controller = MyController.getInstance();

            // startup ui
            UiLayer.Startup(Content);

            // setup debug menu
            #if !RELEASE
                _UI.SetupDebugMenu(null);
            #endif

            base.LoadContent();
        }

        protected override void UnloadContent() {
            // shutdown ui
            UiLayer.Shutdown();

            base.UnloadContent();
        }


        protected override void Update(GameTime gameTime) {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            IsRunningSlowly = gameTime.IsRunningSlowly;

            float frameTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // update input
            _G.GameInput.Update(frameTime);

            UiLayer.Update(frameTime);

            //KeyClass
            KeyMouseReader.Update();

            controller.Update(gameTime);

            ////State Logic
            //GameState = stateManager.GetState(GameState);
            ////Menu Logic
            //GameState = menuManager.GetState(GameState);

            //EndScreen.Title = "Player1: " + Settings.Player1Score + " Player2: " + Settings.Player2Score;

            //switch (GameState) {
            //    case State.TITLE:
            //        Settings.SoundFXSBackgroundMusic.Stop();
            //        Settings.SoundFXSMenu.Play();
            //        startGame();
            //        break;
            //    case State.PLAY:
            //        Settings.SoundFXSMenu.Stop();
            //        Settings.SoundFXSBackgroundMusic.Play();
            //        game.Update(gameTime);
            //        break;
            //    case State.PAUSE:
                    
            //        break;
            //    case State.END:
            //        break;
            //    case State.EXIT:
            //        SaveHighscore();
            //        this.Exit();
            //        break;
            //    default:
            //        break;
            //}

            //switch (Execute) {
            //    case GameCommands.NOTHING:
            //        break;
            //    case GameCommands.RESETGAME:
            //        GameState = State.END;
            //        startGame();
            //        Execute = GameCommands.NOTHING;
            //        break;
            //    default:
            //        break;
            //}

            // TODO: Add your update logic here

            

            //base.Update(gameTime);
        }

		private void SaveHighscore() {

            //string[] lines = { Settings.Player1Score.ToString(), Settings.Player2Score.ToString() };
            //System.IO.File.WriteAllLines(@"C:\WriteLines.txt", lines);

		}

		private void LoadHighscore() {

            //string[] lines = System.IO.File.ReadAllLines(@"C:\WriteLines.txt");
            //Settings.Player1Score = Convert.ToInt16(lines.ElementAt(0));
            //Settings.Player1Score = Convert.ToInt16(lines.ElementAt(1));

		}

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.Black);

            float frameTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // TODO - other stuff here ...

            // render ui


            spriteBatch.Begin();

            controller.Draw(spriteBatch);

            //switch (GameState) {
            //    case State.TITLE:
            //        spriteBatch.Draw(Settings.TextureScreenGreen, GraphicsDevice.Viewport.Bounds, Color.White);
            //        spriteBatch.Draw(Settings.TextureScreenTitle, GraphicsDevice.Viewport.Bounds, Color.White);
            //        TitleScreen.Draw(spriteBatch);
            //        break;
            //    case State.PLAY:
            //        game.Draw(gameTime, spriteBatch);
            //        break;
            //    case State.PAUSE:
            //        game.Draw(gameTime, spriteBatch);
            //        spriteBatch.Draw(Settings.TextureScreenWhite, GraphicsDevice.Viewport.Bounds, new Color(Color.Gray.ToVector4() * .5f));
            //        PauseScreen.Draw(spriteBatch);
            //        break;
            //    case State.END:
            //        game.Draw(gameTime, spriteBatch);
            //        spriteBatch.Draw(Settings.TextureScreenWhite, GraphicsDevice.Viewport.Bounds, new Color(Color.Gray.ToVector4() * .5f));
            //        EndScreen.Draw(spriteBatch);
            //        break;
            //    case State.OPTIONS:
            //        OptionScreen.Draw(spriteBatch);
            //        break;
            //    default:
            //        break;
            //}


            spriteBatch.End();        

            UiLayer.Render(frameTime);


            base.Draw(gameTime);
        }
    }
