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
using System.Diagnostics;


/* Results */

// iTerm2.app 
/*
    TestBColorSame (10000 samples).........................................0.0027ms
    TestBColorOther (10000 samples)........................................0.0052ms
    TestFColorSame (10000 samples).........................................0.0044ms
    TestFColorOther (10000 samples)........................................0.0020ms
    TestSetTopSame (10000 samples).........................................0.0058ms
    TestSetTopOther (10000 samples)........................................0.0061ms
    TestSetLeftSame (10000 samples)........................................0.0063ms
    TestSetLeftOther (10000 samples).......................................0.0056ms
    TestCursorPosSame (10000 samples)......................................0.0025ms
    TestCursorPosOther (10000 samples).....................................0.0059ms
    TestGetTop (10000 samples).............................................0.0000ms
    TestGetLeft (10000 samples)............................................0.0000ms
    Test1CharTitleSame (10000 samples).....................................0.0068ms
    Test1CharTitleOther (10000 samples)....................................1.3605ms
    Test100CharTitleSame (10000 samples)...................................0.0435ms
    Test100CharTitleOther (10000 samples)..................................2.8378ms
 */

// Terminal.app
/*
    TestBColorSame (10000 samples).........................................0.0022ms
    TestBColorOther (10000 samples)........................................0.0020ms
    TestFColorSame (10000 samples).........................................0.0019ms
    TestFColorOther (10000 samples)........................................0.0020ms
    TestSetTopSame (10000 samples).........................................0.0028ms
    TestSetTopOther (10000 samples)........................................0.0028ms
    TestSetLeftSame (10000 samples)........................................0.0027ms
    TestSetLeftOther (10000 samples).......................................0.0025ms
    TestCursorPosSame (10000 samples)......................................0.0025ms
    TestCursorPosOther (10000 samples).....................................0.0025ms
    TestGetTop (10000 samples).............................................0.0000ms
    TestGetLeft (10000 samples)............................................0.0000ms
    Test1CharTitleSame (10000 samples).....................................0.0308ms
    Test1CharTitleOther (10000 samples)....................................0.6702ms
    Test100CharTitleSame (10000 samples)...................................0.9044ms
    Test100CharTitleOther (10000 samples)..................................3.3262ms
 */

// CMD.exe
/*
    TestBColorSame (10000 samples).........................................0.1785ms
    TestBColorOther (10000 samples)........................................0.1797ms
    TestFColorSame (10000 samples).........................................0.1765ms
    TestFColorOther (10000 samples)........................................0.1781ms
    TestSetTopSame (10000 samples).........................................0.2076ms
    TestSetTopOther (10000 samples)........................................0.2064ms
    TestSetLeftSame (10000 samples)........................................0.2064ms
    TestSetLeftOther (10000 samples).......................................0.2062ms
    TestCursorPosSame (10000 samples)......................................0.1130ms
    TestCursorPosOther (10000 samples).....................................0.1141ms
    TestGetTop (10000 samples).............................................0.0861ms
    TestGetLeft (10000 samples)............................................0.0869ms
    Test1CharTitleSame (10000 samples).....................................0.8003ms
    Test1CharTitleOther (10000 samples)....................................0.9307ms
    Test100CharTitleSame (10000 samples)...................................1.0234ms
    Test100CharTitleOther (10000 samples)..................................1.1358ms
 */

// xterm (OS X)
/*
    TestBColorSame (10000 samples).........................................0.0023ms
    TestBColorOther (10000 samples)........................................0.0020ms
    TestFColorSame (10000 samples).........................................0.0019ms
    TestFColorOther (10000 samples)........................................0.0020ms
    TestSetTopSame (10000 samples).........................................0.0027ms
    TestSetTopOther (10000 samples)........................................0.0025ms
    TestSetLeftSame (10000 samples)........................................0.0025ms
    TestSetLeftOther (10000 samples).......................................0.0025ms
    TestCursorPosSame (10000 samples)......................................0.0025ms
    TestCursorPosOther (10000 samples).....................................0.0027ms
    TestGetTop (10000 samples).............................................0.0000ms
    TestGetLeft (10000 samples)............................................0.0000ms
    Test1CharTitleSame (10000 samples).....................................0.0016ms
    Test1CharTitleOther (10000 samples)....................................0.0060ms
    Test100CharTitleSame (10000 samples)...................................0.0081ms
    Test100CharTitleOther (10000 samples)..................................0.0131ms
 */

// ubuntu-terminal
/*
    TestBColorSame (10000 samples).........................................0.0080ms
    TestBColorOther (10000 samples)........................................0.0081ms
    TestFColorSame (10000 samples).........................................0.0078ms
    TestFColorOther (10000 samples)........................................0.0072ms
    TestSetTopSame (10000 samples).........................................0.0094ms
    TestSetTopOther (10000 samples)........................................0.0127ms
    TestSetLeftSame (10000 samples)........................................0.0115ms
    TestSetLeftOther (10000 samples).......................................0.0120ms
    TestCursorPosSame (10000 samples)......................................0.0103ms
    TestCursorPosOther (10000 samples).....................................0.0124ms
    TestGetTop (10000 samples).............................................0.0000ms
    TestGetLeft (10000 samples)............................................0.0000ms
    Test1CharTitleSame (10000 samples).....................................0.0151ms
    Test1CharTitleOther (10000 samples)....................................0.0098ms
    Test100CharTitleSame (10000 samples)...................................0.0755ms
    Test100CharTitleOther (10000 samples)..................................0.0776ms
 */


namespace FoggyConsole.Test
{
    /// <summary>
    /// Contains benchmarks for <code>System.Console</code>
    /// </summary>
    public static class ConsoleBenchmark
    {
        private const int NUM = 10000;
        private static readonly Stopwatch stopwatch = new Stopwatch();
        private static int lastTop = 0;

        /// <summary>
        /// Executes all tests
        /// </summary>
        public static void TestAll()
        {
            Console.Clear();
            ConsoleBenchmark.TestBColorSame();
            ConsoleBenchmark.TestBColorOther();
            ConsoleBenchmark.TestFColorSame();
            ConsoleBenchmark.TestFColorOther();
            ConsoleBenchmark.TestSetTopSame();
            ConsoleBenchmark.TestSetTopOther();
            ConsoleBenchmark.TestSetLeftSame();
            ConsoleBenchmark.TestSetLeftOther();
            ConsoleBenchmark.TestCursorPosSame();
            ConsoleBenchmark.TestCursorPosOther();
            ConsoleBenchmark.TestGetTop();
            ConsoleBenchmark.TestGetLeft();
            ConsoleBenchmark.Test1CharTitleSame();
            ConsoleBenchmark.Test1CharTitleOther();
            ConsoleBenchmark.Test100CharTitleSame();
            ConsoleBenchmark.Test100CharTitleOther();
            Console.Write("\nFinished.");
        }

        /// <summary>
        /// Tests <code>Console.BackgroundColor</code> by setting the same value over and over again
        /// </summary>
        public static void TestBColorSame()
        {
            stopwatch.Start();
            for (int i = 0; i < NUM; i++)
            {
                Console.BackgroundColor = ConsoleColor.Black;
            }
            StopTest("TestBColorSame");
        }

        /// <summary>
        /// Tests <code>Console.BackgroundColor</code> by setting different values
        /// </summary>
        public static void TestBColorOther()
        {
            stopwatch.Start();
            for (int i = 0; i < NUM / 2; i++)
            {
                Console.BackgroundColor = ConsoleColor.Black;
                Console.BackgroundColor = ConsoleColor.White;
            }
            StopTest("TestBColorOther");
        }

        /// <summary>
        /// Tests <code>Console.ForegroundColor</code> by setting the same value over and over again
        /// </summary>
        public static void TestFColorSame()
        {
            stopwatch.Start();
            for (int i = 0; i < NUM; i++)
            {
                Console.ForegroundColor = ConsoleColor.Black;
            }
            StopTest("TestFColorSame");
        }

        /// <summary>
        /// Tests <code>Console.ForegroundColor</code> by setting different values
        /// </summary>
        public static void TestFColorOther()
        {
            stopwatch.Start();
            for (int i = 0; i < NUM / 2; i++)
            {
                Console.ForegroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.White;
            }
            StopTest("TestFColorOther");
        }

        /// <summary>
        /// Tests <code>Console.CursorTop</code> by setting the same value over and over again
        /// </summary>
        public static void TestSetTopSame()
        {
            stopwatch.Start();
            for (int i = 0; i < NUM; i++)
            {
                Console.CursorTop = 10;
            }
            StopTest("TestSetTopSame");
        }

        /// <summary>
        /// Tests <code>Console.CursorTop</code> by setting different values
        /// </summary>
        public static void TestSetTopOther()
        {
            stopwatch.Start();
            for (int i = 0; i < NUM / 2; i++)
            {
                Console.CursorTop = 10;
                Console.CursorTop = 20;
            }
            StopTest("TestSetTopOther");
        }

        /// <summary>
        /// Tests <code>Console.CursorLeft</code> by setting the same value over and over again
        /// </summary>
        public static void TestSetLeftSame()
        {
            stopwatch.Start();
            for (int i = 0; i < NUM; i++)
            {
                Console.CursorLeft = 10;
            }
            StopTest("TestSetLeftSame");
        }

        /// <summary>
        /// Tests <code>Console.CursorLeft</code> by setting different values
        /// </summary>
        public static void TestSetLeftOther()
        {
            stopwatch.Start();
            for (int i = 0; i < NUM / 2; i++)
            {
                Console.CursorLeft = 10;
                Console.CursorLeft = 20;
            }
            StopTest("TestSetLeftOther");
        }

        /// <summary>
        /// Tests <code>Console.SetCursorPosition</code> by setting the same value over and over again
        /// </summary>
        public static void TestCursorPosSame()
        {
            stopwatch.Start();
            for (int i = 0; i < NUM; i++)
            {
                Console.SetCursorPosition(10, 10);
            }
            StopTest("TestCursorPosSame");
        }

        /// <summary>
        /// Tests <code>Console.SetCursorPosition</code> by setting different values
        /// </summary>
        public static void TestCursorPosOther()
        {
            stopwatch.Start();
            for (int i = 0; i < NUM / 2; i++)
            {
                Console.SetCursorPosition(10, 10);
                Console.SetCursorPosition(20, 20);
            }
            StopTest("TestCursorPosOther");
        }

        /// <summary>
        /// Tests <code>Console.CursorTop</code> by getting the value
        /// </summary>
        public static void TestGetTop()
        {
            int tt = 0;
            stopwatch.Start();
            for (int i = 0; i < NUM; i++)
            {
                tt = Console.CursorTop;
            }
            StopTest("TestGetTop");
        }

        /// <summary>
        /// Tests <code>Console.CursorLeft</code> by getting the value
        /// </summary>
        public static void TestGetLeft()
        {
            int tt = 0;
            stopwatch.Start();
            for (int i = 0; i < NUM; i++)
            {
                tt = Console.CursorLeft;
            }
            StopTest("TestGetLeft");
        }

        /// <summary>
        /// Tests <code>Console.Title</code> by setting the same value over and over again
        /// </summary>
        public static void Test1CharTitleSame()
        {
            stopwatch.Start();
            for (int i = 0; i < NUM; i++)
            {
                Console.Title = "a";
            }
            StopTest("Test1CharTitleSame");
        }

        /// <summary>
        /// Tests <code>Console.Title</code> by setting different values
        /// </summary>
        public static void Test1CharTitleOther()
        {
            stopwatch.Start();
            for (int i = 0; i < NUM / 2; i++)
            {
                Console.Title = "b";
                Console.Title = "a";
            }
            StopTest("Test1CharTitleOther");
        }

        /// <summary>
        /// Tests <code>Console.Title</code> by setting the same value over and over again
        /// </summary>
        public static void Test100CharTitleSame()
        {
            stopwatch.Start();
            for (int i = 0; i < NUM; i++)
            {
                Console.Title = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
            }
            StopTest("Test100CharTitleSame");
        }

        /// <summary>
        /// Tests <code>Console.Title</code> by setting different values
        /// </summary>
        public static void Test100CharTitleOther()
        {
            stopwatch.Start();
            for (int i = 0; i < NUM / 2; i++)
            {
                Console.Title = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
                Console.Title = "bbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbb";
            }
            StopTest("Test100CharTitleOther");
        }


        private static void StopTest(string name)
        {
            stopwatch.Stop();
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
            Console.CursorTop = lastTop++;
            Console.CursorLeft = 0;
            var str1 = name + " (" + NUM + " samples)";
            var str2 = (stopwatch.ElapsedMilliseconds/Convert.ToSingle(NUM)).ToString("0.0000") + "ms";
            Console.WriteLine(str1 + new string('.', Console.WindowWidth - str1.Length - str2.Length - 1) + str2);
            stopwatch.Reset();
        }
    }
}
