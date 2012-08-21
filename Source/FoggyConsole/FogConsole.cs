using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FoggyConsole
{
    /// <summary>
    /// Abstracts all calls on the <code>System.Console</code> class.
    /// Ensures that expensive operations like setting <code>Console.CursorLeft</code> or <code>Console.ForegroundColor</code> are only executed if neccessary.
    /// </summary>
    internal static class FogConsole
    {
        // these variables store the last set values
        // they're used to check if the requested value needs to be set
        private static int _left;
        private static int _top;
        private static ConsoleColor _fColor;
        private static ConsoleColor _bColor;

        /// <summary>
        /// Writes <paramref name="o"/> at (<paramref name="left"/>|<paramref name="top"/>)
        /// sets the foreground color to <paramref name="fColor"/> (default: <code>System.ConsoleColor.Gray</code>) and the background color to <paramref name="bColor"/> (default: <code>System.ConsoleColor.Black</code>)
        /// </summary>
        /// <param name="left">Distance from the left edge of the window in characters</param>
        /// <param name="top">Distance from the top edge of the window in characters</param>
        /// <param name="o">The object to write</param>
        /// <param name="boundary">The boundary to draw in, nothing will be drawn outside this area</param>
        /// <param name="fColor">The foreground color to set</param>
        /// <param name="bColor">The background color to set</param>
        public static void Write(int left, int top, object o, Rectangle boundary = null, ConsoleColor fColor = ConsoleColor.Gray, ConsoleColor bColor = ConsoleColor.Black)
        {
            var str = o.ToString();
            if (boundary != null)
            {
                int lastCharLeft = left + str.Length;
                int lastAllowedCharLeft = boundary.Left + boundary.Width;

                if (left > lastAllowedCharLeft) // string is complitly out of view
                    return;
                if (lastCharLeft > lastAllowedCharLeft) // string is partially out of view
                    str = str.Substring(0, boundary.Width - (left - boundary.Left));
            }

            // check for changed values, only set what is needed (huge performance plus)
            if (left != _left)
                Console.CursorLeft = left;
            if (top != _top)
                Console.CursorTop = top;
            if (fColor != _fColor)
                Console.ForegroundColor = fColor;
            if (bColor != _bColor)
                Console.BackgroundColor = bColor;

            Console.Write(str);

            // remember the set values
            _left = left;
            // Console.Write moves the cursor to the end of the written text,
            // so have have to adjust our saved value
            _left += str.Length;
            _top = top;
            _fColor = fColor;
            _bColor = bColor;
        }
    }

    /// <summary>
    /// A very basic represenation of a rectangle
    /// </summary>
    public class Rectangle
    {
        /// <summary>
        /// This rectangles distance from the left edge in characters
        /// </summary>
        public int Left { get; set; }
        /// <summary>
        /// This rectangles distance from the top edge in characters
        /// </summary>
        public int Top { get; set; }
        /// <summary>
        /// This rectangles height in characters
        /// </summary>
        public int Height { get; set; }
        /// <summary>
        /// This rectangles width in characters
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// Creates a new rectangle
        /// </summary>
        /// <param name="left">The left value to set</param>
        /// <param name="top">The top value to set</param>
        /// <param name="height">The height value to set</param>
        /// <param name="width">The width value to set</param>
        public Rectangle(int left, int top, int height, int width)
        {
            this.Left = left;
            this.Top = top;
            this.Height = height;
            this.Width = width;
        }
    }
}
