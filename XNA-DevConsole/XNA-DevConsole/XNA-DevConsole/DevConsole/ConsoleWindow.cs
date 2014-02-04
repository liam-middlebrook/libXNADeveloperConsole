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
using System.Globalization;
using System.Reflection;

namespace XNA_DevConsole.DevConsole
{
    class ConsoleWindow
    {
        public SpriteFont font;

        public Dictionary<string, IConsoleCommand> commandList;

        private Color fontColor;

        public bool isActive;

        public string lineBuffer;

        public ConsoleWindow()
        {
            commandList = new Dictionary<string, IConsoleCommand>();
            commandList.Add("exit", new ConsoleCommand("exit", (string args) => { Environment.Exit(0); return 0; }));
            commandList.Add("changefontcolor", new ConsoleCommand("changefontcolor", ChangeFontColor));
            lineBuffer = string.Empty;
            fontColor = Color.Black;
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
                    for (int i = 0; i < pressedKeys.Length; i++)
                    {


                        if (keyHelper.KeyState.IsKeyDown(pressedKeys[i]) && !keyHelper.PrevKeyState.IsKeyDown(pressedKeys[i]))
                        {
                            switch (pressedKeys[i])
                            {
                                case Keys.D0:
                                    lineBuffer += "0";
                                    break;
                                case Keys.D1:
                                    lineBuffer += "1";
                                    break;
                                case Keys.D2:
                                    lineBuffer += "2";
                                    break;
                                case Keys.D3:
                                    lineBuffer += "3";
                                    break;
                                case Keys.D4:
                                    lineBuffer += "4";
                                    break;
                                case Keys.D5:
                                    lineBuffer += "5";
                                    break;
                                case Keys.D6:
                                    lineBuffer += "6";
                                    break;
                                case Keys.D7:
                                    lineBuffer += "7";
                                    break;
                                case Keys.D8:
                                    lineBuffer += "8";
                                    break;
                                case Keys.D9:
                                    lineBuffer += "9";
                                    break;
                                case Keys.Space:
                                    {
                                        lineBuffer += " ";
                                        break;
                                    }
                                case Keys.Delete:
                                case Keys.Back:
                                    {
                                        if (lineBuffer.Length > 0)
                                        {
                                            lineBuffer = lineBuffer.Remove(lineBuffer.Length - 1);
                                        }
                                        break;
                                    }
                                default:
                                    {
                                        lineBuffer += (pressedKeys[i].ToString().ToLower() ?? string.Empty);
                                        break;
                                    }
                            }
                        }
                    }
                }
                else
                {
                    string[] line = lineBuffer.Split(' ');
                    string args = string.Empty;
                    args = string.Join(" ", line.Skip(1));
                    try
                    {
                        commandList[line[0]].Function(args);
                    }
                    catch (KeyNotFoundException ex)
                    {

                    }
                    lineBuffer = string.Empty;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(font, lineBuffer, Vector2.Zero, fontColor);
        }

        #region BUILT_IN_COMMANDS

        int ChangeFontColor(string args)
        {
            string[] color = args.Split(' ');
            fontColor = new Color(
                (int)MathHelper.Clamp(int.Parse(color[0]), 0, 255),
                (int)MathHelper.Clamp(int.Parse(color[1]), 0, 255),
                (int)MathHelper.Clamp(int.Parse(color[2]), 0, 255),
                color.Length > 3 ? (int)MathHelper.Clamp(int.Parse(color[3]), 0, 255) : 255
                );
            /*
            var color = typeof(Color).GetProperty(args, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            if (color != null)
            {
                fontColor = (Color)color.GetValue(null, null);
            }
            */
            return 0;
        }

        #endregion
    }
}
