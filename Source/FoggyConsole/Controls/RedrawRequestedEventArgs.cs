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
