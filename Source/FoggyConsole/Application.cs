using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using FoggyConsole.Controls;

namespace FoggyConsole
{
    /// <summary>
    /// The actual Application.
    /// It contains a <code>RootControl</code> in which all other <code>Control</code> instances are stored in a tree-format.
    /// It also manages userinput and drawing.
    /// </summary>
    public class Application
    {
        /// <summary>
        /// Enables some debugging options, such as drawing panels with background and displaying pressed keys
        /// </summary>
        public static bool DEBUG_MODE = true;
        
        private bool _running = false;
        private FocusManager _focusManager;

        /// <summary>
        /// The root of the Control-Tree
        /// </summary>
        public ContainerControl RootContainer { get; private set; }
        /// <summary>
        /// Responsible for focus-changes, for example when the user presses the TAB-key
        /// </summary>
        public FocusManager FocusManager
        {
            get { return _focusManager; }
            set
            {
                if(_running)
                    throw new InvalidOperationException("The FocusManager can't be changed once the Application has been started.");
                _focusManager = value;
            }
        }


        /// <summary>
        /// Creates a new Application
        /// </summary>
        /// <param name="rootContainer">A <code>ContainerControl</code> which is at the root of the </param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="rootContainer"/> is null.</exception>
        /// <exception cref="ArgumentException">Thrown when the Container-Property of <paramref name="rootContainer"/> is set.</exception>
        public Application(ContainerControl rootContainer)
        {
            if(rootContainer == null)
                throw new ArgumentNullException("rootContainer");
            if(rootContainer.Container != null)
                throw new ArgumentException("The root-container can't have the Container-Property set.", "rootContainer");

            RootContainer = rootContainer;
        }


        /// <summary>
        /// Starts this <code>Application</code>.
        /// </summary>
        public void Run()
        {
            _running = true;
            Redraw();
            KeyWatcher.KeyPressed += KeyWatcherOnKeyPressed;
        }

        private void Redraw()
        {
            var mainBoundary = new Rectangle(0, 0, Console.WindowHeight, Console.WindowWidth);
            RootContainer.Drawer.Draw(0, 0, mainBoundary);
        }

        private void KeyWatcherOnKeyPressed(object sender, KeyPressedEventArgs eventArgs)
        {
            if(DEBUG_MODE)
                FogConsole.Write(0, Console.WindowHeight - 1, "Key pressed: " + eventArgs.KeyInfo.Key.ToString().PadRight(10), null, ConsoleColor.DarkGray);
            bool handled = false;

            if(FocusManager.FocusedControl is IInputHandler)
            {
                handled = (FocusManager.FocusedControl as IInputHandler).HandleKeyInput(eventArgs.KeyInfo);
            }

            if(!handled && FocusManager != null && FocusManager.HandledKeys.Contains(eventArgs.KeyInfo.Key))
            {
                FocusManager.HandleKeyInput(eventArgs.KeyInfo);
                if(DEBUG_MODE)
                    FogConsole.Write(0, Console.WindowHeight - 2, "Focused: " + FocusManager.FocusedControl.Name.PadRight(20), null, ConsoleColor.DarkGray);
            }

            Redraw();
        }
    }
}
