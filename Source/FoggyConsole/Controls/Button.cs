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
        /// <exception cref="ArgumentException">Thrown if the ButtonDrawer which should be set already has an other Button assigned</exception>
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
    public class ButtonDrawer : ControlDrawer<Button>
    {
        public ButtonDrawer(Button control = null)
            : base(control)
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

            var text = _control.Text;
            if (text.Length + 4 > Control.Width && Control.Width != 0)
                text = text.Substring(0, Control.Width - 4);

            FogConsole.Write(Boundary.Left,
                             Boundary.Top,
                             "[ " + text + " ]",
                             Boundary,
                             Control.IsFocused ? ConsoleColor.Black : ConsoleColor.Gray,
                             Control.IsFocused ? ConsoleColor.Gray : ConsoleColor.Black);
        }

        /// <summary>
        /// Calculates the boundary of the Button given in the Control-Property and stores it in the Boundary-Property
        /// </summary>
        /// <param name="leftOffset">Offset for the left value (used to convert local coordinates within a container to global ones)</param>
        /// <param name="topOffset">Offset for the top value (used to convert local coordinates within a container to global ones)</param>
        /// <param name="boundary">The boundary of the <code>ContainerControl</code> in which the <code>Button</code> is placed</param>
        public override void CalculateBoundary(int leftOffset, int topOffset, Rectangle boundary)
        {
            base.CalculateBoundary(leftOffset, topOffset, boundary);
            if(_control.Width == 0)
                Boundary.Width = _control.Text.Length + 4;
            base.FixBoundaryWidth(boundary);
        }
    }
}
