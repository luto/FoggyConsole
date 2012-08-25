using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FoggyConsole.Controls
{
    /// <summary>
    /// Base class for all ControlDrawers
    /// </summary>
    public abstract class ControlDrawer<T> : IControlDrawer where T : Control
    {
        /// <summary>
        /// The Control which should be drawn
        /// </summary>
        protected T _control;

        /// <summary>
        /// The Control which should be drawn
        /// </summary>
        /// <exception cref="ArgumentException">Thrown if the Control which should be set already has an other Drawer assigned</exception>
        public Control Control
        {
            get { return _control; }
            set
            {
                if(value.Drawer != null && value.Drawer != this)
                    throw new ArgumentException("Control already has a Drawer assigned", "value");
                if(!(value is T))
                    throw new ArgumentException("Control has to be of type: " + typeof(T).Name);
                _control = value as T;
            }
        }

        /// <summary>
        /// The boundary of the Control given in the Control-Property
        /// </summary>
        public Rectangle Boundary { get; protected set; }


        /// <summary>
        /// Creates a new ControlDrawer
        /// </summary>
        /// <param name="control">The Control to draw</param>
        public ControlDrawer(T control = null)
        {
            Control = control;
        }

        /// <summary>
        /// Draws the Control stored in the Control-Property
        /// </summary>
        public abstract void Draw();

        /// <summary>
        /// Calculates the boundary of the Control given in the Control-Property and stores it in the Boundary-Property
        /// </summary>
        /// <param name="leftOffset">Offset for the left value (used to convert local coordinates within a container to global ones)</param>
        /// <param name="topOffset">Offset for the top value (used to convert local coordinates within a container to global ones)</param>
        /// <param name="boundary">The boundary of the <code>ContainerControl</code> in which the <code>Control</code> is placed</param>
        public abstract void CalculateBoundary(int leftOffset, int topOffset, Rectangle boundary);
    }
}
