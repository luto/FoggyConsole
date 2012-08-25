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
        private bool _isFocused;
        private bool _isWidthFixed;
        private bool _isHeightFixed;

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
                var oldTop = _top;

                _top = value;

                if (_top < oldTop)
                    RequestRedraw(RedrawRequestReason.BecameSmaller);
                else if (_top < oldTop)
                    RequestRedraw(RedrawRequestReason.BecameBigger);
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
                var oldLeft = _left;

                _left = value;

                if (_left < oldLeft)
                    RequestRedraw(RedrawRequestReason.BecameSmaller);
                else if (_left < oldLeft)
                    RequestRedraw(RedrawRequestReason.BecameBigger);
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
                if (IsWidthFixed)
                    throw new InvalidOperationException("The Width can't be changed.");
                var oldWidth = _width;

                _width = value;

                if (_width < oldWidth)
                    RequestRedraw(RedrawRequestReason.BecameSmaller);
                else if (_width < oldWidth)
                    RequestRedraw(RedrawRequestReason.BecameBigger);
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
                if (IsHeightFixed)
                    throw new InvalidOperationException("The Height can't be changed.");
                var oldHeight = _height;

                _height = value;

                if (_height < oldHeight)
                    RequestRedraw(RedrawRequestReason.BecameSmaller);
                else if (_height < oldHeight)
                    RequestRedraw(RedrawRequestReason.BecameBigger);
            }
        }
        
        /// <summary>
        /// The name of this Control, must be unique within its Container
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// True if the control is focuses, otherwise false
        /// </summary>
        public bool IsFocused
        {
            get { return _isFocused; }
            set
            {
                _isFocused = value;
                OnIsFocusedChanged();
            }
        }

        /// <summary>
        /// True if the Width of this control can be changed
        /// </summary>
        public bool IsWidthFixed
        {
            get { return _isWidthFixed; }
            protected set { _isWidthFixed = value; }
        }

        /// <summary>
        /// True if the Height of this control can be changed
        /// </summary>
        public bool IsHeightFixed
        {
            get { return _isHeightFixed; }
            protected set { _isHeightFixed = value; }
        }

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
        /// Fired if the <code>IsFocused</code>-Property has been changed
        /// </summary>
        public event EventHandler IsFocusedChanged;

        private void OnIsFocusedChanged()
        {
            if (IsFocusedChanged != null)
                IsFocusedChanged(this, EventArgs.Empty);
        }

        /// <summary>
        /// Fired if the <code>RedrawNeeded</code>-Property has been changed
        /// </summary>
        public event EventHandler<RedrawRequestedEventArgs> RedrawRequested;

        private void OnRedrawNeededChanged(RedrawRequestReason reason)
        {
            if (RedrawRequested != null)
                RedrawRequested(this, new RedrawRequestedEventArgs(reason));
        }

        /// <summary>
        /// Requests an redraw of this control and stores <paramref name="reason"/> for the receiver
        /// </summary>
        /// <param name="reason">The reason for the Redraw-Request</param>
        protected void RequestRedraw(RedrawRequestReason reason)
        {
            OnRedrawNeededChanged(reason);
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
