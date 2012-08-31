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
    /// A very basic <code>ContainerControl</code>
    /// It has no appearance, controls within it are aligned using thier top and left values.
    /// </summary>
    public class Panel : ContainerControl
    {
        /// <summary>
        /// Creates a new <code>Panel</code>
        /// </summary>
        /// <param name="drawer">The <code>ControlDrawer</code> to use. If null a new instance of <code>PanelDrawer</code> will be used.</param>
        /// <exception cref="ArgumentException">Thrown if the <code>ControlDrawer</code> which should be set already has an other Control assigned</exception>
        public Panel(ControlDrawer<Panel> drawer = null)
            : base(drawer)
        {
            if (drawer == null)
                base.Drawer = new PanelDrawer(this);
        }
    }

    /// <summary>
    /// Draws a <code>Panel</code>, which has no own appearance.
    /// All controls within the panel are drawn.
    /// </summary>
    public class PanelDrawer : ContainerControlDrawer<Panel>
    {
        private static readonly ConsoleColor[] DEBUG_COLORS = new[] { ConsoleColor.Blue, ConsoleColor.Red, ConsoleColor.Green, ConsoleColor.Cyan, ConsoleColor.Yellow };
        private static readonly DrawCharacterSet DEBUG_CHAR_SET = new DrawCharacterSet();
        private static int DEBUG_COLOR_COUNTER = 0;

        public PanelDrawer(Panel control)
            : base(control)
        {
        }

        /// <summary>
        /// Draws the <code>Panel</code> given in the Control-Property.
        /// </summary>
        /// <exception cref="InvalidOperationException">Is thrown if the Control-Property isn't set.</exception>
        /// <exception cref="InvalidOperationException">Is thrown if the CalculateBoundary-Method hasn't been called.</exception>
        public override void Draw()
        {
            base.Draw();
            if (Control.Width == 0 || Control.Height == 0)
                return;

            var boxBound = new Rectangle(Boundary.Left,
                                         Boundary.Top,
                                         Control.Height,
                                         Control.Width);
            ConsoleColor boxColor;

            if (Application.DEBUG_MODE)
            {
                boxColor = DEBUG_COLORS[DEBUG_COLOR_COUNTER];

                DEBUG_COLOR_COUNTER++;
                if (DEBUG_COLOR_COUNTER == DEBUG_COLORS.Length)
                    DEBUG_COLOR_COUNTER = 0;
            }
            else
            {
                boxColor = Control.BackColor;
            }

            FogConsole.DrawBox(boxBound, DEBUG_CHAR_SET, Boundary,
                               bColor: boxColor,
                               bFillColor: boxColor,
                               fill: true);

            if (Application.DEBUG_MODE)
                FogConsole.Write(Boundary.Left,
                                 Boundary.Top,
                                 "{" + _control.Name + "}",
                                 Boundary, Control.ForeColor, boxColor);

            foreach (var control in _control)
            {
                control.Drawer.Draw();
            }
        }
    }
}
