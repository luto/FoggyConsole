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
    /// A control which provides single line editing and text input
    /// </summary>
    public class Textbox : TextualBase, IInputHandler
    {
        private int _cursorPos;

        /// <summary>
        /// The position of the cursor within the textbox
        /// </summary>
        public int CursorPos
        {
            get { return _cursorPos; }
            private set
            {
                _cursorPos = value;
                RequestRedraw(RedrawRequestReason.ContentChanged);
            }
        }

        /// <summary>
        /// Creates a new textbox
        /// </summary>
        /// <param name="drawer">The <code>ControlDrawer</code> to use. If null a new instance of <code>TextboxDrawer</code> will be used.</param>
        /// <exception cref="ArgumentException">Thrown if the <code>ControlDrawer</code> which should be set already has an other Control assigned</exception>
        public Textbox(IControlDrawer drawer = null)
            : base("", drawer)
        {
            if(drawer == null)
                base.Drawer = new TextboxDrawer(this);
            this.IsFocusedChanged += (sender, args) => RequestRedraw(RedrawRequestReason.ContentChanged);
        }

        /// <summary>
        /// Handles the key-userinput which is given in <paramref name="keyInfo"/>.
        /// <code>ConsoleKey.Escape</code>, <code>ConsoleKey.Enter</code> and <code>ConsoleKey.Tab</code> are ignored.
        /// <code>ConsoleKey.RightArrow</code> and <code>ConsoleKey.RightArrow</code> are used to move the cursor.
        /// <code>ConsoleKey.Backspace</code> is used to delete text. Other keys are used to add text to the textbox.
        /// </summary>
        /// <returns>true if the keypress was handled, otherwise false</returns>
        /// <param name="keyInfo">The keypress to handle</param>
        public bool HandleKeyInput(ConsoleKeyInfo keyInfo)
        {
            switch (keyInfo.Key)
            {
                case ConsoleKey.Escape:
                case ConsoleKey.Enter:
                case ConsoleKey.Tab:
                    return false;

                case ConsoleKey.RightArrow:
                    if (CursorPos < Text.Length)
                        CursorPos++;
                    break;

                case ConsoleKey.LeftArrow:
                    if (CursorPos > 0)
                        CursorPos--;
                    break;

                case ConsoleKey.Backspace:
                    if(this.Text.Length != 0)
                    {
                        string newStr = null;
                        char c = keyInfo.KeyChar;

                        if (CursorPos == this.Text.Length)
                            newStr = this.Text.Substring(0, this.Text.Length - 1);
                        else
                        {
                            if (CursorPos == 0)
                                return true;
                            newStr = Text.Substring(0, CursorPos - 1) + Text.Substring(CursorPos);
                        }

                        CursorPos--;
                        this.Text = newStr;
                    }
                    break;

                default:
                    if(this.Text.Length < Width)
                    {
                        string newStr = null;
                        char c = keyInfo.KeyChar;

                        if (CursorPos == this.Text.Length)
                            newStr = Text + c;
                        else
                            newStr = Text.Substring(0, CursorPos) + c + Text.Substring(CursorPos);

                        CursorPos++;
                        this.Text = newStr;
                    }
                    break;
            }

            return true;
        }
    }

    /// <summary>
    /// Draws a textbox
    /// </summary>
    public class TextboxDrawer : TextualBaseDrawer<Textbox>
    {
        public TextboxDrawer(Textbox control)
            : base(control, "{1}", 0)
        {
        }

        /// <summary>
        /// Draws the textbox given in the Control-Property
        /// </summary>
        public override void Draw()
        {
            base.Draw();
            base.Draw(Control.ForeColor, Control.BackColor, _control.Text.PadRight(Control.Width));
            if(Control.IsFocused)
            {
                char cc = _control.CursorPos < _control.Text.Length ? _control.Text[_control.CursorPos] : ' ';
                FogConsole.Write(Boundary.Left + _control.CursorPos, Boundary.Top, cc, Boundary, Control.BackColor, Control.ForeColor);
            }
        }
    }
}
