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
        /// sets the foreground color to <paramref name="fColor"/> (default: <code>System.ConsoleColor.Gray</code>) and
        /// the background color to <paramref name="bColor"/> (default: <code>System.ConsoleColor.Black</code>)
        /// </summary>
        /// <param name="left">Distance from the left edge of the window in characters</param>
        /// <param name="top">Distance from the top edge of the window in characters</param>
        /// <param name="o">The object to write</param>
        /// <param name="boundary">The boundary to draw in, nothing will be drawn outside this area</param>
        /// <param name="fColor">The foreground color to set</param>
        /// <param name="bColor">The background color to set</param>
        public static void Write(int left, int top, object o, Rectangle boundary = null,
                                 ConsoleColor fColor = ConsoleColor.Gray, ConsoleColor bColor = ConsoleColor.Black)
        {
            var str = o.ToString();
            if (boundary != null)
            {
                int lastCharLeft = left + str.Length;
                int lastAllowedCharLeft = boundary.Left + boundary.Width;

                if (left > lastAllowedCharLeft) // string is completely out of view
                    return;
                if (lastCharLeft > lastAllowedCharLeft) // string is partially out of view
                    str = str.Substring(0, boundary.Width - (left - boundary.Left));
            }

            // check for changed values, only set what is needed (huge performance plus)
            // Console.CursorLeft and Console.CursorTop get the other value and call
            // SetCursorPosition, which just sets both values directly.
            // That means that SetCursorPosition will always be faster,
            // up to 3,5x times as fast on my system.
            // See ConsoleBenchmark.cs for detailed results.
            if (left != _left || top != _top)
                Console.SetCursorPosition(left, top);
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

        /// <summary>
        /// Draws a box using the characters given in <paramref name="charSet"/>.
        /// The box will be filled with characters if <paramref name="fill"/> is true,
        /// this is a hugh performance plus if <paramref name="fColor"/> equals <paramref name="fFillColor"/> and <paramref name="bColor"/> equals <paramref name="bFillColor"/>.
        /// </summary>
        /// <param name="rect">The dimensions of the box</param>
        /// <param name="charSet">The characters to use to draw the box</param>
        /// <param name="boundary">The boundary to draw in, nothing will be drawn outside this area</param>
        /// <param name="fColor">The foreground color of the edges</param>
        /// <param name="bColor">The background color of the edges</param>
        /// <param name="fill">true if the box should be filled, otherwise false</param>
        /// <param name="fFillColor">The foreground color of the fill</param>
        /// <param name="bFillColor">The background color of the fill</param>
        public static void DrawBox(Rectangle rect, DrawCharacterSet charSet, Rectangle boundary = null,
                                   ConsoleColor fColor = ConsoleColor.Gray, ConsoleColor bColor = ConsoleColor.Black,
                                   bool fill = false, ConsoleColor fFillColor = ConsoleColor.Gray, ConsoleColor bFillColor = ConsoleColor.Black)
        {
            #region Corners
            var topLine = charSet.TopLeftCorner + new string(charSet.HorizontalEdge, rect.Width - 2) + charSet.TopRightCorner;
            var bottomLine = charSet.BottomLeftCorner + new string(charSet.HorizontalEdge, rect.Width - 2) + charSet.BottomRightCorner;

            if(boundary != null && topLine.Length + rect.Left > boundary.Left + boundary.Width)
            {
                var charsInside = boundary.Width - (rect.Left - boundary.Left);
                topLine = topLine.Substring(0, charsInside);
                bottomLine = bottomLine.Substring(0, charsInside);
            }

            Write(rect.Left, rect.Top, topLine, null, fColor, bColor);
            var bottomLineTop = rect.Top + rect.Height - 1;
            if(boundary == null || bottomLineTop < boundary.Top + boundary.Height)
                Write(rect.Left, bottomLineTop, bottomLine, null, fColor, bColor);
            #endregion

            #region Left and right edge
            // Drawing of left and right edges is optimized to call Console.Write as few as possible.
            // This is only possible when the box should be filled and all fill-colors equals the edge-colors.
            //     Fill = true
            //         Colors same:
            //             create a string which contains the whole line (edges and fill) => draw in one part
            //         Colors different:
            //             draw the line in 3 parts
            //             TODO: draw edges with dummy chars in between + draw the actual filling afterwards (=> 2 parts instead of 3)
            //     Fill = false
            //         draw the line in 3 parts (existing characters which could be inside the box have to be preserved)

            string middleLine;
            if (fColor == fFillColor && bColor == bFillColor)
                middleLine = charSet.VerticalEdge + new string(charSet.Empty, rect.Width - 2) + charSet.VerticalEdge;
            else
                middleLine = new string(charSet.Empty, rect.Width - 2);

            if (boundary != null &&
                middleLine.Length + rect.Left > boundary.Left + boundary.Width &&
                (fColor == fFillColor && bColor == bFillColor))
            {
                var charsInside = boundary.Width - (rect.Left - boundary.Left);
                middleLine = middleLine.Substring(0, charsInside);
            }

            var middleHeight = rect.Height - 1;
            if (boundary != null && rect.Top + middleHeight > boundary.Top + boundary.Height)
            {
                middleHeight = boundary.Height - (rect.Top - boundary.Top);
            }

            for (int i = 1; i < middleHeight; i++)
            {
                if (!fill || fColor != fFillColor || bColor == bFillColor)
                {
                    Write(rect.Left, rect.Top + i, charSet.VerticalEdge, null, fColor, bColor);
                    var left = rect.Left + rect.Width - 1;
                    if(boundary == null || left < boundary.Left + boundary.Width)
                        Write(left, rect.Top + i, charSet.VerticalEdge, null, fColor, bColor);
                }
                if(fill)
                {
                    if (fColor == fFillColor && bColor == bFillColor)
                    {
                        Write(rect.Left, rect.Top + i, middleLine, null, fColor, bColor);
                    }
                    else
                    {
                        Write(rect.Left + 1, rect.Top + i, middleLine, null, fFillColor, bFillColor);
                    }
                }
            }
            #endregion
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
