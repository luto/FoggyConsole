using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FoggyConsole.Controls
{
    /// <summary>
    /// A <code>Control</code> that can be triggered when focused.
    /// The standard look is: <example>[ Button Name ]</example> (drawn by <code>ButtonDrawer</code>).
    /// If no Width is set the button will use as much space as required.
    /// </summary>
    public class Button : Control
    {
        private ButtonDrawer ButtonDrawer { get { return Drawer as ButtonDrawer; } }

        /// <summary>
        /// Gets or sets the text which is drawn onto the Button.
        /// </summary>
        public string Text { get; set; }
        /// <summary>
        /// Gets the standard height for buttons.
        /// </summary>
        public new int Height { get { return 1; } }


        /// <summary>
        /// Creates a new <code>Button</code>
        /// </summary>
        /// <param name="text">The text which is drawn onto the Button.</param>
        /// <param name="drawer">The <code>ButtonDrawer</code> to use. If null a new instance of <code>ButtonDrawer</code> will be used.</param>
        /// <exception cref="ArgumentException">Thrown if the ButtonDrawer which should be set already has an other Button assigned</exception>
        public Button(string text, ButtonDrawer drawer = null)
            : base(drawer)
        {
            if(text == null)
                throw new ArgumentNullException("text");
            if(drawer == null)
                base.Drawer = new ButtonDrawer(this);

            this.Text = text;
        }
    }

    /// <summary>
    /// Draws a <code>Button</code>-Control
    /// </summary>
    public class ButtonDrawer : ControlDrawer
    {
        private Button Button { get { return Control as Button; } }


        public ButtonDrawer(Button control = null)
            : base(control)
        {
        }


        public override void Draw(int leftOffset, int topOffset, Rectangle boundary)
        {
            if(Control == null)
                throw new InvalidOperationException("Can't draw without the Control-Property set.");

            var text = Button.Text;
            if (Button.Width != 0 && text.Length + 4 > Button.Width)
                text = text.Substring(0, Button.Width - 4);

            FogConsole.Write(leftOffset + Button.Left,
                             topOffset  + Button.Top,
                             "[ " + text + " ]",
                             boundary);
        }
    }
}
