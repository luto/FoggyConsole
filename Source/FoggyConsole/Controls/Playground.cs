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
    // TODO: don't redraw the whole thing all the time
    // TODO: add methods for easy from-drawing
    // TODO: entities?
    // TODO: make Height and Width changeable

    /// <summary>
    /// A class which draws the contents of an two-dimentional char-array.
    /// This can be used to draw a small game or display graphs.
    /// </summary>
    public class Playground : Control
    {
        private char[,] _field;

        /// <summary>
        /// Gets or sets the char at (<paramref name="top"/>|<paramref name="left"/>).
        /// Setting triggers an redraw if <code>Playground.AutoRedaw</code> is true.
        /// </summary>
        /// <param name="top"></param>
        /// <param name="left"></param>
        /// <returns></returns>
        /// <seealso cref="AutoRedraw"/>
        public char this[int top, int left]
        {
            get { return _field[top, left]; }
            set
            {
                if (top > _field.GetLength(0))
                    throw new ArgumentOutOfRangeException("top");
                if (left > _field.GetLength(1))
                    throw new ArgumentOutOfRangeException("left");

                _field[top, left] = value;
                if (AutoRedraw) Redraw();
            }
        }

        /// <summary>
        /// True if a redraw should be triggered on every change
        /// </summary>
        /// <seealso cref="Redraw"/>
        public bool AutoRedraw { get; set; }

        /// <summary>
        /// Creates a new <code>Playground</code>
        /// </summary>
        /// <param name="height">The height of this Playground</param>
        /// <param name="width">The width of this Playground</param>
        /// <param name="drawer">The <code>ControlDrawer</code> to use. If null a new instance of <code>PlaygroundDrawer</code> will be used.</param>
        /// <exception cref="ArgumentException">Thrown if the <code>ControlDrawer</code> which should be set already has an other Control assigned</exception>
        public Playground(int height, int width, ControlDrawer<Playground> drawer = null)
            : base(drawer)
        {
            if(drawer == null)
                Drawer = new PlaygroundDrawer(this);

            this.Height = height;
            base.IsHeightFixed = true;
            this.Width = width;
            base.IsWidthFixed = true;

            _field = new char[height, width];
            AutoRedraw = true;
        }

        /// <summary>
        /// Triggers an redraw
        /// </summary>
        /// <seealso cref="AutoRedraw"/>
        public void Redraw()
        {
            RequestRedraw(RedrawRequestReason.ContentChanged);
        }
    }

    /// <summary>
    /// Draws an <code>Playground</code>
    /// </summary>
    public class PlaygroundDrawer : ControlDrawer<Playground>
    {
        public PlaygroundDrawer(Playground control)
            : base(control)
        {
        }

        /// <summary>
        /// Draws all characters within the given playground
        /// </summary>
        public override void Draw()
        {
            base.Draw();

            for (int i = 0; i < _control.Height; i++)
            {
                var cc = new char[_control.Width];
                for (int j = 0; j < _control.Width; j++)
                    cc[j] = _control[i, j];

                FogConsole.Write(Boundary.Left,
                                    Boundary.Top + i,
                                    new string(cc),
                                    Boundary,
                                    _control.ForeColor, _control.BackColor);
            }
        }
    }
}
