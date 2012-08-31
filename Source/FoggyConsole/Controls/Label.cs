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
    /// A control which displays one line of text
    /// </summary>
    public class Label : TextualBase
    {
        private ContentAlign _align;

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
        /// <exception cref="ArgumentException">Thrown if the <code>ControlDrawer</code> which should be set already has an other Control assigned</exception>
        public Label(string text, IControlDrawer drawer = null)
            : base(text, drawer)
        {
            if(drawer == null)
                base.Drawer = new LabelDrawer(this);
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

            base.Draw(Control.ForeColor, Control.BackColor, text);
        }
    }
}
