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
        public Panel(PanelDrawer drawer = null)
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
    public class PanelDrawer : ControlDrawer
    {
        private Panel Panel { get { return Control as Panel; } }
        private static ConsoleColor[] DEBUG_COLORS = new [] { ConsoleColor.Blue, ConsoleColor.Red, ConsoleColor.Green, ConsoleColor.Cyan, ConsoleColor.Yellow };
        private static int DEBUG_COLOR_COUNTER = 0;

        public PanelDrawer(Panel control)
            : base(control)
        {
        }


        public override void Draw(int leftOffset, int topOffset, Rectangle boundary)
        {
            if(Panel.Width == 0 || Panel.Height == 0)
                return;

            leftOffset += Panel.Left;
            topOffset  += Panel.Top;
            
            boundary = new Rectangle(leftOffset, topOffset, Panel.Height, Panel.Width);

            if (Application.DEBUG_MODE)
            {
                FogConsole.DrawBox(boundary, new DrawCharacterSet(),
                                   bColor: DEBUG_COLORS[DEBUG_COLOR_COUNTER],
                                   bFillColor: DEBUG_COLORS[DEBUG_COLOR_COUNTER],
                                   fill: true);
                FogConsole.Write(leftOffset, topOffset, "{" + Panel.Name + "}", null, ConsoleColor.Black, DEBUG_COLORS[DEBUG_COLOR_COUNTER]);
                DEBUG_COLOR_COUNTER++;
                if (DEBUG_COLOR_COUNTER == DEBUG_COLORS.Length)
                    DEBUG_COLOR_COUNTER = 0;
            }

            foreach (var control in Panel)
            {
                control.Drawer.Draw(leftOffset, topOffset, boundary);
            }
        }
    }
}
