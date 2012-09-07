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
        private int _cursorPosition;
        private char _passwordChar;
        private bool _passwordMode;

        /// <summary>
        /// The position of the cursor within the textbox
        /// </summary>
        public int CursorPosition
        {
            get { return _cursorPosition; }
            private set
            {
                _cursorPosition = value;
                RequestRedraw(RedrawRequestReason.ContentChanged);
            }
        }

        /// <summary>
        /// The char as which all characters should be rendered if <code>PasswordMode</code> is true
        /// </summary>
        /// <seealso cref="PasswordMode"/>
        public char PasswordChar
        {
            get { return _passwordChar; }
            set
            {
                _passwordChar = value;
                RequestRedraw(RedrawRequestReason.ContentChanged);
            }
        }

        /// <summary>
        /// True if all characters should be rendered as <code>PasswordChar</code>
        /// </summary>
        /// <seealso cref="PasswordChar"/>
        public bool PasswordMode
        {
            get { return _passwordMode; }
            set
            {
                _passwordMode = value;
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
            this.PasswordChar = '*';
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
                    if (CursorPosition < Text.Length)
                        CursorPosition++;
                    break;

                case ConsoleKey.LeftArrow:
                    if (CursorPosition > 0)
                        CursorPosition--;
                    break;

                case ConsoleKey.Backspace:
                    if(this.Text.Length != 0)
                    {
                        string newStr = null;
                        char c = keyInfo.KeyChar;

                        if (CursorPosition == this.Text.Length)
                            newStr = this.Text.Substring(0, this.Text.Length - 1);
                        else
                        {
                            if (CursorPosition == 0)
                                return true;
                            newStr = Text.Substring(0, CursorPosition - 1) + Text.Substring(CursorPosition);
                        }

                        CursorPosition--;
                        this.Text = newStr;
                    }
                    break;

                default:
                    if(this.Text.Length < Width)
                    {
                        string newStr = null;
                        char c = keyInfo.KeyChar;

                        if (CursorPosition == this.Text.Length)
                            newStr = Text + c;
                        else
                            newStr = Text.Substring(0, CursorPosition) + c + Text.Substring(CursorPosition);

                        CursorPosition++;
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
            string text = null;

            if(!_control.PasswordMode)
                text = _control.Text;
            else
                text = new string(_control.PasswordChar, _control.Text.Length);
            text = text.PadRight(Control.Width);

            base.Draw(Control.ForeColor, Control.BackColor, text);

            if(Control.IsFocused)
            {
                char cc;
                if(_control.CursorPosition < _control.Text.Length)
                {
                    if(_control.PasswordMode)
                        cc = _control.PasswordChar;
                    else
                        cc = _control.Text[_control.CursorPosition];
                }
                else
                {
                    cc = ' ';
                }

                FogConsole.Write(Boundary.Left + _control.CursorPosition, Boundary.Top, cc, Boundary, Control.BackColor, Control.ForeColor);
            }
        }
    }
}
