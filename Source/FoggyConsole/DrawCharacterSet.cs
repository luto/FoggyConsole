using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FoggyConsole
{
    /// <summary>
    /// A set of characters which can be used to draw a box or complex lines
    /// </summary>
    public class DrawCharacterSet
    {
        /// <summary>
        /// A character representing the top left corner of an rectangle
        /// </summary>
        public char TopLeftCorner { get; set; }
        /// <summary>
        /// A character representing the top right corner of an rectangle
        /// </summary>
        public char TopRightCorner { get; set; }
        /// <summary>
        /// A character representing the bottom left corner of an rectangle
        /// </summary>
        public char BottomLeftCorner { get; set; }
        /// <summary>
        /// A character representing the bottom right corner of an rectangle
        /// </summary>
        public char BottomRightCorner { get; set; }
        /// <summary>
        /// A character representing the a left or right edge of an rectangle
        /// </summary>
        public char VerticalEdge { get; set; }
        /// <summary>
        /// A character representing the a top or bottom edge of an rectangle
        /// </summary>
        public char HorizontalEdge { get; set; }
        /// <summary>
        /// A character representing a connection between a <code>HorizontalEdge</code> and a <code>VerticalEdge</code>,
        /// in which the <code>VerticalEdge</code> is above the <code>HorizontalEdge</code>
        /// </summary>
        public char ConnectionHorizontalUp { get; set; }
        /// <summary>
        /// A character representing a connection between a <code>HorizontalEdge</code> and a <code>VerticalEdge</code>,
        /// in which the <code>VerticalEdge</code> is below the <code>HorizontalEdge</code>
        /// </summary>
        public char ConnectionHorizontalDown { get; set; }
        /// <summary>
        /// A character representing a connection between a <code>VerticalEdge</code> and a <code>HorizontalEdge</code>,
        /// in which the <code>HorizontalEdge</code> is on the left side of the <code>VerticalEdge</code>
        /// </summary>
        public char ConnectionVerticalRight { get; set; }
        /// <summary>
        /// A character representing a connection between a <code>VerticalEdge</code> and a <code>HorizontalEdge</code>,
        /// in which the <code>HorizontalEdge</code> is on the right side of the <code>VerticalEdge</code>
        /// </summary>
        public char ConnectionVerticalLeft { get; set; }
        /// <summary>
        /// A characer representing a connection between two <code>VerticalEdge</code> and two <code>HorizontalEdge</code>
        /// </summary>
        public char ConnectionCross { get; set; }
        /// <summary>
        /// A empty character which can be used to fill boxes
        /// </summary>
        public char Empty { get; set; }


        /// <summary>
        /// Creates a <code>CharacterSet</code> in which all characters are set to space (' ')
        /// </summary>
        public DrawCharacterSet()
        {
            var emptyChar = ' ';
            TopLeftCorner = emptyChar;
            TopRightCorner = emptyChar;
            BottomLeftCorner = emptyChar;
            BottomRightCorner = emptyChar;
            VerticalEdge = emptyChar;
            HorizontalEdge = emptyChar;
            ConnectionHorizontalUp = emptyChar;
            ConnectionHorizontalDown = emptyChar;
            ConnectionVerticalRight = emptyChar;
            ConnectionVerticalLeft = emptyChar;
            ConnectionCross = emptyChar;
            Empty = emptyChar;
        }


        /// <summary>
        /// Creates a very simple <code>DrawCharacterSet</code>.
        /// </summary>
        /// <returns></returns>
        public static DrawCharacterSet GetSimpleSet()
        {
            var set = new DrawCharacterSet
            {
                TopLeftCorner            = '.',
                TopRightCorner           = '.',
                BottomLeftCorner         = '`',
                BottomRightCorner        = '´',
                VerticalEdge             = '|',
                HorizontalEdge           = '-',
                ConnectionHorizontalUp   = '+',
                ConnectionHorizontalDown = '+',
                ConnectionVerticalRight  = '+',
                ConnectionVerticalLeft   = '+',
                ConnectionCross          = '+',
                Empty                    = ' '
            };
            return set;
        }
        
        /// <summary>
        /// Creates a <code>DrawCharacterSet</code> which uses single-lined box-drawing-characters
        /// </summary>
        /// <returns></returns>
        public static DrawCharacterSet GetSingleLinesSet()
        {
            var set = new DrawCharacterSet
                {
                    TopLeftCorner            = '\u250C', // ┌
                    TopRightCorner           = '\u2510', // ┐
                    BottomLeftCorner         = '\u2514', // └
                    BottomRightCorner        = '\u2518', // ┘
                    VerticalEdge             = '\u2502', // │
                    HorizontalEdge           = '\u2500', // ─
                    ConnectionHorizontalUp   = '\u2534', // ┴
                    ConnectionHorizontalDown = '\u252C', // ┬
                    ConnectionVerticalRight  = '\u251C', // ├
                    ConnectionVerticalLeft   = '\u2524', // ┤
                    ConnectionCross          = '\u253C', // ┼
                    Empty                    = ' '
                };
            return set;
        }

        /// <summary>
        /// Creates a <code>DrawCharacterSet</code> which uses double-lined box-drawing-characters
        /// </summary>
        /// <returns></returns>
        public static DrawCharacterSet GetDoubleLinesSet()
        {
            var set = new DrawCharacterSet
                {
                    TopLeftCorner            = '\u2554', // ╔
                    TopRightCorner           = '\u2557', // ╗
                    BottomLeftCorner         = '\u255A', // ╚
                    BottomRightCorner        = '\u255D', // ╝
                    VerticalEdge             = '\u2551', // ║
                    HorizontalEdge           = '\u2550', // ═
                    ConnectionHorizontalUp   = '\u2569', // ╩
                    ConnectionHorizontalDown = '\u2566', // ╦
                    ConnectionVerticalRight  = '\u2560', // ╠
                    ConnectionVerticalLeft   = '\u2563', // ╣
                    ConnectionCross          = '\u256C', // ╬
                    Empty                    = ' '
                };
            return set;
        }
    }
}
