using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using FoggyConsole.Controls;

namespace FoggyConsole
{
    public interface IFocusManager : IInputHandler
    {
        Control FocusedControl { get; }
        ConsoleKey[] HandledKeys { get; }
    }

    public class FocusManager : IFocusManager
    {
        private static ConsoleKey[] _handledKeys = new [] { ConsoleKey.Tab };

        public Control FocusedControl { get; private set; }
        public ConsoleKey[] HandledKeys { get { return _handledKeys; } }


        public FocusManager(Control startControl)
        {
            FocusedControl = startControl;
        }


        /// <summary>
        /// Handles the key-userinput which is given in <paramref name="keyInfo"/>
        /// </summary>
        /// <returns>true if the keypress was handled, otherwise false</returns>
        /// <param name="keyInfo">The keypress to handle</param>
        public bool HandleKeyInput(ConsoleKeyInfo keyInfo)
        {
            switch (keyInfo.Key)
            {
                case ConsoleKey.Tab:
                    FocusedControl = HandleTab(FocusedControl);
                    break;
                default:
                    throw new ArgumentException("Given key isn't listed in HandledKeys", "keyInfo");
            }
            return true;
        }

        private Control HandleTab(Control control)
        {
            var index = GetIndexOf(control);

            if(control.Container == null) // got the root
            {
                // this has always to be a ContainerControl because
                // the Application-ctor only accepts ContainerControls as root
                return ((ContainerControl)control)[0];
            }

            if(index == control.Container.Count - 1)
            {
                return HandleTab(control.Container);
            }
            var foundControl = control.Container[index + 1];

            if(foundControl is ContainerControl)
            {
                foundControl = (foundControl as ContainerControl)[0];
            }

            return foundControl;
        }

        private int GetIndexOf(Control control)
        {
            if (control.Container == null)
                return -1;

            for (int i = 0; i < control.Container.Count; i++)
            {
                if (control.Container[i] == control)
                    return i;
            }

            return -1;
        }
    }
}
