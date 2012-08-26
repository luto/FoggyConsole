using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FoggyConsole.Controls
{
    /// <summary>
    /// Base class for <code>Label</code> und <code>Button</code>.
    /// A control which is able to display a single line of text.
    /// </summary>
    public abstract class TextualBase : Control
    {
        private string _text;

        /// <summary>
        /// Gets or sets the text which is drawn onto this Control.
        /// </summary>
        public string Text
        {
            get { return _text; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("text");
                if (value.Any(c => c == '\n' || c == '\r'))
                    throw new ArgumentException("Text can't contain linefeeds or carriage returns.", "text");

                var oldText = _text;
                _text = value;

                // if the width is zero the control will always take as much width
                // as needed to draw the full text, so the text-lenght directly affects the size
                if (Width == 0)
                {
                    var oldLen = _text == null ? 0 : _text.Length;
                    if (_text.Length < oldLen)
                        base.RequestRedraw(RedrawRequestReason.BecameSmaller);
                    else if (_text.Length > oldLen)
                        base.RequestRedraw(RedrawRequestReason.BecameBigger);
                    else if (oldText != _text)
                        base.RequestRedraw(RedrawRequestReason.ContentChanged);
                }
                else if(oldText != _text)
                {
                    base.RequestRedraw(RedrawRequestReason.ContentChanged);
                }
            }
        }

        protected TextualBase(string text, IControlDrawer drawer = null)
            : base(drawer)
        {
            Text = text;
            base.Height = 1;
            base.IsHeightFixed = true;
        }
    }
}
