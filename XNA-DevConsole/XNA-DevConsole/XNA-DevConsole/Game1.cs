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

using libXNADeveloperConsole;

namespace XNA_DevConsole
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        KeyboardHelper keyHelper;

        Color backgroudColor = Color.Black;


        List<GemGameDemo.Gem> gemList;
        Texture2D gemTexture;
        Random rand;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            keyHelper = new KeyboardHelper();
            gemList = new List<GemGameDemo.Gem>();
            rand = new Random();
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

           ConsoleWindow.GetInstance().AddCommand(
                new ConsoleCommand("changebackgroundcolor",
                    (string args, LimitedMessageQueue logQueue) =>
                    {
                        bool formatError = false;

                        formatError = (args == string.Empty);

                        string[] color = args.Split(' ');

                        formatError = formatError
                                        || color.Length < 3
                                        || (color[0] == string.Empty)
                                        || (color[1] == string.Empty)
                                        || (color[2] == string.Empty)
                                        || (color.Length > 3 && color[3] == string.Empty);

                        if (formatError)
                        {
                            logQueue.Enqueue("Error ChangeBackgroundColor is the following format:\n\t"
                                + "changebackgroundcolor <R 0-255> <G 0-255> <B 0-255> (A 0-255)");
                            return -1;
                        }

                        backgroudColor = new Color(
                            (int)MathHelper.Clamp(int.Parse(color[0]), 0, 255),
                            (int)MathHelper.Clamp(int.Parse(color[1]), 0, 255),
                            (int)MathHelper.Clamp(int.Parse(color[2]), 0, 255),
                            color.Length > 3 ? (int)MathHelper.Clamp(int.Parse(color[3]), 0, 255) : 255
                            );

                        return 0;
                    }));

            ConsoleWindow.GetInstance().AddCommand(
                new ConsoleCommand("addgem",
                    (string args, LimitedMessageQueue logQueue) =>
                    {
                        bool formatError = false;

                        formatError = (args == string.Empty);

                        string[] color = args.Split(' ');

                        formatError = formatError
                                        || color.Length < 3
                                        || (color[0] == string.Empty)
                                        || (color[1] == string.Empty)
                                        || (color[2] == string.Empty)
                                        || (color.Length > 3 && color[3] == string.Empty);

                        if (formatError)
                        {
                            logQueue.Enqueue("Error AddGem is the following format:\n\t"
                                + "AddGem <R 0-255> <G 0-255> <B 0-255> (A 0-255)");
                            return -1;
                        }

                        Color gemColor = new Color(
                            (int)MathHelper.Clamp(int.Parse(color[0]), 0, 255),
                            (int)MathHelper.Clamp(int.Parse(color[1]), 0, 255),
                            (int)MathHelper.Clamp(int.Parse(color[2]), 0, 255),
                            color.Length > 3 ? (int)MathHelper.Clamp(int.Parse(color[3]), 0, 255) : 255
                            );

                        gemList.Add(
                            new GemGameDemo.Gem(
                                new Vector2(rand.Next(0, graphics.PreferredBackBufferWidth), rand.Next(0, graphics.PreferredBackBufferHeight)),
                                gemTexture,
                                gemColor
                                ));

                        return 0;
                    }));

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

            ConsoleWindow.GetInstance().ConsoleFont = Content.Load<SpriteFont>("Consolas");
            gemTexture = Content.Load<Texture2D>("gem");

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            keyHelper.UpdateKeyStates();
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            ConsoleWindow.GetInstance().Update(keyHelper);
            System.Threading.Thread.Sleep(10);
            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(backgroudColor);

            // TODO: Add your drawing code here

            spriteBatch.Begin();


            foreach (GemGameDemo.Gem gem in gemList)
            {
                gem.Draw(spriteBatch);
            }

            if (!ConsoleWindow.GetInstance().IsActive)
            {
                spriteBatch.DrawString(ConsoleWindow.GetInstance().ConsoleFont, "Press ~ to open the developer console | Type 'help' for a list of commands.", new Vector2(10, 450), Color.White);
            }


            ConsoleWindow.GetInstance().Draw(spriteBatch);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
