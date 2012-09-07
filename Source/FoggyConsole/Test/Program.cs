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

            var txtLeftText = new Textbox();
            txtLeftText.Text = "asd123";
            txtLeftText.Top = 3;
            txtLeftText.Left = 21;
            txtLeftText.Width = 10;
            txtLeftText.BackColor = ConsoleColor.Gray;
            txtLeftText.ForeColor = ConsoleColor.Black;
            txtLeftText.Name = "txtLeftText";

            var btnLeftSetText = new Button("Set this text =>");
            btnLeftSetText.Top = 3;
            btnLeftSetText.Name = "btnLeftSetText";
            btnLeftSetText.Pressed += (sender, eventArgs) => lblLeftStatusText.Text = txtLeftText.Text;

            var chkLeftMoveDir = new Checkbox("Move button up");
            chkLeftMoveDir.Top = 5;
            chkLeftMoveDir.Left = 12;
            chkLeftMoveDir.Name = "chkLeftMoveDir";
            chkLeftMoveDir.Checked = CheckState.Unchecked;

            var prgLeftMoveButton = new Progressbar();
            prgLeftMoveButton.Top = 6;
            prgLeftMoveButton.Left = 12;
            prgLeftMoveButton.Name = "prgLeftMoveButton";
            prgLeftMoveButton.Width = chkLeftMoveDir.Text.Length + 4;
            prgLeftMoveButton.Value = 10;

            var lblLeftPrgDesc = new Label(@"^ Progressbar ^");
            lblLeftPrgDesc.Top = 7;
            lblLeftPrgDesc.Left = 14;
            lblLeftPrgDesc.Name = "lblLeftPrgDesc";

            var myPlayground = new Playground(7, 14);
            myPlayground.Name = "playground";
            myPlayground.Top = 9;
            myPlayground.Left = 14;
            myPlayground.BackColor = ConsoleColor.Blue;
            myPlayground.ForeColor = ConsoleColor.Red;
            myPlayground[0, 0] = 'x';
            myPlayground[2, 2] = 'x';
            myPlayground[4, 4] = 'x';
            myPlayground[6, 6] = 'o';
            myPlayground[4, 8] = 'x';
            myPlayground[2, 10] = 'x';
            myPlayground[0, 12] = 'x';
            
            var btnLeftMove = new Button("Move");
            btnLeftMove.Top = 4;
            btnLeftMove.Name = "btnLeftMove";
            btnLeftMove.Pressed += (sender, eventArgs) =>
                {
                    var mv = chkLeftMoveDir.Checked == CheckState.Checked ? -1 : 1;
                    if(btnLeftMove.Top + mv < btnLeftMove.Container.Height - 3 &&
                       btnLeftMove.Top + mv > 3)
                        btnLeftMove.Top += mv;
                    prgLeftMoveButton.Value = (int)(((btnLeftMove.Top - 2) / (float)(btnLeftMove.Container.Height - 6)) * 100);
                };



            leftBox.Add(lblLeftStatus);
            leftBox.Add(lblLeftStatusText);
            leftBox.Add(btnLeftIncrement);
            leftBox.Add(btnLeftClear);
            leftBox.Add(btnLeftSetText);
            leftBox.Add(txtLeftText);
            leftBox.Add(btnLeftMove);
            leftBox.Add(chkLeftMoveDir);
            leftBox.Add(prgLeftMoveButton);
            leftBox.Add(lblLeftPrgDesc);
            leftBox.Add(myPlayground);
            #endregion

            #region Right Side
            var rightBox = new Groupbox();
            rightBox.Name = "rightBox";
            rightBox.Header = "Right Box";
            rightBox.Top = 0;
            rightBox.Left = mainPanel.Width / 2;
            rightBox.Width = mainPanel.Width / 2;
            rightBox.Height = mainPanel.Height;

            var rightRightBox = new Groupbox();
            rightRightBox.Name = "rightRightBox";
            rightRightBox.Header = "Checkbox!";
            rightRightBox.Top = 1;
            rightRightBox.Width = rightBox.Width / 2 - 2;
            rightRightBox.Height = 8;
            for (int i = 0; i < 4; i++)
                rightRightBox.Add(new Checkbox("Foo" + i) { Name = "foo" + i, Top = i });

            var rightLeftBox = new Groupbox();
            rightLeftBox.Name = "rightLeftBox";
            rightLeftBox.Header = "Radiobutton!";
            rightLeftBox.Top = 1;
            rightLeftBox.Left = rightBox.Width / 2 - 2;
            rightLeftBox.Width = rightBox.Width / 2 + 2;
            rightLeftBox.Height = 8;
            for (int i = 0; i < 4; i++)
                rightLeftBox.Add(new RadioButton("Bar" + i) { Name = "bar" + i, Top = i });

            var lblRightRadioDesc = new Label("Radiobuttons can have groups:");
            lblRightRadioDesc.Top = rightLeftBox.Height + 2;
            lblRightRadioDesc.Name = "lblDesc";
            for (int i = 0; i < 6; i++)
                rightBox.Add(new RadioButton("asd" + i) { Text = "Group" + (i / 3),
                                                          Name = "chk" + i.ToString(),
                                                          Top = (i % 3) + rightRightBox.Height + 3,
                                                          Left = (i / 3) * 15 + 2,
                                                          ComboboxGroup = "grp" + (i / 3) });

            rightBox.Add(rightRightBox);
            rightBox.Add(rightLeftBox);
            rightBox.Add(lblRightRadioDesc);
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
