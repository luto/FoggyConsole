﻿/*
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
    /// A <code>ContainerControl</code> which has a border.
    /// </summary>
    public class Groupbox : ContainerControl
    {
        private string _header;

        /// <summary>
        /// A description of the contents inside this Groupbox for the user
        /// </summary>
        public string Header
        {
            get { return _header; }
            set
            {
                if(value == null)
                    throw new ArgumentNullException();
                if (value.Any(c => c == '\n' || c == '\r'))
                    throw new ArgumentException("Header can't contain linefeeds or carriage returns.");

                var oldText = _header;
                _header = value;
                if (oldText != _header)
                    RequestRedraw(RedrawRequestReason.ContentChanged);
            }
        }

        /// <summary>
        /// Creates a new <code>Groupbox</code>
        /// </summary>
        /// <param name="drawer">The <code>ControlDrawer</code> to use. If null a new instance of <code>GroupboxDrawer</code> will be used.</param>
        /// <exception cref="ArgumentException">Thrown if the <code>ControlDrawer</code> which should be set already has an other Control assigned</exception>
        public Groupbox(ControlDrawer<Groupbox> drawer = null)
            : base(drawer)
        {
            if (drawer == null)
                base.Drawer = new GroupboxDrawer(this);
            Header = "";
        }
    }

    /// <summary>
    /// Draws a Groupbox
    /// </summary>
    public class GroupboxDrawer : ContainerControlDrawer<Groupbox>
    {
        /// <summary>
        /// The DrawCharacterSet which is used to draw this Groupbox
        /// </summary>
        public DrawCharacterSet CharacterSet { get; set; }

        public GroupboxDrawer(Groupbox control)
            : base(control)
        {
            CharacterSet = DrawCharacterSet.GetSingleLinesSet();
        }

        /// <summary>
        /// Draws the Groupbox given in the Control-Property
        /// </summary>
        public override void Draw()
        {
            base.Draw();

            FogConsole.DrawBox(Boundary, CharacterSet,
                               fColor: Control.ForeColor,
                               bColor: Control.BackColor,
                               fill: true);
            var headerBound = Boundary;
            headerBound.Width = headerBound.Width - 3;
            FogConsole.Write(Boundary.Left + 2, Boundary.Top, _control.Header, headerBound, Control.ForeColor, Control.BackColor);

            foreach (var control in _control)
            {
                control.Drawer.Draw();
            }
        }

        /// <summary>
        /// Calculates the boundary of all childen, only allows them to draw inside the border
        /// </summary>
        protected override void CalcuateChildBoundaries()
        {
            var innerBound = new Rectangle(Boundary.Left + 1, Boundary.Top + 1, Boundary.Height - 2, Boundary.Width - 2);
            foreach (var control in _control)
            {
                control.Drawer.CalculateBoundary(innerBound.Left, innerBound.Top, innerBound);
            }
        }
    }
}
