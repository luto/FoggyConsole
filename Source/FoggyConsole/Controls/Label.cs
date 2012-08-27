using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FoggyConsole.Controls
{
    /// <summary>
    /// A control which displays one line of text
    /// </summary>
    public class Label : TextualBase
    {
        private ConsoleColor _bColor;
        private ConsoleColor _fColor;
        private ContentAlign _align;

        /// <summary>
        /// The background-color
        /// </summary>
        public ConsoleColor BColor
        {
            get { return _bColor; }
            set
            {
                _bColor = value;
                RequestRedraw(RedrawRequestReason.ContentChanged);
            }
        }

        /// <summary>
        /// The foreground-color
        /// </summary>
        public ConsoleColor FColor
        {
            get { return _fColor; }
            set
            {
                _fColor = value;
                RequestRedraw(RedrawRequestReason.ContentChanged);
            }
        }

        /// <summary>
        /// The align of the text
        /// </summary>
        public ContentAlign Align
        {
            get { return _align; }
            set
            {
                _align = value;
                RequestRedraw(RedrawRequestReason.ContentChanged);
            }
        }

        /// <summary>
        /// Creates a new Label
        /// </summary>
        /// <param name="text">The text on the Label</param>
        /// <param name="drawer">The <code>ControlDrawer</code> to use. If null a new instance of <code>LabelDrawer</code> will be used.</param>
        public Label(string text, IControlDrawer drawer = null)
            : base(text, drawer)
        {
            if(drawer == null)
                base.Drawer = new LabelDrawer(this);

            this.FColor = ConsoleColor.Gray;
            this.BColor = ConsoleColor.Black;
        }
    }

    /// <summary>
    /// Draws a <code>Label</code>-Control
    /// </summary>
    public class LabelDrawer : TextualBaseDrawer<Label>
    {
        public LabelDrawer(Label control)
            : base(control, "{1}", 0)
        {
        }

        /// <summary>
        /// Draws the <code>Label</code> given in the Control-Property.
        /// </summary>
        /// <exception cref="InvalidOperationException">Is thrown if the Control-Property isn't set.</exception>
        /// <exception cref="InvalidOperationException">Is thrown if the CalculateBoundary-Method hasn't been called.</exception>
        public override void Draw()
        {
            base.Draw();

            var text = _control.Text;
            if (text.Length > Boundary.Width)
            {
                text = text.Substring(0, Boundary.Width);
            }
            else
            {
                switch (_control.Align)
                {
                    case ContentAlign.Right:
                        text = text.PadRight(Boundary.Width);
                        break;
                    case ContentAlign.Center:
                        var fillStr = new string(' ', (Boundary.Width - text.Length)/2);
                        text = fillStr + text + fillStr;
                    break;
                    case ContentAlign.Left:
                        text = text.PadLeft(Boundary.Width);
                        break;
                }
            }

            base.Draw(_control.FColor, _control.BColor, text);
        }
    }
}
