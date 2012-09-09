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
        /// Called if the control-tree has been changed
        /// </summary>
        void ControlTreeChanged();
    }

    /// <summary>
    /// Basic <code>IFocusManager</code> which just cycles through all controls when the user presses TAB
    /// </summary>
    public class FocusManager : IFocusManager
    {
        private Control[] _controls;
        private ContainerControl _rootControl;
        private int _focusedIndex;

        /// <summary>
        /// The currently focused control
        /// </summary>
        public Control FocusedControl { get { return _controls[_focusedIndex]; } }

        /// <summary>
        /// Creates a new FocusManager
        /// </summary>
        /// <param name="rootControl">The control which represents</param>
        /// <param name="startControl"></param>
        /// <exception cref="ArgumentNullException">Is thrown if <paramref name="startControl"/> or <paramref name="rootControl"/> is null</exception>
        /// <exception cref="ArgumentException">Is thrown if <paramref name="startControl"/> has no container</exception>
        /// <exception cref="ArgumentException">Is thrown if <paramref name="startControl"/> is no IInputHandler</exception>
        /// <exception cref="ArgumentException">Is thrown if <paramref name="rootControl"/> has an container</exception>
        /// <exception cref="ArgumentException">Is thrown if <paramref name="startControl"/> is not within <paramref name="rootControl"/></exception>
        public FocusManager(ContainerControl rootControl, Control startControl)
        {
            if (rootControl == null)
                throw new ArgumentNullException("rootControl");
            if (rootControl.Container != null)
                throw new ArgumentException("rootControl has an container!", "rootControl");

            if (startControl == null)
                throw new ArgumentNullException("startControl");
            if (startControl.Container == null)
                throw new ArgumentException("startControl doesn't have an container!", "startControl");
            if (!(startControl is IInputHandler))
                throw new ArgumentException("startControl isn't an IInputHandler!", "startControl");

            _rootControl = rootControl;

            CalculateList();
            _focusedIndex = -1;
            for (int i = 0; i < _controls.Length; i++)
            {
                if (_controls[i] == startControl)
                {
                    _focusedIndex = i;
                    _controls[i].IsFocused = true;
                }
                else
                {
                    _controls[i].IsFocused = false;
                }
            }

            if(_focusedIndex == -1)
                throw new ArgumentException("startControl is not within rootControl", "startControl");
        }

        /// <summary>
        /// Called if the control-tree has been changed
        /// </summary>
        public void ControlTreeChanged()
        {
            CalculateList();
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
                    if (_focusedIndex == _controls.Length - 1)
                        SetFocusedIndex(0);
                    else
                        SetFocusedIndex(_focusedIndex + 1);
                    return true;

                case ConsoleKey.LeftArrow:
                case ConsoleKey.RightArrow:
                case ConsoleKey.UpArrow:
                case ConsoleKey.DownArrow:
                    var up = keyInfo.Key == ConsoleKey.UpArrow;
                    var down = keyInfo.Key == ConsoleKey.DownArrow;
                    var left = keyInfo.Key == ConsoleKey.LeftArrow;
                    var right = keyInfo.Key == ConsoleKey.RightArrow;

                    var upDown = up || down;
                    var leftRight = left || right;

                    var controls = GetNearbyControls(FocusedControl,
                                                     checkTop: leftRight,
                                                     checkLeft: upDown)
                                     .OrderBy(i => upDown ? _controls[i].Drawer.Boundary.Top : _controls[i].Drawer.Boundary.Left * -1).ToArray();

                    if (controls.Length == 0)
                        return true;

                    for (int i = 0; i < controls.Length; i++)
                    {
                        if(controls[i] == _focusedIndex)
                        {
                            if ((up || right) && i != 0)
                                SetFocusedIndex(controls[i - 1]);
                            if ((down || left) && i != controls.Length - 1)
                                SetFocusedIndex(controls[i + 1]);
                            break;
                        }
                    }
                    return true;
            }
            return false;
        }

        private void SetFocusedIndex(int index)
        {
            FocusedControl.IsFocused = false;
            _focusedIndex = index;
            FocusedControl.IsFocused = true;
        }

        /// <summary>
        /// Searches all controlls which are nearby <paramref name="c"/>.
        /// Controls in the same row are searched if <paramref name="checkTop"/> is <code>true</code>.
        /// Controls in the same collumn are searched if <paramref name="checkLeft"/> is <code>true</code>.
        /// </summary>
        /// <param name="c"></param>
        /// <param name="checkTop"></param>
        /// <param name="checkLeft"></param>
        /// <returns>A list of controls nearby <paramref name="c"/></returns>
        private IEnumerable<int> GetNearbyControls(Control c, bool checkTop = false, bool checkLeft = false)
        {
            if(c.Drawer == null)
                yield break;

            for (int i = 0; i < _controls.Length; i++)
            {
                if(_controls[i].Drawer == null)
                    continue;
                if (c.Drawer.Boundary.Top == _controls[i].Drawer.Boundary.Top && checkTop ||
                    c.Drawer.Boundary.Left == _controls[i].Drawer.Boundary.Left && checkLeft)
                    yield return i;
            }
        }

        /// <summary>
        /// Recalculates <code>FocusManager._controls</code>
        /// </summary>
        private void CalculateList()
        {
            _controls = CalculateList(_rootControl).ToArray();
        }

        /// <summary>
        /// Gets all <code>IInputHandler</code> controls within <paramref name="control"/> ordered by thier TabIndex
        /// </summary>
        /// <param name="control">The control to search in</param>
        /// <returns>A list of controls</returns>
        private IEnumerable<Control> CalculateList(ContainerControl control)
        {
            foreach (var c in control.OrderBy(c => c.TabIndex))
            {
                if (c is IInputHandler)
                    yield return c;
                if (c is ContainerControl)
                    foreach (var cc in CalculateList(c as ContainerControl).Where(cc => cc is IInputHandler))
                        yield return cc;
            }
        }
    }
}
