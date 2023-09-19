using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Calculator
{
    public partial class Form1 : Form
    {
        public struct BtnStruct
        {
            public char content;
            public bool isBold;
            public bool isNumber;
            public BtnStruct(char c, bool b=false, bool n=false) 
            {
                this.content = c;
                this.isBold = b;
                this.isNumber = n;
            }
        }
        private BtnStruct[,] buttons =
        {
            {new BtnStruct('%', false), new BtnStruct('\u0152', false), new BtnStruct('C', false), new BtnStruct('\u232b', false) },
            {new BtnStruct('\u215f', false), new BtnStruct('\u00b2', false), new BtnStruct('\u221a', false), new BtnStruct('\u00f7', false) },
            {new BtnStruct('7', true, true), new BtnStruct('8', true, true), new BtnStruct('9', true, true), new BtnStruct('\u00d7', false) },
            {new BtnStruct('4', true, true), new BtnStruct('5', true, true), new BtnStruct('6', true, true), new BtnStruct('-', false) },
            {new BtnStruct('1', true, true), new BtnStruct('2', true, true), new BtnStruct('3', true, true), new BtnStruct('+', false) },
            {new BtnStruct('\u00b1', true), new BtnStruct('0', true, true), new BtnStruct(',', true), new BtnStruct('=', false) }
        };
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            madeButtons(buttons.GetLength(0), buttons.GetLength(1));  
        }

        private void madeButtons(int rows, int cols)
        {
            int btnWidth = 80;
            int btnHeight = 60;
            int posX = 0;
            int posY = 116;
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    Button myButton = new Button();
                    FontStyle fs = buttons[i, j].isBold ? FontStyle.Bold : FontStyle.Regular;
                    myButton.Font = new Font("Segoe UI", 16, fs);
                    myButton.BackColor = buttons[i, j].isBold ? Color.White : Color.Transparent;
                    myButton.Text = buttons[i, j].content.ToString();
                    myButton.Width = btnWidth;
                    myButton.Height = btnHeight;
                    myButton.Top = posY;
                    myButton.Left = posX;
                    myButton.Tag = buttons[i, j];
                    myButton.Click += MyButton_Click;
                    this.Controls.Add(myButton);
                    posX += myButton.Width;
                }
                posX = 0;
                posY += btnHeight;
            }
        }

        private void MyButton_Click(object sender, EventArgs e)
        {
            Button clickedButton = (Button)sender;
            BtnStruct clickedButtonStruct = (BtnStruct)clickedButton.Tag;
            if (clickedButtonStruct.isNumber) 
            {
                lblResult.Text += clickedButton.Text;
            }
        }
    }
}
