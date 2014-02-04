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

namespace XNA_DevConsole.DevConsole
{
    class ConsoleWindow
    {
        public SpriteFont font;

        public Dictionary<string, IConsoleCommand> commandList;

        public bool isActive;

        public string lineBuffer;

        public ConsoleWindow()
        {
            commandList = new Dictionary<string, IConsoleCommand>();
            commandList.Add("exit", new ConsoleCommand("exit", (string args) => { Environment.Exit(0); return 0; }));
            lineBuffer = string.Empty;
        }

        public void Update(KeyboardHelper keyHelper)
        {
            if (!keyHelper.KeyState.IsKeyDown(Keys.OemTilde) && keyHelper.PrevKeyState.IsKeyDown(Keys.OemTilde))
            {
                isActive = !isActive;
            }

            KeyboardHelper.IsConsoleMode = isActive;

            if (isActive)
            {
                if (!(keyHelper.KeyState.IsKeyDown(Keys.Enter) || keyHelper.PrevKeyState.IsKeyDown(Keys.Enter)))
                {
                    Keys[] pressedKeys = keyHelper.KeyState.GetPressedKeys();
                    if(pressedKeys.Length > 0 && keyHelper.KeyState.IsKeyDown(pressedKeys[0]) && !keyHelper.PrevKeyState.IsKeyDown(pressedKeys[0]))
                    {
                    lineBuffer += (pressedKeys[0].ToString() ?? string.Empty);
                }}
                else
                {
                    string[] line = lineBuffer.Split(' ');
                    string args = string.Empty;
                    for (int i = 1; i < line.Length; i++)
                    {
                        args += line[i];
                    }
                    try
                    {
                        commandList[line[0].ToLower()].Function(args);
                    }
                    catch (Exception ex)
                    {
                        
                    }
                    lineBuffer = string.Empty;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(font, lineBuffer, Vector2.Zero, Color.Black);
        }
    }
}
