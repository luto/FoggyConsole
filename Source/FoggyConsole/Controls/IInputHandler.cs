using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FoggyConsole.Controls
{
    /// <summary>
    /// Discribes a class which can handle userinput
    /// </summary>
    public interface IInputHandler
    {
        /// <summary>
        /// Handles the key-userinput which is given in <paramref name="keyInfo"/>
        /// </summary>
        /// <returns>true if the keypress was handled, otherwise false</returns>
        /// <param name="keyInfo">The keypress to handle</param>
        bool HandleKeyInput(ConsoleKeyInfo keyInfo);
    }
}
