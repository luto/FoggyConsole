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
    public class Button : Control, IInputHandler
    {
        private string _text;

        /// <summary>
        /// Gets or sets the text which is drawn onto the Button.
        /// </summary>
        public string Text
        {
            get { return _text; }
            set
            {
                int oldLen = _text == null ? 0 : _text.Length;
                _text = value;

                // if the width is zero the control will always take as much width
                // as needed to draw the full text, so the text-lenght directly affects the size
                if (Width == 0)
                {
                    if (_text.Length < oldLen)
                        base.RequestRedraw(RedrawRequestReason.BecameSmaller);
                    else if (_text.Length > oldLen)
                        base.RequestRedraw(RedrawRequestReason.BecameBigger);
                    else
                        base.RequestRedraw(RedrawRequestReason.ContentChanged);
                }
            }
        }
        /// <summary>
        /// Gets the standard height for buttons.
        /// </summary>
        public new int Height { get { return 1; } }

        /// <summary>
        /// Fired if the button is focuses and the user presses the space bar
        /// </summary>
        public event EventHandler Pressed;

        /// <summary>
        /// Creates a new <code>Button</code>
        /// </summary>
        /// <param name="text">The text which is drawn onto the Button.</param>
        /// <param name="drawer">The <code>ButtonDrawer</code> to use. If null a new instance of <code>ButtonDrawer</code> will be used.</param>
        /// <exception cref="ArgumentException">Thrown if the ButtonDrawer which should be set already has an other Button assigned</exception>
        public Button(string text, ControlDrawer<Button> drawer = null)
            : base(drawer)
        {
            if(text == null)
                throw new ArgumentNullException("text");
            if(drawer == null)
                base.Drawer = new ButtonDrawer(this);

            this.Text = text;
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
        public override void Draw()
        {
            if(Control == null)
                throw new InvalidOperationException("Can't draw without the Control-Property set.");

            var text = _control.Text;
            if (text.Length + 4 > Boundary.Width)
                text = text.Substring(0, Boundary.Width - 4);

            FogConsole.Write(Boundary.Left,
                             Boundary.Top,
                             "[ " + text + " ]",
                             Boundary,
                             Control.IsFocused ? ConsoleColor.Black : ConsoleColor.Gray,
                             Control.IsFocused ? ConsoleColor.Gray : ConsoleColor.Black);
        }

        /// <summary>
        /// Calculates the boundary of the Control given in the Control-Property and stores it in the Boundary-Property
        /// </summary>
        /// <param name="leftOffset">Offset for the left value (used to convert local coordinates within a container to global ones)</param>
        /// <param name="topOffset">Offset for the top value (used to convert local coordinates within a container to global ones)</param>
        /// <param name="boundary">The boundary of the <code>ContainerControl</code> in which the <code>Control</code> is placed</param>
        public override void CalculateBoundary(int leftOffset, int topOffset, Rectangle boundary)
        {
            Boundary = new Rectangle(leftOffset + Control.Left,
                                     topOffset + Control.Top,
                                     Control.Height,
                                     (Control.Width == 0 ? _control.Text.Length : Control.Width) + 4);
        }
    }
}
