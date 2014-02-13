using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace libXNADeveloperConsole
{
    public sealed class KeyboardHelper
    {
        public static bool IsConsoleMode = false;

        private KeyboardState keyState;
        private KeyboardState prevKeyState;

        public KeyboardState KeyState { get { return keyState; } }

        public KeyboardState PrevKeyState { get { return prevKeyState; } }

        public KeyboardHelper()
        {
            keyState = Keyboard.GetState();
            keyState = prevKeyState;
        }

        public void UpdateKeyStates()
        {
            prevKeyState = keyState;

            keyState = Keyboard.GetState();
        }
    }
}
