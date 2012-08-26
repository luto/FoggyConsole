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
        /// <exception cref="InvalidOperationException">Is thrown if the Control-Property isn't set.</exception>
        /// <exception cref="InvalidOperationException">Is thrown if the CalculateBoundary-Method hasn't been called.</exception>
        public virtual void Draw()
        {
            if (Control == null)
                throw new InvalidOperationException("Can't draw without the Control-Property set.");
            if (Boundary == null)
                throw new InvalidOperationException("CalculateBoundary has to be called before Draw can be called.");
        }

        /// <summary>
        /// Calculates the boundary of the Control given in the Control-Property and stores it in the Boundary-Property
        /// </summary>
        /// <param name="leftOffset">Offset for the left value (used to convert local coordinates within a container to global ones)</param>
        /// <param name="topOffset">Offset for the top value (used to convert local coordinates within a container to global ones)</param>
        /// <param name="boundary">The boundary of the <code>ContainerControl</code> in which the <code>Control</code> is placed</param>
        public virtual void CalculateBoundary(int leftOffset, int topOffset, Rectangle boundary)
        {
            int left = leftOffset + Control.Left;
            int top = topOffset + Control.Top;
            int width = Control.Width;
            int height = Control.Height;

            Boundary = new Rectangle(left,
                                     top,
                                     height,
                                     width);

            FixBoundaryHeight(boundary);
            FixBoundaryWidth(boundary);
        }

        /// <summary>
        /// Resizes the boundary-width to fit into <paramref name="containerBound"/>.
        /// This should be called if the boundary-width gets changed after CalculateBoundary has been called.
        /// </summary>
        /// <param name="containerBound">The boundary of the <code>ContainerControl</code> which contains the control given in the Control-Property</param>
        protected void FixBoundaryWidth(Rectangle containerBound)
        {
            if (Boundary.Left + Boundary.Width > containerBound.Left + containerBound.Width)
                Boundary.Width = containerBound.Width - (Boundary.Left - containerBound.Left);
        }

        /// <summary>
        /// Resizes the boundary-height to fit into <paramref name="containerBound"/>.
        /// This should be called if the boundary-height gets changed after CalculateBoundary has been called.
        /// </summary>
        /// <param name="containerBound">The boundary of the <code>ContainerControl</code> which contains the control given in the Control-Property</param>
        protected void FixBoundaryHeight(Rectangle containerBound)
        {
            if (Boundary.Top + Boundary.Height > containerBound.Top + containerBound.Height)
                Boundary.Height = containerBound.Height - (Boundary.Top - containerBound.Top);
        }
    }
}
