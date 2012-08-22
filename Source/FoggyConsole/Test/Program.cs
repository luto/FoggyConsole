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
            rootPanel.Width = 50;
            rootPanel.Height = 17;

            Button button0 = new Button("asd0") { Left = 1, Top = 1 };
            Button button1 = new Button("asd1") { Left = 10, Top = 1 };
            Button button2 = new Button("asd2") { Left = 15, Top = 2 };
            
            rootPanel.Add(button0);
            rootPanel.Add(button1);
            rootPanel.Add(button2);

            Panel innerPanel = new Panel();
            innerPanel.Top = 5;
            innerPanel.Left = 5;
            innerPanel.Width = 20;
            innerPanel.Height = 10;

            Button button3 = new Button("asd3") { Left = 2, Top = 2 };
            innerPanel.Add(button3);

            rootPanel.Add(innerPanel);

            Application app = new Application(rootPanel);
            app.Run();
        }
    }
}
