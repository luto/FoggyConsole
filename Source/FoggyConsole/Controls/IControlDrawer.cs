using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FoggyConsole.Controls
{
    /// <summary>
    /// Represents a class which is able to draw an specific type of control.
    /// </summary>
    public interface IControlDrawer
    {
        /// <summary>
        /// The Control which should be drawn
        /// </summary>
        Control Control { get; set; }

        /// <summary>
        /// The boundary of the Control given in the Control-Property
        /// </summary>
        Rectangle Boundary { get; }

        /// <summary>
        /// Draws the Control stored in the Control-Property
        /// </summary>
        void Draw();

        /// <summary>
        /// Calculates the boundary of the Control given in the Control-Property and stores it in the Boundary-Property
        /// </summary>
        /// <param name="leftOffset">Offset for the left value (used to convert local coordinates within a container to global ones)</param>
        /// <param name="topOffset">Offset for the top value (used to convert local coordinates within a container to global ones)</param>
        /// <param name="boundary">The boundary of the <code>ContainerControl</code> in which the <code>Control</code> is placed</param>
        void CalculateBoundary(int leftOffset, int topOffset, Rectangle boundary);
    }
}
