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
    /// Base class for <code>Checkbox</code> and <code>RadioButton</code>
    /// </summary>
    public abstract class CheckableBase : TextualBase, IInputHandler
    {
        private CheckState _checked;
        /// <summary>
        /// Gets or sets the state of this checkbox
        /// </summary>
        public CheckState Checked
        {
            get { return _checked; }
            set
            {
                var cancel = OnCheckedChanging(value);
                if (!cancel)
                {
                    _checked = value;
                    OnCheckedChanged();
                }
            }
        }

        /// <summary>
        /// Fired if the Checked-Property is going to change
        /// </summary>
        public event EventHandler<CheckedChangingEventArgs> CheckedChanging;

        /// <summary>
        /// Fired if the Checked-Property has been changed
        /// </summary>
        public event EventHandler CheckedChanged;

        protected CheckableBase(string text, IControlDrawer drawer)
            : base(text, drawer)
        {
            base.IsFocusedChanged += (sender, args) => RequestRedraw(RedrawRequestReason.ContentChanged);
        }

        /// <summary>
        /// Fires the CheckedChanging-event and returns true if the process should be canceled
        /// </summary>
        /// <param name="state">The state the checkbox is going to have</param>
        /// <returns>True if the process should be canceled, otherwise false</returns>
        private bool OnCheckedChanging(CheckState state)
        {
            var args = new CheckedChangingEventArgs(state);
            if (CheckedChanging != null)
                CheckedChanging(this, args);
            return args.Cancel;
        }

        /// <summary>
        /// Fires the CheckedChanged-event and requests an redraw
        /// </summary>
        private void OnCheckedChanged()
        {
            RequestRedraw(RedrawRequestReason.ContentChanged);
            if (CheckedChanged != null)
                CheckedChanged(this, EventArgs.Empty);
        }

        /// <summary>
        /// Handles the key-userinput which is given in <paramref name="keyInfo"/>.
        /// The Checked-State will flip if the user presses the spacebar.
        /// </summary>
        /// <returns>true if the keypress was handled, otherwise false</returns>
        /// <param name="keyInfo">The keypress to handle</param>
        public virtual bool HandleKeyInput(ConsoleKeyInfo keyInfo)
        {
            if (keyInfo.Key == ConsoleKey.Spacebar)
            {
                CheckState newState;
                switch (Checked)
                {
                    case CheckState.Checked:
                        newState = CheckState.Unchecked;
                        break;
                    case CheckState.Unchecked:
                    case CheckState.Indeterminate:
                        newState = CheckState.Checked;
                        break;
                    default:
                        // stupid C#-compiler is stupid, will never happen.
                        throw new ArgumentOutOfRangeException();
                }
                Checked = newState;
                return true;
            }
            return false;
        }
    }

    /// <summary>
    /// All possible states for <code>Checkbox</code>
    /// </summary>
    public enum CheckState
    {
        /// <summary>
        /// The checkbox is checked
        /// </summary>
        Checked,
        /// <summary>
        /// The checkbox is unchecked
        /// </summary>
        Unchecked,
        /// <summary>
        /// The checkbox is in an indeterminate state,
        /// can be used to force to user to actively select one state
        /// </summary>
        Indeterminate
    }

    /// <summary>
    /// Contains information about an occuring status-change of a <code>Checkbox</code>
    /// </summary>
    public class CheckedChangingEventArgs : EventArgs
    {
        /// <summary>
        /// The state the Checkbox is going to have
        /// </summary>
        public CheckState State { get; private set; }
        /// <summary>
        /// True if the change should be canceled, otherwise false
        /// </summary>
        public bool Cancel { get; set; }

        /// <summary>
        /// Creates a new <code>CheckboxCheckedChangingEventArg</code>
        /// </summary>
        /// <param name="state">The state the Checkbox is going to have</param>
        public CheckedChangingEventArgs(CheckState state)
        {
            this.State = state;
            this.Cancel = false;
        }
    }

    /// <summary>
    /// Base class for <code>RadioButtonDrawer</code> and <code>CheckboxDrawer</code>
    /// </summary>
    public class CheckableBaseDrawer<T> : TextualBaseDrawer<T> where T : CheckableBase
    {
        public CheckableBaseDrawer(T control, string format)
            : base(control, format, 4)
        {
            
        }

        /// <summary>
        /// Draws the checkbox given in the Control-Property
        /// </summary>
        public override void Draw()
        {
            base.Draw();

            char checkChar;
            switch (_control.Checked)
            {
                case CheckState.Checked:
                    checkChar = 'X';
                    break;
                case CheckState.Unchecked:
                case CheckState.Indeterminate:
                    checkChar = ' ';
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            base.Draw(Control.IsFocused ? Control.BackColor : Control.ForeColor,
                      Control.IsFocused ? Control.ForeColor : Control.BackColor,
                      checkChar);

            if (_control.Checked == CheckState.Indeterminate)
            {
                // overwrites the character which indicates the checked-state of the checkbox,
                // to have an inverted background-color
                FogConsole.Write(Boundary.Left + 1, Boundary.Top,
                                 checkChar.ToString(), Boundary,
                                 bColor: Control.IsFocused ? ConsoleColor.Black : ConsoleColor.Gray);
            }
        }
    }
}
