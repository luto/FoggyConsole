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
        private static Label lblLeftStatusText;

        static void Main(string[] args)
        {
            var mainPanel = new Panel();
            mainPanel.Name = "mainPanle";
            mainPanel.Width = Application.STANDARD_ROOT_BOUNDARY.Width - 12;
            mainPanel.Height = Application.STANDARD_ROOT_BOUNDARY.Height - 6;
            mainPanel.Top = 3;
            mainPanel.Left = 6;

            #region Left Side
            var leftBox = new Groupbox();
            leftBox.Name = "leftBox";
            leftBox.Header = "Left Box";
            leftBox.Top = leftBox.Left = 0;
            leftBox.Width = mainPanel.Width / 2;
            leftBox.Height = mainPanel.Height;

            var lblLeftStatus = new Label("Status:");
            lblLeftStatus.Name = "lblLeftStatus";

            lblLeftStatusText = new Label();
            lblLeftStatusText.Name = "lblLeftStatusText";
            lblLeftStatusText.Left = lblLeftStatus.Text.Length;
            lblLeftStatusText.Width = leftBox.Width - lblLeftStatus.Width;
            lblLeftStatusText.Align = ContentAlign.Right;
            
            var btnLeftIncrement = new Button("Increment");
            btnLeftIncrement.Top = 1;
            btnLeftIncrement.Name = "btnLeftIncrement";
            btnLeftIncrement.Pressed += BtnLeftIncrementOnPressed;

            var btnLeftClear = new Button("Clear");
            btnLeftClear.Top = 2;
            btnLeftClear.Name = "btnLeftClear";
            btnLeftClear.Pressed += (sender, eventArgs) => lblLeftStatusText.Text = "";

            var btnLeftMove = new Button("Move");
            btnLeftMove.Top = 3;
            btnLeftMove.Name = "btnLeftMove";
            btnLeftMove.Pressed += (sender, eventArgs) => btnLeftMove.Top++;


            leftBox.Add(lblLeftStatus);
            leftBox.Add(lblLeftStatusText);
            leftBox.Add(btnLeftIncrement);
            leftBox.Add(btnLeftClear);
            leftBox.Add(btnLeftMove);
            #endregion

            #region Right Side
            var rightBox = new Groupbox();
            rightBox.Name = "rightBox";
            rightBox.Header = "Right Box";
            rightBox.Top = 0;
            rightBox.Left = mainPanel.Width / 2;
            rightBox.Width = mainPanel.Width / 2;
            rightBox.Height = mainPanel.Height;
            #endregion

            mainPanel.Add(leftBox);
            mainPanel.Add(rightBox);

            var app = new Application(mainPanel);
            app.FocusManager = new FocusManager(mainPanel, btnLeftIncrement);
            app.Name = "FoggyConsole";
            app.Run();
        }

        private static void BtnLeftIncrementOnPressed(object sender, EventArgs eventArgs)
        {
            int i = 0;
            int.TryParse(lblLeftStatusText.Text, out i);
            i++;
            lblLeftStatusText.Text = i.ToString();
        }
    }
}
