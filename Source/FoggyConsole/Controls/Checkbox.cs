using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FoggyConsole.Controls
{
    /// <summary>
    /// A control which displays a Text and has a Checked-State
    /// </summary>
    public class Checkbox : TextualBase, IInputHandler
    {
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
        public class CheckboxCheckedChangingEventArgs : EventArgs
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
            public CheckboxCheckedChangingEventArgs(CheckState state)
            {
                this.State = state;
                this.Cancel = false;
            }
        }


        private CheckState _checked;
        /// <summary>
        /// Gets or sets the state of this checkbox
        /// </summary>
        public CheckState Checked
        {
            get { return _checked; }
            set
            {
                var cancel = OnCheckedChanged(value);
                if (!cancel)
                {
                    _checked = value;
                    RequestRedraw(RedrawRequestReason.ContentChanged);
                }
            }
        }


        /// <summary>
        /// Creates a new Checkbox
        /// </summary>
        /// <param name="text"></param>
        /// <param name="drawer">The <code>ControlDrawer</code> to use. If null a new instance of <code>CheckboxDrawer</code> will be used.</param>
        public Checkbox(string text, ControlDrawer<Checkbox> drawer = null)
            : base(text, drawer)
        {
            if(drawer == null)
                Drawer = new CheckboxDrawer(this);
            this.Checked = CheckState.Indeterminate;
            base.IsFocusedChanged += (sender, args) => RequestRedraw(RedrawRequestReason.ContentChanged);
        }

        /// <summary>
        /// Fired if the Checked-State of this checkbox is going to change
        /// </summary>
        public event EventHandler<CheckboxCheckedChangingEventArgs> CheckedChanging;

        /// <summary>
        /// Fires the CheckedChanging-event and returns true if the process should be canceled
        /// </summary>
        /// <param name="state">The state the checkbox is going to have</param>
        /// <returns>True if the process should be canceled, otherwise false</returns>
        private bool OnCheckedChanged(CheckState state)
        {
            var args = new CheckboxCheckedChangingEventArgs(state);
            if (CheckedChanging != null)
                CheckedChanging(this, args);
            return args.Cancel;
        }

        /// <summary>
        /// Handles the key-userinput which is given in <paramref name="keyInfo"/>.
        /// The Checked-State will flip if the user presses the spacebar.
        /// </summary>
        /// <returns>true if the keypress was handled, otherwise false</returns>
        /// <param name="keyInfo">The keypress to handle</param>
        public bool HandleKeyInput(ConsoleKeyInfo keyInfo)
        {
            if(keyInfo.Key == ConsoleKey.Spacebar)
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
    /// Draws a <code>Checkbox</code>-Control
    /// </summary>
    public class CheckboxDrawer : TextualBaseDrawer<Checkbox>
    {
        public CheckboxDrawer(Checkbox control = null)
            : base(control, "[{1}] {0}", 4)
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
                case Checkbox.CheckState.Checked:
                    checkChar = 'X';
                    break;
                case Checkbox.CheckState.Unchecked:
                case Checkbox.CheckState.Indeterminate:
                    checkChar = ' ';
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            base.Draw(Control.IsFocused ? Control.BackColor : Control.ForeColor,
                      Control.IsFocused ? Control.ForeColor : Control.BackColor,
                      checkChar);

            if(_control.Checked == Checkbox.CheckState.Indeterminate)
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
