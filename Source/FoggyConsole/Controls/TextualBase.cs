using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FoggyConsole.Controls
{
    /// <summary>
    /// Base class for <code>Label</code>, <code>Button</code> and <code>Checkbox</code>.
    /// A control which is able to display a single line of text.
    /// </summary>
    public abstract class TextualBase : Control
    {
        private string _text;

        /// <summary>
        /// Gets or sets the text which is drawn onto this Control.
        /// </summary>
        public string Text
        {
            get { return _text; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("text");
                if (value.Any(c => c == '\n' || c == '\r'))
                    throw new ArgumentException("Text can't contain linefeeds or carriage returns.", "text");

                var oldText = _text;
                _text = value;

                // if the width is zero the control will always take as much width
                // as needed to draw the full text, so the text-lenght directly affects the size
                if (Width == 0)
                {
                    var oldLen = _text == null ? 0 : _text.Length;
                    if (_text.Length < oldLen)
                        base.RequestRedraw(RedrawRequestReason.BecameSmaller);
                    else if (_text.Length > oldLen)
                        base.RequestRedraw(RedrawRequestReason.BecameBigger);
                    else if (oldText != _text)
                        base.RequestRedraw(RedrawRequestReason.ContentChanged);
                }
                else if(oldText != _text)
                {
                    base.RequestRedraw(RedrawRequestReason.ContentChanged);
                }
            }
        }

        protected TextualBase(string text, IControlDrawer drawer = null)
            : base(drawer)
        {
            Text = text;
            base.Height = 1;
            base.IsHeightFixed = true;
        }
    }

    public abstract class TextualBaseDrawer<T> : ControlDrawer<T> where T : TextualBase
    {
        private readonly string _format;
        private readonly int _addWidth;

        protected TextualBaseDrawer(T control, string format, int addWidth)
            : base(control)
        {
            this._format = format;
            this._addWidth = addWidth;
        }

        protected void Draw(ConsoleColor fColor, ConsoleColor bColor, params object[] args)
        {
            var text = String.Format(_format, new object[] { _control.Text }.Concat(args).ToArray());

            if (text.Length + 4 > Control.Width && Control.Width != 0)
                text = text.Substring(0, Control.Width - 4);

            FogConsole.Write(Boundary.Left, Boundary.Top, text, Boundary, fColor, bColor);
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
            if (_control.Width == 0)
                Boundary.Width = _control.Text.Length + _addWidth;
            base.FixBoundaryWidth(boundary);
        }
    }
}
