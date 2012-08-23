using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FoggyConsole.Controls
{
    /// <summary>
    /// The base class for all controls
    /// </summary>
    public abstract class Control
    {
        private IControlDrawer _drawer;
        private int _top;
        private int _left;
        private int _width;
        private int _height;

        /// <summary>
        /// Distance from the top edge of its Container in characters
        /// </summary>
        public int Top
        {
            get { return _top; }
            set
            {
                if (value < 0)
                    throw new ArgumentException("Top has to be bigger than zero.");
                _top = value;
            }
        }
        
        /// <summary>
        /// Distance from the left edge of its Container in characters
        /// </summary>
        public int Left
        {
            get { return _left; }
            set
            {
                if (value < 0)
                    throw new ArgumentException("Left has to be bigger than zero.");
                _left = value;
            }
        }
        
        /// <summary>
        /// The width of this Control in characters
        /// </summary>
        public int Width
        {
            get { return _width; }
            set
            {
                if (value < 0)
                    throw new ArgumentException("Width has to be bigger than zero.");
                _width = value;
            }
        }
        
        /// <summary>
        /// The height of this Control in characters
        /// </summary>
        public int Height
        {
            get { return _height; }
            set
            {
                if (value < 0)
                    throw new ArgumentException("Height has to be bigger than zero.");
                _height = value;
            }
        }
        
        /// <summary>
        /// The name of this Control, must be unique within its Container
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Used to determine the order of controls when the user uses the TAB-key navigate between them
        /// </summary>
        public int TabIndex { get; set; }

        /// <summary>
        /// The <code>ContainerControl</code> in which this Control is placed in
        /// </summary>
        public ContainerControl Container { get; set; }

        /// <summary>
        /// An instance of a subclass of <code>ControlDrawer</code> which is able to draw this specific type of Control
        /// </summary>
        /// <exception cref="ArgumentException">Thrown if the ControlDrawer which should be set already has an other Control assigned</exception>
        public IControlDrawer Drawer
        {
            get { return _drawer; }
            set
            {
                if(value != null && value.Control != null && value.Control != this)
                    throw new ArgumentException("Drawer already has an other Control assigned.", "value");
                _drawer = value ;
            }
        }

        /// <summary>
        /// Creates a new <code>Control</code>
        /// </summary>
        /// <param name="drawer">The <code>ControlDrawer</code> to set</param>
        /// <exception cref="ArgumentException">Thrown if the ControlDrawer which should be set already has an other Control assigned</exception>
        public Control(IControlDrawer drawer)
        {
            Drawer = drawer;
        }
    }
}
