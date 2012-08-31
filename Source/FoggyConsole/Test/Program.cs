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
        private static Label lblStatus;

        static void Main(string[] args)
        {
            Console.CursorVisible = false;

            Application.DEBUG_MODE = true;

            var rootPanel = new Groupbox();
            rootPanel.Top = 3;
            rootPanel.Left = 3;
            rootPanel.Width = 70;
            rootPanel.Height = 17;
            rootPanel.Name = "rootPanel";
            rootPanel.Header = "I'm a header!";
            rootPanel.ForeColor = ConsoleColor.White;

            Button button0 = new Button("asd0") { Left = 1, Top = 1, Name = "asd0", TabIndex = 3 };
            Button button1 = new Button("asd1") { Left = 10, Top = 1, Name = "asd1", TabIndex =  2 };
            Button button2 = new Button("asd2") { Left = 15, Top = 2, Name = "asd2", TabIndex = 1 };
            lblStatus = new Label("") { Left = 15, Top = 0, Width = 40, Align = ContentAlign.Center, Name = "lblStatus" };
            Progressbar bar = new Progressbar();
            bar.Top = 3;
            bar.Left = 20;
            bar.Width = 15;
            bar.Value = 100;

            Checkbox cb0 = new Checkbox("foobar") { Left = 1, Top = 2, Name = "cb0" };
            rootPanel.Add(cb0);
            
            rootPanel.Add(button0);
            rootPanel.Add(button1);
            rootPanel.Add(button2);
            rootPanel.Add(bar);
            rootPanel.Add(lblStatus);

            Panel innerPanel1 = new Panel();
            innerPanel1.Top = 5;
            innerPanel1.Left = 5;
            innerPanel1.Width = 20;
            innerPanel1.Height = 20;
            innerPanel1.Name = "innerPanel1";

            Button button3 = new Button("asd3") { Left = 2, Top = 2, Name = "asd3" };
            innerPanel1.Add(button3);



            Panel innerPanel2 = new Panel();
            innerPanel2.Top = 4;
            innerPanel2.Left = 28;
            innerPanel2.Width = 45;
            innerPanel2.Height = 12;
            innerPanel2.Name = "innerPanel2";

            Button button4 = new Button("asd4") { Left = 2, Top = 2, Name = "asd4" };
            Button button5 = new Button("asd5") { Left = 4, Top = 3, Name = "asd5" };
            innerPanel2.Add(button4);
            innerPanel2.Add(button5);


            Panel innerPanel3 = new Panel();
            innerPanel3.Top = 5;
            innerPanel3.Left = 5;
            innerPanel3.Height = 5;
            innerPanel3.Width = 20;
            innerPanel3.Name = "innerInnerPanel3";
            Button button6 = new Button("asd6") { Left = 2, Top = 2, Name = "asd6" };
            innerPanel3.Add(button6);
            innerPanel2.Add(innerPanel3);

            Panel innerPanel4 = new Panel();
            innerPanel4.Top = 5;
            innerPanel4.Left = 25;
            innerPanel4.Height = 50;
            innerPanel4.Width = 40;
            innerPanel4.Name = "innerInnerPanel4";
            Button button7 = new Button("asd7") { Left = 2, Top = 2, Name = "asd7" };
            innerPanel4.Add(button7);
            innerPanel2.Add(innerPanel4);
            

            rootPanel.Add(innerPanel1);
            rootPanel.Add(innerPanel2);

            button0.Pressed += ButtonPressed;
            button1.Pressed += ButtonPressed;
            button2.Pressed += ButtonPressed;
            button3.Pressed += ButtonPressed;
            button4.Pressed += ButtonPressed;
            button5.Pressed += ButtonPressed;
            button6.Pressed += ButtonPressed;
            button7.Pressed += ButtonPressed;

            Application app = new Application(rootPanel);
            app.FocusManager = new FocusManager(cb0);
            app.Run();
        }

        private static void ButtonPressed(object sender, EventArgs eventArgs)
        {
            lblStatus.Text = "Button pressed: " + (sender as Control).Name;
            (sender as Control).Left++;
        }
    }
}
