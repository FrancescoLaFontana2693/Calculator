using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Calculator
{
    public partial class Form1 : Form
    {
        public enum SymbolType
        {
            Number,
            Operator,
            specialOperator,
            DecimalPoint,
            PlusMinusSign,
            BackSpace,
            ClearAll,
            ClearEntry,
            Undefined
        }
        public struct BtnStruct
        {
            public char Content;
            public SymbolType Type;
            public bool isBold;
            public BtnStruct(char c, SymbolType t = SymbolType.Undefined, bool b = false) 
            {
                this.Content = c;
                this.Type = t;
                this.isBold = b; 
            }
        }
        private BtnStruct[,] buttons =
        {
            {new BtnStruct('%'), new BtnStruct('\u0152', SymbolType.ClearEntry), new BtnStruct('C', SymbolType.ClearAll), new BtnStruct('\u232b', SymbolType.BackSpace) },
            {new BtnStruct('\u215f', SymbolType.specialOperator), new BtnStruct('\u00b2'), new BtnStruct('\u221a'), new BtnStruct('\u00f7', SymbolType.Operator) },
            {new BtnStruct('7', SymbolType.Number, true), new BtnStruct('8', SymbolType.Number, true), new BtnStruct('9', SymbolType.Number, true), new BtnStruct('\u00d7', SymbolType.Operator) },
            {new BtnStruct('4', SymbolType.Number, true), new BtnStruct('5', SymbolType.Number, true), new BtnStruct('6', SymbolType.Number, true), new BtnStruct('-', SymbolType.Operator) },
            {new BtnStruct('1', SymbolType.Number, true), new BtnStruct('2', SymbolType.Number, true), new BtnStruct('3', SymbolType.Number, true), new BtnStruct('+', SymbolType.Operator) },
            {new BtnStruct('\u00b1', SymbolType.PlusMinusSign), new BtnStruct('0', SymbolType.Number, true), new BtnStruct(',', SymbolType.DecimalPoint), new BtnStruct('=', SymbolType.Operator) }
        };

        float lblResultBaseFontSize;
        const int lblResultWidthMargin = 24;
        const int lblResultMaxDigit = 25;

        char lastOperator = ' ';
        decimal operand1, operand2, result;
        BtnStruct lastButtonClicked;

        public Form1()
        {
            InitializeComponent();
            lblResultBaseFontSize = lblResult.Font.Size;
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
                    myButton.Text = buttons[i, j].Content.ToString();
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
           
            switch (clickedButtonStruct.Type)
            {
                case SymbolType.Number:
                    if (lblResult.Text == "0" || lastButtonClicked.Type == SymbolType.Operator) lblResult.Text = ""; 
                    lblResult.Text += clickedButton.Text;
                    break;
                case SymbolType.specialOperator:
                    ManageSpecialOperator(clickedButtonStruct);
                    break;
                case SymbolType.Operator:
                    if (lastButtonClicked.Type != SymbolType.Operator || clickedButtonStruct.Content == '=')
                    {
                        ManageOperator(clickedButtonStruct);
                    }
                    else
                    {
                        lastOperator = clickedButtonStruct.Content;
                    }
                    break;
                case SymbolType.DecimalPoint:
                    if (lblResult.Text.IndexOf(",") == -1)
                        lblResult.Text += clickedButton.Text;
                    break;
                case SymbolType.PlusMinusSign:
                    if (lblResult.Text != "0")
                        if (lblResult.Text.IndexOf("-") == -1)
                            lblResult.Text = "-" + lblResult.Text;
                        else
                            lblResult.Text = lblResult.Text.Substring(1);
                    if (lastButtonClicked.Type == SymbolType.Operator)
                    {
                        operand1 = -operand1;
                    }
                    break;
                case SymbolType.BackSpace:
                    if (lastButtonClicked.Type != SymbolType.Operator)
                    {
                        lblResult.Text = lblResult.Text.Substring(0, lblResult.Text.Length - 1);
                        if (lblResult.Text.Length == 0 || lblResult.Text == "-0" || lblResult.Text == "-")
                            lblResult.Text = "0";
                    }
                   
                    break;
                case SymbolType.ClearEntry:
                    if (lastButtonClicked.Content == '=') 
                        clearAll();
                    else 
                        lblResult.Text = "0";
                    break;
                case SymbolType.ClearAll:
                    clearAll();
                    break;
                case SymbolType.Undefined:
                    break;
                default:
                    break;
            }
            if(clickedButtonStruct.Type != SymbolType.BackSpace && clickedButtonStruct.Type != SymbolType.PlusMinusSign)
            lastButtonClicked = clickedButtonStruct;
        }

        private void clearAll()
        {
            operand1 = 0;
            operand2 = 0;
            result = 0;
            lastOperator = ' ';
            lblResult.Text = "0";
        }

        private void ManageSpecialOperator(BtnStruct clickedButtonStruct)
        {
            if (clickedButtonStruct.Type == SymbolType.specialOperator)
            {
                operand2 = decimal.Parse(lblResult.Text);
                switch (clickedButtonStruct.Content)
                {
                    case '%':
                        result = operand1 * operand2;
                        break;
                    case '\u215F':  // 1/x
                        result = 1 / operand2;
                        break;
                    case '\u00B2':  // x^2
                        result = operand2 * operand2;
                        break ;
                    case '\u221A':
                        result = (decimal)Math.Sqrt((double)operand2);
                        break ;
                    default:
                        break;
                }
                lblResult.Text = result.ToString();
            }
        }
        private void ManageOperator(BtnStruct clickedButtonStruct) 
        {
            if (lastOperator == ' ')
            {
                operand1 = decimal.Parse(lblResult.Text);
                if (clickedButtonStruct.Content != '=') lastOperator = clickedButtonStruct.Content;
                lblCronologia.Text = operand1.ToString() + " " + lastOperator.ToString() + " "; 
            }
            else
            {
                if (lastButtonClicked.Content != '=') operand2 = decimal.Parse(lblResult.Text);
                lblCronologia.Text += operand2.ToString() + " ="; 
                switch (lastOperator)
                {
                    case '+':
                        result = operand1 + operand2;
                        break;
                    case '-':
                        result = operand1 - operand2;
                        break;
                    case '\u00d7':
                        result = operand1 * operand2;
                        break;
                    case '\u00F7':
                        result = operand1 / operand2;
                        break; 
                    default:
                        break;
                }
                operand1 = result;
                if (clickedButtonStruct.Content != '=')
                {
                    lastOperator = clickedButtonStruct.Content;
                    if (lastButtonClicked.Content == '=')
                        operand2 = 0;
                }
                lblResult.Text = result.ToString();
            }
        }

        private void lblResult_TextChanged(object sender, EventArgs e)
        {
            if (lblResult.Text.Length > 0) 
            {
                double num = double.Parse(lblResult.Text); string stOut = "";
                NumberFormatInfo nfi = new CultureInfo("it-IT", false).NumberFormat;
                int decimalSeparatorPosition = lblResult.Text.IndexOf(",");
                nfi.NumberDecimalDigits = decimalSeparatorPosition == -1 ? 0 
                    : lblResult.Text.Length - decimalSeparatorPosition - 1;
                stOut = num.ToString("N", nfi);
                if (lblResult.Text.IndexOf(",") == lblResult.Text.Length - 1) stOut += ",";
                lblResult.Text = stOut;
            }
            if (lblResult.Text.Length > lblResultMaxDigit)
                lblResult.Text = lblResult.Text.Substring(0, lblResultMaxDigit);

            int textWidth = TextRenderer.MeasureText(lblResult.Text, lblResult.Font).Width;
            float newSize = lblResult.Font.Size * (((float)lblResult.Size.Width - lblResultWidthMargin) / textWidth);
            if (newSize > lblResultBaseFontSize) newSize = lblResultBaseFontSize; 
            lblResult.Font = new Font("Segoe UI", newSize, FontStyle.Regular); 
        }
    }
}
