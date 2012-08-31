/*
This file is part of FoggyConsole.

FoggyConsole is free software: you can redistribute it and/or modify
it under the terms of the GNU Lesser General Public License as
published by the Free Software Foundation, either version 3 of
the License, or (at your option) any later version.

FoogyConsole is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU Lesser General Public License for more details.

You should have received a copy of the GNU Lesser General Public License
along with FoggyConsole.  If not, see <http://www.gnu.org/licenses/lgpl.html>.
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace FoggyConsole
{
    /// <summary>
    /// Watches out of userinput using <code>Console.KeyAvailable</code> and <code>Code.ReadKey</code>
    /// </summary>
    internal static class KeyWatcher
    {
        private static Thread _watcherThread;
        /// <summary>
        /// Is fired when a user presses an key
        /// </summary>
        public static event EventHandler<KeyPressedEventArgs> KeyPressed;

        static KeyWatcher()
        {
            _watcherThread = new Thread(WatchOut);
            _watcherThread.Name = "KeyWatcher";
            _watcherThread.Start();
        }

        /// <summary>
        /// Stops the internal watcher-thread
        /// </summary>
        public static void Stop()
        {
            _watcherThread.Abort();
        }

        private static void WatchOut()
        {
            while (true)
            {
                if(Console.KeyAvailable)
                {
                    var keyInfo = Console.ReadKey(true);
                    KeyPressed(null, new KeyPressedEventArgs(keyInfo));
                }
                Thread.Sleep(75);
            }
        }
    }

    /// <summary>
    /// Stores information about a pressed key
    /// </summary>
    internal class KeyPressedEventArgs : EventArgs
    {
        /// <summary>
        /// The key which was pressed
        /// </summary>
        public ConsoleKeyInfo KeyInfo { get; private set; }

        /// <summary>
        /// Creates a new <code>KeyPressedEventArgs</code> instance
        /// </summary>
        /// <param name="keyInfo">The key which was pressed</param>
        public KeyPressedEventArgs(ConsoleKeyInfo keyInfo)
            : base()
        {
            KeyInfo = keyInfo;
        }
    }
}
