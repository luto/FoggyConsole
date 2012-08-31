using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FoggyConsole.Controls
{
    /// <summary>
    /// A control which displays a Text and has a Checked-State
    /// </summary>
    public class Checkbox : CheckableBase
    {
        /// <summary>
        /// Creates a new Checkbox
        /// </summary>
        /// <param name="text">The text to display</param>
        /// <param name="drawer">The <code>ControlDrawer</code> to use. If null a new instance of <code>CheckboxDrawer</code> will be used.</param>
        /// <exception cref="ArgumentException">Thrown if the <code>ControlDrawer</code> which should be set already has an other Control assigned</exception>
        public Checkbox(string text, ControlDrawer<Checkbox> drawer = null)
            : base(text, drawer)
        {
            if(drawer == null)
                Drawer = new CheckboxDrawer(this);
            this.Checked = CheckState.Indeterminate;
        }

        /// <summary>
        /// Handles the key-userinput which is given in <paramref name="keyInfo"/>.
        /// The Checked-State will flip if the user presses the spacebar.
        /// </summary>
        /// <returns>true if the keypress was handled, otherwise false</returns>
        /// <param name="keyInfo">The keypress to handle</param>
        public override bool HandleKeyInput(ConsoleKeyInfo keyInfo)
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
    public class CheckboxDrawer : CheckableBaseDrawer<Checkbox>
    {
        public CheckboxDrawer(Checkbox control = null)
            : base(control, "[{1}] {0}")
        {
        }
    }
}
