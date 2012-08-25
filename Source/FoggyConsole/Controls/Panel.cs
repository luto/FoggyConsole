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
        /// <param name="drawer">The <code>PanelDrawer</code> to use. If null a new instance of <code>PanelDrawer</code> will be used.</param>
        /// <exception cref="ArgumentException">Thrown if the PanelDrawer which should be set already has an other Panel assigned</exception>
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
    public class PanelDrawer : ControlDrawer<Panel>
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
        public override void Draw()
        {
            if (Control == null)
                throw new InvalidOperationException("Can't draw without the Control-Property set.");
            if (Control.Width == 0 || Control.Height == 0)
                return;

            if (Application.DEBUG_MODE)
            {
                var boxBound = new Rectangle(Boundary.Left,
                                             Boundary.Top,
                                             Control.Height,
                                             Control.Width);

                FogConsole.DrawBox(boxBound, DEBUG_CHAR_SET, Boundary,
                                   bColor: DEBUG_COLORS[DEBUG_COLOR_COUNTER],
                                   bFillColor: DEBUG_COLORS[DEBUG_COLOR_COUNTER],
                                   fill: true);
                FogConsole.Write(Boundary.Left, Boundary.Top, "{" + _control.Name + "}", Boundary, ConsoleColor.Black, DEBUG_COLORS[DEBUG_COLOR_COUNTER]);
                DEBUG_COLOR_COUNTER++;
                if (DEBUG_COLOR_COUNTER == DEBUG_COLORS.Length)
                    DEBUG_COLOR_COUNTER = 0;
            }

            foreach (var control in _control)
            {
                control.Drawer.Draw();
            }
        }

        /// <summary>
        /// Calculates the boundary of the Control given in the Control-Property and stores it in the Boundary-Property
        /// </summary>
        /// <param name="leftOffset">Offset for the left value (used to convert local coordinates within a container to global ones)</param>
        /// <param name="topOffset">Offset for the top value (used to convert local coordinates within a container to global ones)</param>
        /// <param name="boundary">The boundary of the <code>ContainerControl</code> in which the <code>Control</code> is placed</param>
        public override void CalculateBoundary(int leftOffset, int topOffset, Rectangle boundary)
        {
            int left = leftOffset + Control.Left;
            int top = topOffset + Control.Top;
            int width = Control.Width;
            int height = Control.Height;

            if (left + width > boundary.Left + boundary.Width)
                width = boundary.Width - (left - boundary.Left);
            if (top + height > boundary.Top + boundary.Height)
                height = boundary.Height - (top - boundary.Top);

            Boundary = new Rectangle(left,
                                     top,
                                     height,
                                     width);

            foreach (var control in _control)
            {
                control.Drawer.CalculateBoundary(left, top, Boundary);
            }
        }
    }
}
