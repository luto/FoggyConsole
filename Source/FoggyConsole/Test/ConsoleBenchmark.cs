using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;


/* Results */

// iTerm2.app 
/*
    TestBColorSame (10000 samples).........................................0.0061ms
    TestBColorOther (10000 samples)........................................0.0057ms
    TestFColorSame (10000 samples).........................................0.0038ms
    TestFColorOther (10000 samples)........................................0.0019ms
    TestTopSame (10000 samples)............................................0.0026ms
    TestTopOther (10000 samples)...........................................0.0025ms
    TestLeftSame (10000 samples)...........................................0.0024ms
    TestLeftOther (10000 samples)..........................................0.0028ms
 */

// Terminal.app
/*
    TestBColorSame (10000 samples).........................................0.0061ms
    TestBColorOther (10000 samples)........................................0.0020ms
    TestFColorSame (10000 samples).........................................0.0019ms
    TestFColorOther (10000 samples)........................................0.0020ms
    TestTopSame (10000 samples)............................................0.0025ms
    TestTopOther (10000 samples)...........................................0.0025ms
    TestLeftSame (10000 samples)...........................................0.0025ms
    TestLeftOther (10000 samples)..........................................0.0025ms
 */

// CMD.exe
/*
    TestBColorSame (10000 samples).........................................0.1770ms
    TestBColorOther (10000 samples)........................................0.1743ms
    TestFColorSame (10000 samples).........................................0.1774ms
    TestFColorOther (10000 samples)........................................0.1747ms
    TestTopSame (10000 samples)............................................0.1977ms
    TestTopOther (10000 samples)...........................................0.1999ms
    TestLeftSame (10000 samples)...........................................0.1985ms
    TestLeftOther (10000 samples)..........................................0.1986ms
 */

// xterm (OS X)
/*
    TestBColorSame (10000 samples).........................................0.0058ms
    TestBColorOther (10000 samples)........................................0.0021ms
    TestFColorSame (10000 samples).........................................0.0019ms
    TestFColorOther (10000 samples)........................................0.0020ms
    TestTopSame (10000 samples)............................................0.0026ms
    TestTopOther (10000 samples)...........................................0.0025ms
    TestLeftSame (10000 samples)...........................................0.0025ms
    TestLeftOther (10000 samples)..........................................0.0025ms
 */

// ubuntu-terminal
/*
    TestBColorSame (10000 samples).........................................0.0104ms
    TestBColorOther (10000 samples)........................................0.0086ms
    TestFColorSame (10000 samples).........................................0.0074ms
    TestFColorOther (10000 samples)........................................0.0081ms
    TestTopSame (10000 samples)............................................0.0106ms
    TestTopOther (10000 samples)...........................................0.0099ms
    TestLeftSame (10000 samples)...........................................0.0120ms
    TestLeftOther (10000 samples)..........................................0.0115ms
 */


namespace FoggyConsole.Test
{
    public static class ConsoleBenchmark
    {
        private const int NUM = 10000;
        private static readonly Stopwatch stopwatch = new Stopwatch();
        private static int lastTop = 0;

        public static void TestAll()
        {
            ConsoleBenchmark.TestBColorSame();
            ConsoleBenchmark.TestBColorOther();
            ConsoleBenchmark.TestFColorSame();
            ConsoleBenchmark.TestFColorOther();
            ConsoleBenchmark.TestTopSame();
            ConsoleBenchmark.TestTopOther();
            ConsoleBenchmark.TestLeftSame();
            ConsoleBenchmark.TestLeftOther();
        }

        public static void TestBColorSame()
        {
            stopwatch.Start();
            for (int i = 0; i < NUM; i++)
            {
                Console.BackgroundColor = ConsoleColor.Black;
            }
            StopTest("TestBColorSame");
        }

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

        public static void TestFColorSame()
        {
            stopwatch.Start();
            for (int i = 0; i < NUM; i++)
            {
                Console.ForegroundColor = ConsoleColor.Black;
            }
            StopTest("TestFColorSame");
        }

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

        public static void TestTopSame()
        {
            stopwatch.Start();
            for (int i = 0; i < NUM; i++)
            {
                Console.CursorTop = 10;
            }
            StopTest("TestTopSame");
        }

        public static void TestTopOther()
        {
            stopwatch.Start();
            for (int i = 0; i < NUM / 2; i++)
            {
                Console.CursorTop = 10;
                Console.CursorTop = 20;
            }
            StopTest("TestTopOther");
        }

        public static void TestLeftSame()
        {
            stopwatch.Start();
            for (int i = 0; i < NUM; i++)
            {
                Console.CursorLeft = 10;
            }
            StopTest("TestLeftSame");
        }

        public static void TestLeftOther()
        {
            stopwatch.Start();
            for (int i = 0; i < NUM / 2; i++)
            {
                Console.CursorLeft = 10;
                Console.CursorLeft = 20;
            }
            StopTest("TestLeftOther");
        }


        private static void StopTest(string name)
        {
            stopwatch.Stop();
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.CursorTop = lastTop++;
            Console.CursorLeft = 0;
            var str1 = name + " (" + NUM + " samples)";
            var str2 = (stopwatch.ElapsedMilliseconds/Convert.ToSingle(NUM)).ToString("0.0000") + "ms";
            Console.WriteLine(str1 + new string('.', Console.WindowWidth - str1.Length - str2.Length - 1) + str2);
            stopwatch.Reset();
        }
    }
}
