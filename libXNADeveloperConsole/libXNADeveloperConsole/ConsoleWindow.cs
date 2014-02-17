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

namespace libXNADeveloperConsole
{
    /// <summary>
    /// A virtual terminal class for XNA that will act as an in-game developer console
    /// </summary>
    public class ConsoleWindow
    {
        #region SINGLETON_ATTRIBUTES

        private static ConsoleWindow _instance;

        /// <summary>
        /// Gets the instance of ConsoleWindow
        /// </summary>
        /// <returns>The instance of ConsoleWindow</returns>
        public static ConsoleWindow GetInstance()
        {
            if (_instance == null)
            {
                _instance = new ConsoleWindow();
            }
            return _instance;
        }

        #endregion

        #region Fields

        private SpriteFont font;

        private Dictionary<string, IConsoleCommand> commandList;

        private Color fontColor;

        private bool isActive;

        private string lineBuffer;

        private string[] lineHistory;

        private int lineHistoryIndex;

        private bool lineDelay;

        private LimitedMessageQueue loggingQueue;

        #endregion

        #region Properties

        /// <summary>
        /// Returns whether or not the Console Window is active (if the ~ key was pressed)
        /// </summary>
        public bool IsActive { get { return isActive; } }

        /// <summary>
        /// Gets and Sets the font of the ConsoleWindow.
        /// <remarks>If the value to set to is null nothing is changed.</remarks>
        /// </summary>
        public SpriteFont ConsoleFont
        {
            get { return font; }
            set { font = value ?? font; }
        }

        #endregion


        private ConsoleWindow()
        {
            loggingQueue = new LimitedMessageQueue(5);
            commandList = new Dictionary<string, IConsoleCommand>();

            lineBuffer = string.Empty;
            fontColor = Color.White;
            lineHistory = new string[5];
            lineHistoryIndex = -1;
            lineDelay = false;

            AddDefaultCommands();
        }

        /// <summary>
        /// Handles keyboard input to update the state of the console window
        /// </summary>
        /// <param name="keyHelper">A keyboard handler class that stores the current and previous keyboard states</param>
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
                                case Keys.Apps:
                                case Keys.Attn:
                                case Keys.BrowserBack:
                                case Keys.BrowserFavorites:
                                case Keys.BrowserForward:
                                case Keys.BrowserHome:
                                case Keys.BrowserRefresh:
                                case Keys.BrowserSearch:
                                case Keys.BrowserStop:
                                case Keys.ChatPadGreen:
                                case Keys.ChatPadOrange:
                                case Keys.Crsel:
                                case Keys.End:
                                case Keys.EraseEof:
                                case Keys.Escape:
                                case Keys.Execute:
                                case Keys.Exsel:
                                case Keys.F1:
                                case Keys.F10:
                                case Keys.F11:
                                case Keys.F12:
                                case Keys.F13:
                                case Keys.F14:
                                case Keys.F15:
                                case Keys.F16:
                                case Keys.F17:
                                case Keys.F18:
                                case Keys.F19:
                                case Keys.F2:
                                case Keys.F20:
                                case Keys.F21:
                                case Keys.F22:
                                case Keys.F23:
                                case Keys.F24:
                                case Keys.F3:
                                case Keys.F4:
                                case Keys.F5:
                                case Keys.F6:
                                case Keys.F7:
                                case Keys.F8:
                                case Keys.F9:
                                case Keys.Help:
                                case Keys.Home:
                                case Keys.ImeConvert:
                                case Keys.ImeNoConvert:
                                case Keys.Insert:
                                case Keys.Kana:
                                case Keys.Kanji:
                                case Keys.LaunchApplication1:
                                case Keys.LaunchApplication2:
                                case Keys.LaunchMail:
                                case Keys.Left:
                                case Keys.LeftAlt:
                                case Keys.LeftControl:
                                case Keys.LeftShift:
                                case Keys.LeftWindows:
                                case Keys.MediaNextTrack:
                                case Keys.MediaPlayPause:
                                case Keys.MediaPreviousTrack:
                                case Keys.MediaStop:
                                case Keys.None:
                                case Keys.NumLock:
                                case Keys.Oem8:
                                case Keys.OemAuto:
                                case Keys.OemClear:
                                case Keys.OemCopy:
                                case Keys.OemEnlW:
                                case Keys.OemTilde:
                                case Keys.Pa1:
                                case Keys.PageDown:
                                case Keys.PageUp:
                                case Keys.Pause:
                                case Keys.Play:
                                case Keys.Print:
                                case Keys.PrintScreen:
                                case Keys.ProcessKey:
                                case Keys.Right:
                                case Keys.RightAlt:
                                case Keys.RightControl:
                                case Keys.RightShift:
                                case Keys.RightWindows:
                                case Keys.Scroll:
                                case Keys.Select:
                                case Keys.SelectMedia:
                                case Keys.Separator:
                                case Keys.Sleep:
                                case Keys.VolumeDown:
                                case Keys.VolumeMute:
                                case Keys.VolumeUp:
                                case Keys.Zoom:
                                    break;

                                case Keys.Tab:
                                    {
                                        #region Tab_Completion

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

                                        #endregion
                                    }
                                case Keys.OemOpenBrackets:
                                    {
                                        lineBuffer += "[";
                                        break;
                                    }
                                case Keys.OemCloseBrackets:
                                    {
                                        lineBuffer += "]";
                                        break;
                                    }
                                case Keys.OemPipe:
                                    {
                                        lineBuffer += "\\";
                                        break;
                                    }
                                case Keys.OemPlus:
                                case Keys.Add:
                                    {
                                        lineBuffer += "+";
                                        break;
                                    }
                                case Keys.OemMinus:
                                case Keys.Subtract:
                                    {
                                        lineBuffer += "-";
                                        break;
                                    }
                                case Keys.Multiply:
                                    {
                                        lineBuffer += "*";
                                        break;
                                    }
                                case Keys.Divide:
                                    {
                                        lineBuffer += "/";
                                        break;
                                    }
                                case Keys.Decimal:
                                case Keys.OemPeriod:
                                    lineBuffer += ".";
                                    break;
                                case Keys.OemComma:
                                    lineBuffer += ",";
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
                                case Keys.NumPad0:
                                    lineBuffer += "0";
                                    break;
                                case Keys.NumPad1:
                                    lineBuffer += "1";
                                    break;
                                case Keys.NumPad2:
                                    lineBuffer += "2";
                                    break;
                                case Keys.NumPad3:
                                    lineBuffer += "3";
                                    break;
                                case Keys.NumPad4:
                                    lineBuffer += "4";
                                    break;
                                case Keys.NumPad5:
                                    lineBuffer += "5";
                                    break;
                                case Keys.NumPad6:
                                    lineBuffer += "6";
                                    break;
                                case Keys.NumPad7:
                                    lineBuffer += "7";
                                    break;
                                case Keys.NumPad8:
                                    lineBuffer += "8";
                                    break;
                                case Keys.NumPad9:
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
                                        if (lineHistoryIndex > 0)
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

        /// <summary>
        /// Draws the console window to the screen
        /// </summary>
        /// <param name="spriteBatch">The spriteBatch object to draw the console to the screen with.</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            if (isActive)
            {
                spriteBatch.DrawString(font, "XNA Dev-Console\n >| " + lineBuffer + "\n" + loggingQueue, Vector2.Zero, fontColor);
            }
        }

        /// <summary>
        /// Adds a new Command to the list of console commands
        /// </summary>
        /// <param name="consoleCommand">The Command to add to the list</param>
        public void AddCommand(IConsoleCommand consoleCommand)
        {
            commandList.Add(consoleCommand.Name, consoleCommand);
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
                logQueue.Enqueue("Error ChangeFontColor is the following format:\n\t"
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

        private void AddDefaultCommands()
        {
            commandList.Add(
                "help",
                new ConsoleCommand(
                    "help",
                    (string args, LimitedMessageQueue logQueue) =>
                    {
                        logQueue.Capacity = commandList.Keys.Count + 1;
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
        }

        #endregion
    }
}
