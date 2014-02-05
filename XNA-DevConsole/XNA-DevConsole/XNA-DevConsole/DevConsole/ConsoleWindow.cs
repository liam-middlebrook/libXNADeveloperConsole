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

        private string[] lineHistory;

        private int lineHistoryIndex;

        private bool lineDelay;

        LimitedMessageQueue loggingQueue;

        public ConsoleWindow()
        {
            loggingQueue = new LimitedMessageQueue(5);
            commandList = new Dictionary<string, IConsoleCommand>();
            commandList.Add(
                "help",
                new ConsoleCommand(
                    "help",
                    (string args, LimitedMessageQueue logQueue) =>
                    {
                        logQueue.DataLimit = commandList.Keys.Count + 1;
                        foreach (string command in commandList.Keys)
                        {
                            logQueue.Enqueue(command);
                        }
                        logQueue.Enqueue("List of Valid Commands:");
                        return 0;
                    }));

            commandList.Add("exit", new ConsoleCommand("exit", (string args, LimitedMessageQueue logQueue) => { Environment.Exit(0); return 0; }));
            commandList.Add("echo", new ConsoleCommand("echo", (string args, LimitedMessageQueue logQueue) => { logQueue.Enqueue(args); return 0; }));
            commandList.Add("clear", new ConsoleCommand("clear", (string args, LimitedMessageQueue logQueue) => { logQueue.Clear(); return 0; }));
            commandList.Add("changefontcolor", new ConsoleCommand("changefontcolor", ChangeFontColor));
            lineBuffer = string.Empty;
            fontColor = Color.White;
            lineHistory = new string[5];
            lineHistoryIndex = -1;
            lineDelay = false;

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
                                case Keys.Tab:
                                    {
                                        int keyIndex = -1;
                                        List<string> commands = commandList.Keys.ToList();
                                        for (int j = 0; j < commands.Count; j++)
                                        {
                                            if (commands[j].StartsWith(lineBuffer))
                                            {
                                                keyIndex = j;
                                            }
                                        }
                                        if (keyIndex != -1)
                                        {
                                            lineBuffer = commands[keyIndex];
                                        }
                                        break;
                                    }
                                case Keys.OemPeriod:
                                    lineBuffer += ".";
                                    break;
                                case Keys.OemComma:
                                    lineBuffer += ",";
                                    break;
                                case Keys.LeftAlt:
                                case Keys.LeftControl:
                                case Keys.LeftShift:
                                case Keys.LeftWindows:
                                case Keys.RightAlt:
                                case Keys.RightControl:
                                case Keys.RightShift:
                                case Keys.RightWindows:
                                case Keys.CapsLock:
                                case Keys.OemTilde:
                                    break;
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
                                case Keys.Up:
                                    {
                                        if (lineHistoryIndex < lineHistory.Length - 1)
                                        {
                                            ++lineHistoryIndex;
                                            lineBuffer = lineHistory[lineHistoryIndex];
                                        }
                                        break;
                                    }
                                case Keys.Down:
                                    {
                                        if (lineHistoryIndex >= 0)
                                        {
                                            --lineHistoryIndex;
                                            lineBuffer = lineHistory[lineHistoryIndex];
                                        }
                                        else
                                        {
                                            lineBuffer = string.Empty;
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
                    lineDelay = false;
                }
                else if (!lineDelay)
                {
                    lineDelay = true;
                    for (int i = lineHistory.Length - 1; i > 0; i--)
                    {
                        lineHistory[i] = lineHistory[i - 1];
                    }
                    lineHistory[0] = lineBuffer;

                    string[] line = lineBuffer.Split(' ');
                    string args = string.Empty;
                    args = string.Join(" ", line.Skip(1));
                    if (commandList.ContainsKey(line[0]))
                    {
                        commandList[line[0]].Function(args, loggingQueue);
                    }
                    else
                    {
                        loggingQueue.Enqueue("Error: " + line[0] + " is NOT a valid command! Type Help for a list of valid commands!");
                    }

                    lineBuffer = string.Empty;
                    keyHelper.UpdateKeyStates();
                    lineHistoryIndex = -1;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (isActive)
            {
                spriteBatch.DrawString(font, "XNA Dev-Console\n >| " + lineBuffer + "\n" + loggingQueue, Vector2.Zero, fontColor);
            }
        }

        #region BUILT_IN_COMMANDS

        int ChangeFontColor(string args, LimitedMessageQueue logQueue)
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
                logQueue.Enqueue("Error ChangeFontColor is the following format:\n"
                    + "changefontcolor <R 0-255> <G 0-255> <B 0-255> (A 0-255)");
                return -1;
            }

            fontColor = new Color(
                (int)MathHelper.Clamp(int.Parse(color[0]), 0, 255),
                (int)MathHelper.Clamp(int.Parse(color[1]), 0, 255),
                (int)MathHelper.Clamp(int.Parse(color[2]), 0, 255),
                color.Length > 3 ? (int)MathHelper.Clamp(int.Parse(color[3]), 0, 255) : 255
                );

            return 0;
        }

        #endregion
    }
}
