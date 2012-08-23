using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FoggyConsole.Controls
{
    public interface IControlDrawer
    {
        /// <summary>
        /// The Control which should be drawn
        /// </summary>
        Control Control { get; set; }

        /// <summary>
        /// Draws the Control stored in the Control-Property
        /// </summary>
        /// <param name="leftOffset">Offset for the left value (used to convert local coordinates within a container to global ones)</param>
        /// <param name="topOffset">Offset for the top value (used to convert local coordinates within a container to global ones)</param>
        /// <param name="boundary">The boundary of the <code>ContainerControl</code> in which the <code>Control</code> is placed</param>
        void Draw(int leftOffset, int topOffset, Rectangle boundary);
    }
}
