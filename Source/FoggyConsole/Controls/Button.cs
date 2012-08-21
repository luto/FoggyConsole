using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoggyConsole.Controls
{
    /// <summary>
    /// A <code>Control</code> that can be triggered when focused
    /// The standard look is: <example>[ Button Name ]</example> (drawn by <code>ButtonDrawer</code>)
    /// </summary>
    public class Button : Control
    {
        private ButtonDrawer ButtonDrawer { get { return Drawer as ButtonDrawer; } }
        public string Text { get; set; }


        /// <summary>
        /// Creates a new <code>Button</code>
        /// </summary>
        /// <param name="drawer">The <code>ControlDrawer</code> to use. If null a new instance of <code>ButtonDrawer</code> will be used.</param>
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


        public override void Draw(int leftOffset, int topOffset)
        {
            if(Control == null)
                throw new InvalidOperationException("Can't draw without the Control-Property set.");

            FogConsole.Write(leftOffset + Button.Left,
                             topOffset  + Button.Top,
                             "[ " + Button.Text + " ]");
        }
    }
}
