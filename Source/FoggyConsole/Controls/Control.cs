using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoggyConsole.Controls
{
    /// <summary>
    /// The base class for all controls
    /// </summary>
    public abstract class Control
    {
        private ControlDrawer _drawer;

        /// <summary>
        /// Distance from the top edge of its Container in characters
        /// </summary>
        public int Top { get; set; }
        
        /// <summary>
        /// Distance from the left edge of its Container in characters
        /// </summary>
        public int Left { get; set; }
        
        /// <summary>
        /// The width of this Control in characters
        /// </summary>
        public int Width { get; set; }
        
        /// <summary>
        /// The height of this Control in characters
        /// </summary>
        public int Height { get; set; }
        
        /// <summary>
        /// The name of this Control
        /// This must be unique within its Container
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The <code>ContainerControl</code> in which this Control is placed in
        /// </summary>
        public ContainerControl Container { get; set; }

        /// <summary>
        /// An instance of a subclass of <code>ControlDrawer</code> which is able to draw this specific type of Control
        /// </summary>
        /// <exception cref="ArgumentException">Thrown if the ControlDrawer which should be set already has a Control assigned</exception>
        public ControlDrawer Drawer
        {
            get { return _drawer; }
            set
            {
                if(value != null && value.Control != null && value.Control != this)
                    throw new ArgumentException("Drawer already has an other Control assigned.", "value");
                _drawer = value;
            }
        }

        
        public Control(ControlDrawer drawer)
        {
            Drawer = drawer;
        }
    }
}
