using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FoggyConsole
{
    public class DrawCharacterSet
    {
        public char TopLeftCorner { get; set; }
        public char TopRightCorner { get; set; }
        public char BottomLeftCorner { get; set; }
        public char BottomRightCorner { get; set; }
        public char VerticalEdge { get; set; }
        public char HorizontalEdge { get; set; }
        public char ConnectionHorizontalUp { get; set; }
        public char ConnectionHorizontalDown { get; set; }
        public char ConnectionVerticalRight { get; set; }
        public char ConnectionVerticalLeft { get; set; }
        public char ConnectionCross { get; set; }
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
