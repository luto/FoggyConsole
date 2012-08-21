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
        /// The root of the Control-Tree
        /// </summary>
        public ContainerControl RootContainer { get; private set; }
        private Control _focusedControl;


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
        /// This call blocks until the Application has ended.
        /// </summary>
        public void Run()
        {
            while (true)
            {
                RootContainer.Drawer.Draw(0, 0, new Rectangle(0, 0, Console.WindowHeight, Console.WindowWidth));
                var input = GetUserInput();
                if(input.HasValue)
                {
                    HandleUserInput(input.Value);
                }
            }
        }

        private ConsoleKeyInfo? GetUserInput()
        {
            if (Console.KeyAvailable)
                return Console.ReadKey(true);
            return null;
        }

        private void HandleUserInput(ConsoleKeyInfo value)
        {
            throw new NotImplementedException();
        }
    }
}
