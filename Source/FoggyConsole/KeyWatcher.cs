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
