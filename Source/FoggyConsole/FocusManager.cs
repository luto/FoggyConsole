using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using FoggyConsole.Controls;

namespace FoggyConsole
{
    /// <summary>
    /// Represents a class which can handle focus changed. The standard implementation is <code>FocusManager</code>
    /// </summary>
    public interface IFocusManager : IInputHandler
    {
        /// <summary>
        /// The currently focused control
        /// </summary>
        Control FocusedControl { get; }
        /// <summary>
        /// Keys which can be handled by this <code>IFocusManager</code>
        /// </summary>
        ConsoleKey[] HandledKeys { get; }
    }

    /// <summary>
    /// Basic <code>IFocusManager</code> which just cycles through all controls when the user presses TAB
    /// </summary>
    public class FocusManager : IFocusManager
    {
        private static readonly ConsoleKey[] _handledKeys = new [] { ConsoleKey.Tab };

        /// <summary>
        /// The currently focused control
        /// </summary>
        public Control FocusedControl { get; private set; }
        /// <summary>
        /// Keys which can be handled by this <code>FocusManager</code>
        /// </summary>
        public ConsoleKey[] HandledKeys { get { return _handledKeys; } }

        /// <summary>
        /// Creates a new FocusManager
        /// </summary>
        /// <param name="startControl"></param>
        /// <exception cref="ArgumentException">Is thrown if <paramref name="startControl"/> has no container</exception>
        /// <exception cref="ArgumentException">Is thrown if <paramref name="startControl"/> is no IInputHandler</exception>
        public FocusManager(Control startControl)
        {
            if (startControl.Container == null)
                throw new ArgumentException("startControl doesn't have an container!", "startControl");
            if (!(startControl is IInputHandler))
                throw new ArgumentException("startControl isn't an IInputHandler!", "startControl");
            FocusedControl = startControl;
            FocusedControl.IsFocused = true;
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
                    FocusedControl.IsFocused = false;
                    FocusedControl = HandleTab(FocusedControl);
                    FocusedControl.IsFocused = true;
                    break;
                default:
                    throw new ArgumentException("Given key isn't listed in HandledKeys", "keyInfo");
            }
            return true;
        }

        private Control HandleTab(Control control)
        {
            // TODO: use TabIndex
            
            var index = GetIndexOf(control);

            if(control.Container == null) // got the root
            {
                // this has always to be a ContainerControl because
                // the Application-ctor only accepts ContainerControls as root
                return ((ContainerControl)control)[0];
            }

            if (index == control.Container.Count - 1)
            {
                return HandleTab(control.Container);
            }
            var foundControl = control.Container[index + 1];

            if(foundControl is ContainerControl && (foundControl as ContainerControl).Count > 0)
            {
                foundControl = (foundControl as ContainerControl)[0];
            }

            // this *should* go well because it isn't possible to create an FocusManager
            // if there are no IInputHandler-Controls in the Application
            while (foundControl is ContainerControl || !(foundControl is IInputHandler))
                foundControl = HandleTab(foundControl);

            return foundControl;
        }

        private int GetIndexOf(Control control)
        {
            if (control.Container == null)
                return -1;

            var controls = control.Container.ToArray();

            for (int i = 0; i < controls.Length; i++)
            {
                if (controls[i] == control)
                    return i;
            }

            return -1;
        }
    }
}
