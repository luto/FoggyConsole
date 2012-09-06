/*
This file is part of FoggyConsole.

FoggyConsole is free software: you can redistribute it and/or modify
it under the terms of the GNU Lesser General Public License as
published by the Free Software Foundation, either version 3 of
the License, or (at your option) any later version.

FoogyConsole is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU Lesser General Public License for more details.

You should have received a copy of the GNU Lesser General Public License
along with FoggyConsole.  If not, see <http://www.gnu.org/licenses/lgpl.html>.
*/

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

    /// <summary>
    /// Base class for all Drawers which can draw an <code>TextualBase</code>
    /// </summary>
    public abstract class TextualBaseDrawer<T> : ControlDrawer<T> where T : TextualBase
    {
        private readonly string _format;
        private readonly int _addWidth;

        /// <summary>
        /// Creates a new TextualBaseDrawer
        /// </summary>
        /// <param name="control">The control to draw</param>
        /// <param name="format">The format to use to format the objects given</param>
        /// <param name="addWidth"></param>
        /// <exception cref="ArgumentNullException">Is thrown if <paramref name="format"/> is null</exception>
        protected TextualBaseDrawer(T control, string format, int addWidth)
            : base(control)
        {
            if(format == null)
                throw new ArgumentNullException("format");
            this._format = format;
            this._addWidth = addWidth;
        }

        /// <summary>
        /// Draws the objects given in <paramref name="args"/> using the format string given in the ctor
        /// </summary>
        /// <param name="fColor">The foreground color</param>
        /// <param name="bColor">The background color</param>
        /// <param name="args">The objects to draw</param>
        protected void Draw(ConsoleColor fColor, ConsoleColor bColor, params object[] args)
        {
            var text = String.Format(_format, new object[] { _control.Text }.Concat(args).ToArray());

            if (text.Length + _addWidth > Control.Width && Control.Width != 0)
                text = text.Substring(0, Control.Width - _addWidth);

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
