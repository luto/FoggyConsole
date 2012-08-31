using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        public static bool DEBUG_MODE = false;

        /// <summary>
        /// Used as the boundary for the rootContainer if the terminal-size can't determined
        /// </summary>
        public static Rectangle STANDARD_ROOT_BOUNDARY = new Rectangle(0, 0, 25, 80);
        
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
            if (rootContainer == null)
                throw new ArgumentNullException("rootContainer");
            if (rootContainer.Container != null)
                throw new ArgumentException("The root-container can't have the Container-Property set.", "rootContainer");

            RootContainer = rootContainer;
        }

        /// <summary>
        /// Starts this <code>Application</code>.
        /// </summary>
        public void Run()
        {
            CalculateRootBound();
            Console.Clear();
            RootContainer.Drawer.Draw();

            WireControlEvents(RootContainer);
            KeyWatcher.KeyPressed += KeyWatcherOnKeyPressed;
            RootContainer.ControlAdded += OnControlAdded;
            RootContainer.ControlRemoved += OnControlRemoved;

            _running = true;
        }

        /// <summary>
        /// Stops this <code>Application</code>.
        /// </summary>
        public void Stop()
        {
            RemoveControlEvents(RootContainer);
            KeyWatcher.KeyPressed -= KeyWatcherOnKeyPressed;
            RootContainer.ControlAdded -= OnControlAdded;
            RootContainer.ControlRemoved -= OnControlRemoved;
            _running = false;
        }

        private void CalculateRootBound()
        {
            var rootBound = STANDARD_ROOT_BOUNDARY;
            // Size dedection will work on windows and on most unix-systems
            // mono uses the same values for Window- and Buffer-Properties,
            // so it doesn't matter which values are used.
            if (Console.WindowHeight != 0)
                rootBound = new Rectangle(0, 0, Console.WindowHeight, Console.WindowWidth);
            RootContainer.Drawer.CalculateBoundary(0, 0, rootBound);
        }

        private void OnControlAdded(object sender, ContainerControlEventArgs eventArgs)
        {
            WireControlEvents(eventArgs.Control);
            eventArgs.Control.RedrawRequested += (o, args) => RedrawRequested(o as Control, args);
        }

        private void OnControlRemoved(object sender, ContainerControlEventArgs eventArgs)
        {
            RemoveControlEvents(eventArgs.Control);
        }

        /// <summary>
        /// Subscribes <code>RedrawRequested</code> to <code>Control.RedrawRequested</code> on <paramref name="control"/> and all its child-controls.
        /// Also subscribes <code>OnControlAdded</code> to <code>ContainerControl.ControlAdded</code> if <paramref name="control"/> is an <code>ContainerControl</code>
        /// </summary>
        /// <param name="control">The control to whose events should be subscribed</param>
        /// <seealso cref="RemoveControlEvents"/>
        private void WireControlEvents(Control control)
        {
            control.RedrawRequested += RedrawRequested;

            if (control is ContainerControl)
            {
                var container = control as ContainerControl;
                container.ControlAdded += OnControlAdded;

                foreach (var c in container)
                    WireControlEvents(c);
            }
        }

        /// <summary>
        /// Unsubscribes <code>RedrawRequested</code> from <code>Control.RedrawRequested</code> on <paramref name="control"/> and all its child-controls.
        /// Also unsubscribes <code>OnControlAdded</code> from <code>ContainerControl.ControlAdded</code> if <paramref name="control"/> is an <code>ContainerControl</code>
        /// </summary>
        /// <param name="control">The control from whose events should be unsubscribed</param>
        /// <seealso cref="WireControlEvents"/>
        private void RemoveControlEvents(Control control)
        {
            control.RedrawRequested -= RedrawRequested;

            if (control is ContainerControl)
            {
                var container = control as ContainerControl;
                container.ControlAdded -= OnControlAdded;

                foreach (var c in container)
                    WireControlEvents(c);
            }
        }

        private void RedrawRequested(object sender, RedrawRequestedEventArgs eventArgs)
        {
            var control = (Control)sender;

            if (eventArgs.Reason == RedrawRequestReason.BecameSmaller ||
                eventArgs.Reason == RedrawRequestReason.BecameBigger  ||
                eventArgs.Reason == RedrawRequestReason.Moved)
                CalculateRootBound();

            switch (eventArgs.Reason)
            {
                case RedrawRequestReason.BecameSmaller:
                case RedrawRequestReason.Moved:
                    control.Container.Drawer.Draw();
                    break;
                case RedrawRequestReason.BecameBigger:
                case RedrawRequestReason.ContentChanged:
                    control.Drawer.Draw();
                    break;
            }
        }

        private void KeyWatcherOnKeyPressed(object sender, KeyPressedEventArgs eventArgs)
        {
            if (DEBUG_MODE)
                FogConsole.Write(0, Console.WindowHeight - 1, "Key pressed: " + eventArgs.KeyInfo.Key.ToString().PadRight(10), null, ConsoleColor.DarkGray);
            bool handled = false;

            if (FocusManager.FocusedControl is IInputHandler)
            {
                handled = (FocusManager.FocusedControl as IInputHandler).HandleKeyInput(eventArgs.KeyInfo);
            }

            if (!handled && FocusManager != null && FocusManager.HandledKeys.Contains(eventArgs.KeyInfo.Key))
            {
                FocusManager.HandleKeyInput(eventArgs.KeyInfo);
                if (DEBUG_MODE)
                    FogConsole.Write(0, Console.WindowHeight - 2, "Focused: " + FocusManager.FocusedControl.Name.PadRight(20), null, ConsoleColor.DarkGray);
            }
        }
    }
}
