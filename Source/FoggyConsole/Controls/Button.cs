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

namespace FoggyConsole.Controls
{
    /// <summary>
    /// A <code>Control</code> that can be triggered when focused.
    /// The standard look is: <example>[ Button Name ]</example> (drawn by <code>ButtonDrawer</code>).
    /// If Width is zero, the button will use as much space as required.
    /// </summary>
    public class Button : TextualBase, IInputHandler
    {
        /// <summary>
        /// Fired if the button is focused and the user presses the space bar
        /// </summary>
        public event EventHandler Pressed;

        /// <summary>
        /// Creates a new <code>Button</code>
        /// </summary>
        /// <param name="text">The text which is drawn onto the Button.</param>
        /// <param name="drawer">The <code>ControlDrawer</code> to use. If null a new instance of <code>ButtonDrawer</code> will be used.</param>
        /// <exception cref="ArgumentException">Thrown if the <code>ControlDrawer</code> which should be set already has an other Control assigned</exception>
        public Button(string text, ControlDrawer<Button> drawer = null)
            : base(text, drawer)
        {
            if(drawer == null)
                base.Drawer = new ButtonDrawer(this);

            base.IsFocusedChanged += (sender, args) => RequestRedraw(RedrawRequestReason.ContentChanged);
        }

        bool IInputHandler.HandleKeyInput(ConsoleKeyInfo keyInfo)
        {
            if(keyInfo.Key == ConsoleKey.Spacebar)
            {
                OnPressed();
                return true;
            }
            return false;
        }

        private void OnPressed()
        {
            if (Pressed != null)
                Pressed(this, EventArgs.Empty);
        }
    }

    /// <summary>
    /// Draws a <code>Button</code>-Control
    /// </summary>
    public class ButtonDrawer : TextualBaseDrawer<Button>
    {
        public ButtonDrawer(Button control = null)
            : base(control, "[ {0} ]", 4)
        {
        }

        /// <summary>
        /// Draws the <code>Button</code> given in the Control-Property.
        /// </summary>
        /// <exception cref="InvalidOperationException">Is thrown if the Control-Property isn't set.</exception>
        /// <exception cref="InvalidOperationException">Is thrown if the CalculateBoundary-Method hasn't been called.</exception>
        public override void Draw()
        {
            base.Draw();
            base.Draw(Control.IsFocused ? Control.BackColor : Control.ForeColor,
                      Control.IsFocused ? Control.ForeColor : Control.BackColor);
        }
    }
}
