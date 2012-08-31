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
    /// Contains further information about a Redraw-Request
    /// </summary>
    public class RedrawRequestedEventArgs : EventArgs
    {
        /// <summary>
        /// The reason for the Redraw-Request
        /// </summary>
        public RedrawRequestReason Reason { get; private set; }

        /// <summary>
        /// Creates a new <code>RedrawRequestedEventArgs</code>
        /// </summary>
        /// <param name="reason">The reason to store</param>
        public RedrawRequestedEventArgs(RedrawRequestReason reason)
        {
            this.Reason = reason;
        }
    }

    /// <summary>
    /// The reason for a Redraw-Request
    /// </summary>
    public enum RedrawRequestReason
    {
        /// <summary>
        /// The control became smaller
        /// </summary>
        BecameSmaller,
        /// <summary>
        /// The control became bigger
        /// </summary>
        BecameBigger,
        /// <summary>
        /// The control has been moved
        /// </summary>
        Moved,
        /// <summary>
        /// The control-content changed
        /// </summary>
        ContentChanged
    }
}
