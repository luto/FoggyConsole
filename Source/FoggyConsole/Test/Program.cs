using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using FoggyConsole;
using FoggyConsole.Controls;

namespace FoggyConsole.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.CursorVisible = false;

            Panel rootPanel = new Panel();
            rootPanel.Top = 3;
            rootPanel.Left = 3;
            rootPanel.Width = 10;
            rootPanel.Height = 10;

            Button button0 = new Button("asd0") { Left = 1, Top = 1 };
            Button button1 = new Button("asd1") { Left = 10, Top = 1 };
            Button button2 = new Button("asd2") { Left = 15, Top = 1 };
            
            rootPanel.Add(button0);
            rootPanel.Add(button1);
            rootPanel.Add(button2);

            Application app = new Application(rootPanel);
            app.Run();
        }
    }
}
