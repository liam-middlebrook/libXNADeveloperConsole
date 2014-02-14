using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace libXNADeveloperConsole
{
    /// <summary>
    /// A KeyboardHelper class for XNA intended for use with libXNADeveloperConsole
    /// </summary>
    public sealed class KeyboardHelper
    {
        /// <summary>
        /// Is the Console Window Active?
        /// </summary>
        public static bool IsConsoleMode = false;

        private KeyboardState keyState;
        private KeyboardState prevKeyState;

        /// <summary>
        /// The keyboard state for the current update cycle.
        /// </summary>
        public KeyboardState KeyState { get { return keyState; } }

        /// <summary>
        /// The keyboard state for the previous update cycle.
        /// </summary>
        public KeyboardState PrevKeyState { get { return prevKeyState; } }

        /// <summary>
        /// Creates a new instance of the KeyboardHelper
        /// </summary>
        public KeyboardHelper()
        {
            keyState = Keyboard.GetState();
            keyState = prevKeyState;
        }

        /// <summary>
        /// Updates the KeyStates for the KeyboardHelper
        /// </summary>
        public void UpdateKeyStates()
        {
            prevKeyState = keyState;

            keyState = Keyboard.GetState();
        }
    }
}
