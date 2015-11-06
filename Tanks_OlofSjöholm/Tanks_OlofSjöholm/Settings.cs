using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Tanks_OlofSjöholm
{
    class Settings
    {

        public static SpriteFont FontStandard;
        public static Texture2D TextureScreenBlack;
        public static Texture2D TextureScreenBlue;

        public static Texture2D TextureGreen;
        public static Texture2D TextureRed;

		public static Texture2D TextureTank;
		public static Texture2D TextureAnimationSheet;
        public static Texture2D TextureWoodWall;
        public static Texture2D TextureCracksAnimationSheet;
        public static Texture2D TextureScreenTitle;
		public static Texture2D TextureScreenWhite;
		public static Texture2D TextureParticle;
		public static Texture2D TexturePowerUp;

		public static SoundEffect SoundBackgroundMusic;
		public static SoundEffect SoundExplosion;
		public static SoundEffect SoundMenu;
		public static SoundEffect SoundPowerUp;
		public static SoundEffectInstance SoundFXSBackgroundMusic;
		public static SoundEffectInstance SoundFXSMenu;

		public static int NumberOfXTiles = 35;
		public static int NumberOfYTiles = 20;
		public static float TileScale = 1.3f;
		public static int TileSize = (int)(32 * TileScale);
		public static int ScreenWidth = (int)(NumberOfXTiles * TileSize);
		public static int ScreenHeight = (int)(NumberOfYTiles * TileSize);

		public static Vector2 TankSize = new Vector2(32, 32);
		public static float TankSpeed = 2f;
		public static ChangeableFloat TankSpeed2 = new ChangeableFloat(2f);
		public static double TankTurnSpeed = Math.PI / 180 * 3f;
        public static double TankTurretTurnSpeed = Math.PI / 180 * 3f;
        public static float BulletSpeed = 5f;
        public static int BulletReloadTime = 250;
        public static float Scale = 1f;

		public static int PowerUpSize = 32;

        public static int Player1Score = 0;
        public static int Player2Score = 0;

		public static float TileWallProbability = 0.2f;
        public static float TileDestructableProbability = 0.6f;


        public static void LoadContent(ContentManager cm) {

            //TextureScreenBlack = cm.Load<Texture2D>("Images/black");
            //TextureScreenBlue = cm.Load<Texture2D>("Images/blue");
            TextureGreen = cm.Load<Texture2D>("Images/green");
            TextureRed = cm.Load<Texture2D>("Images/red");
            FontStandard = cm.Load<SpriteFont>("Fonts/standardFont");
			TextureTank = cm.Load<Texture2D>("Images/Tank");
			TextureAnimationSheet = cm.Load<Texture2D>("Images/SpriteSheet_svae");
            TextureWoodWall = cm.Load<Texture2D>("Images/woodwall");
            TextureCracksAnimationSheet = cm.Load<Texture2D>("Images/cracks");
            TextureScreenTitle = cm.Load<Texture2D>("Images/TankTitle");
			//TextureScreenWhite = cm.Load<Texture2D>("Images/white");
			TextureParticle = cm.Load<Texture2D>("Images/particleTexture");
			TexturePowerUp = cm.Load<Texture2D>("Images/powerup");
			SoundExplosion = cm.Load<SoundEffect>("Sound/animals025");
			//SoundBackgroundMusic = cm.Load<SoundEffect>("Sound/01-_Vehicle4");
			//SoundMenu = cm.Load<SoundEffect>("Sound/04-_Slaughter4");
            //SoundPowerUp = cm.Load<SoundEffect>("Sound/powerup");
            //SoundFXSBackgroundMusic = SoundBackgroundMusic.CreateInstance();
            //SoundFXSBackgroundMusic.Volume = .3f;
            //SoundFXSMenu = SoundMenu.CreateInstance();
            //SoundFXSMenu.Volume = .3f;

        }

    }
}
