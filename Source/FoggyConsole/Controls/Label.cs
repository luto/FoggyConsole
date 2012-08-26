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
    public class LabelDrawer : ControlDrawer<Label>
    {
        public LabelDrawer(Label control)
            : base(control)
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

            FogConsole.Write(Boundary.Left,
                             Boundary.Top,
                             text,
                             Boundary,
                             _control.FColor,
                             _control.BColor);
        }

        /// <summary>
        /// Calculates the boundary of the Label given in the Control-Property and stores it in the Boundary-Property
        /// </summary>
        /// <param name="leftOffset">Offset for the left value (used to convert local coordinates within a container to global ones)</param>
        /// <param name="topOffset">Offset for the top value (used to convert local coordinates within a container to global ones)</param>
        /// <param name="boundary">The boundary of the <code>ContainerControl</code> in which the <code>Label</code> is placed</param>
        public override void CalculateBoundary(int leftOffset, int topOffset, Rectangle boundary)
        {
            base.CalculateBoundary(leftOffset, topOffset, boundary);
            if (_control.Width == 0)
                Boundary.Width = _control.Text.Length;
            FixBoundaryWidth(boundary);
        }
    }
}
