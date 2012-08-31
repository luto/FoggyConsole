/*
This file is part of FoggyConsole.

FoggyConsole is free software: you can redistribute it and/or modify
it under the terms of the GNU Lesser General Public License as
published by the Free Software Foundation, either version 3 of
the License, or (at your option) any later version.

FoogyConsole is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU Lesser General Public License for more details.

You should have received a copy of the GNU Lesser General Public License
along with FoggyConsole.  If not, see <http://www.gnu.org/licenses/lgpl.html>.
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FoggyConsole.Controls
{
    /// <summary>
    /// A Control which is able to display the progress of an ongoing task
    /// </summary>
    public class Progressbar : Control
    {
        private int _value;

        /// <summary>
        /// The progress which is shown, 0 is no progress, 100 is finished
        /// </summary>
        public int Value
        {
            get { return _value; }
            set
            {
                if (value < 0 || value > 100)
                    throw new ArgumentOutOfRangeException("value");
                _value = value;
                OnValueChanged();
            }
        }

        /// <summary>
        /// Fired if the Value-Property has changed
        /// </summary>
        public event EventHandler ValueChanged;

        /// <summary>
        /// Creates a new Progressbar
        /// </summary>
        /// <param name="drawer">The <code>ControlDrawer</code> to use. If null a new instance of <code>ProgressbarDrawer</code> will be used.</param>
        /// <exception cref="ArgumentException">Thrown if the <code>ControlDrawer</code> which should be set already has an other Control assigned</exception>
        public Progressbar(ControlDrawer<Progressbar> drawer = null)
            : base(drawer)
        {
            if (drawer == null)
                Drawer = new ProgressBarDrawer(this);
            this.Height = 1;
            this.IsHeightFixed = true;
        }

        /// <summary>
        /// Fires the ValueChanged-event and requests an redraw
        /// </summary>
        private void OnValueChanged()
        {
            RequestRedraw(RedrawRequestReason.ContentChanged);
            if (ValueChanged != null)
                ValueChanged(this, EventArgs.Empty);
        }
    }

    /// <summary>
    /// Draws a <code>Progressbar</code>-control
    /// </summary>
    public class ProgressBarDrawer : ControlDrawer<Progressbar>
    {
        /// <summary>
        /// The character which is used to draw the bar
        /// </summary>
        public char ProgressChar { get; set; }

        public ProgressBarDrawer(Progressbar control)
            : base(control)
        {
            ProgressChar = '|';
        }

        /// <summary>
        /// Draws the Progressbar given in the Control-Property
        /// </summary>
        public override void Draw()
        {
            base.Draw();

            int totalWidth = Control.Width - 2;
            int barWidth = (int)((totalWidth) * (_control.Value / 100f));
            string str = "[" + new string(ProgressChar, barWidth) + new string(' ', totalWidth - barWidth) + "]";

            FogConsole.Write(Boundary.Left, Boundary.Top, str, Boundary, Control.ForeColor, Control.BackColor);
        }
    }
}
