using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FoggyConsole.Controls
{
    public class Checkbox : TextualBase, IInputHandler
    {
        public enum CheckState
        {
            Checked,
            Unchecked,
            Indeterminate
        }

        public class CheckboxCheckedChangedEventArgs : EventArgs
        {
            public CheckState State { get; private set; }
            public bool Cancel { get; set; }

            public CheckboxCheckedChangedEventArgs(CheckState state)
            {
                this.State = state;
                this.Cancel = false;
            }
        }


        private CheckState _checked;
        /// <summary>
        /// 
        /// </summary>
        public CheckState Checked
        {
            get { return _checked; }
            set { _checked = value; }
        }


        public Checkbox(string text, ControlDrawer<Checkbox> drawer = null)
            : base(text, drawer)
        {
            if(drawer == null)
                Drawer = new CheckboxDrawer(this);
            this.Checked = CheckState.Indeterminate;
            base.IsFocusedChanged += (sender, args) => RequestRedraw(RedrawRequestReason.ContentChanged);
        }

        private event EventHandler<CheckboxCheckedChangedEventArgs> CheckedChanged;

        /// <summary>
        /// Retruns 
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        private bool OnCheckedChanged(CheckState state)
        {
            var args = new CheckboxCheckedChangedEventArgs(state);
            if (CheckedChanged != null)
                CheckedChanged(this, args);
            return args.Cancel;
        }

        /// <summary>
        /// Handles the key-userinput which is given in <paramref name="keyInfo"/>
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

                bool cancel = OnCheckedChanged(newState);
                if (!cancel)
                {
                    Checked = newState;
                    RequestRedraw(RedrawRequestReason.ContentChanged);
                }
                return true;
            }
            return false;
        }
    }

    public class CheckboxDrawer : TextualBaseDrawer<Checkbox>
    {
        public CheckboxDrawer(Checkbox control = null)
            : base(control, "[{1}] {0}", 4)
        {
        }

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

            base.Draw(Control.IsFocused ? ConsoleColor.Black : ConsoleColor.Gray,
                      Control.IsFocused ? ConsoleColor.Gray : ConsoleColor.Black,
                      checkChar);

            if(_control.Checked == Checkbox.CheckState.Indeterminate)
                FogConsole.Write(Boundary.Left + 1, Boundary.Top, checkChar.ToString(), Boundary, bColor: Control.IsFocused ? ConsoleColor.Black : ConsoleColor.Gray);
        }
    }
}
