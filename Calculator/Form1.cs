using System;
using System.Windows.Forms;

namespace Calculator
{
    public partial class Form1 : Form
    {
        private int _numberOfBrackets;
        private bool _isEqualsButton = false;
        private int NumberOfBrackets
        {
            get => _numberOfBrackets;
            set => _numberOfBrackets = value <= 0 ? 0 : value;
        }


        public Form1()
        {
            InitializeComponent();
        }      

        private bool LastSymbolIsOperation()
        {
            return (richTextBox1.Text[richTextBox1.Text.Length - 1] == '+'
                || richTextBox1.Text[richTextBox1.Text.Length - 1] == '-'
                || richTextBox1.Text[richTextBox1.Text.Length - 1] == '/'
                || richTextBox1.Text[richTextBox1.Text.Length - 1] == '*'
                || richTextBox1.Text[richTextBox1.Text.Length - 1] == '^');
        }

        private bool LastSymbolIsBracket()
        {
            if(richTextBox1.Text.Length == 0)
                return false;
            return (richTextBox1.Text[richTextBox1.Text.Length - 1] == ')');
        }
        private bool LastSymbolIsDot()
        {
            return (richTextBox1.Text[richTextBox1.Text.Length - 1] == ',');
        }
        private void SubstringLastSymbolInRichTextBox(ref RichTextBox richTextBox)
        {
            richTextBox.Text = richTextBox.Text.Substring(0, richTextBox.Text.Length - 1);
        }

        private void AddNumbersOnClick(string symbol)
        {
            if (_isEqualsButton)
                richTextBox1.Text = "";
            
            if(NumberOfBrackets > 0)
                SubstringLastSymbolInRichTextBox(ref richTextBox1);
            
            if (LastSymbolIsBracket())
                richTextBox1.Text += '*';

            richTextBox1.Text += symbol;
            _isEqualsButton = false;
            
            if(NumberOfBrackets > 0)
                richTextBox1.Text += ')';
        }

        private void OperationButtonOnClick(string operationSymbol, int operationNumber)
        {
            if (richTextBox1.Text.Length == 0)
                richTextBox1.Text = "0";

            if (LastSymbolIsOperation() || LastSymbolIsDot())
                SubstringLastSymbolInRichTextBox(ref richTextBox1);

            if (NumberOfBrackets > 0)
                SubstringLastSymbolInRichTextBox(ref richTextBox1);

            richTextBox1.Text += operationSymbol;

            if (NumberOfBrackets > 0)
                richTextBox1.Text += ')';
        }

        private void AddTrigonometricOperation(string trigonometric)
        {
            if (LastSymbolIsBracket())
                SubstringLastSymbolInRichTextBox(ref richTextBox1);
            NumberOfBrackets++;
            richTextBox1.Text += trigonometric + "()";
            _isEqualsButton = false;
        }

        private void ZeroButtonClick(object sender, EventArgs e)
        {
            AddNumbersOnClick("0");
        }
        private void OneButtonClick(object sender, EventArgs e)
        {
            AddNumbersOnClick("1");
        }
        private void TwoButtonClick(object sender, EventArgs e)
        {
            AddNumbersOnClick("2");
        }
        private void ThreeButtonClick(object sender, EventArgs e)
        {
            AddNumbersOnClick("3");
        }
        private void FourButtonClick(object sender, EventArgs e)
        {
            AddNumbersOnClick("4");
        }
        private void FiveButtonClick(object sender, EventArgs e)
        {
            AddNumbersOnClick("5");
        }
        private void SixButtonClick(object sender, EventArgs e)
        {
            AddNumbersOnClick("6");
        }
        private void SevenButtonClick(object sender, EventArgs e)
        {
            AddNumbersOnClick("7");
        }
        private void EightButtonClick(object sender, EventArgs e)
        {
            AddNumbersOnClick("8");
        }
        private void NineButtonClick(object sender, EventArgs e)
        {
            AddNumbersOnClick("9");
        }
        private void DotButtonClick(object sender, EventArgs e)
        {
            if (LastSymbolIsOperation() || LastSymbolIsDot())
                SubstringLastSymbolInRichTextBox(ref richTextBox1);

            if (_isEqualsButton)
                richTextBox1.Text = "";

            if(richTextBox1.Text.Length == 0)
                richTextBox1.Text = "0";

            richTextBox1.Text += ",";
            _isEqualsButton = false;
        }
        private void SumButtonClick(object sender, EventArgs e)
        {
            OperationButtonOnClick("+", 0);
        }
        private void SubtractButtonClick(object sender, EventArgs e)
        {
            OperationButtonOnClick("-", 1);
        }
        private void MultiplicationButtonClick(object sender, EventArgs e)
        {
            OperationButtonOnClick("*", 2);
        }
        private void DivisionButtonClick(object sender, EventArgs e)
        {
            OperationButtonOnClick("/", 3);
        }
        private void DegreeButtonClick(object sender, EventArgs e)
        {
            OperationButtonOnClick("^", 4);
        }
        private void FactorialButtonClick(object sender, EventArgs e)
        {
            if (richTextBox1.Text.Length == 0)
                richTextBox1.Text = "0";
            OperationButtonOnClick("!", 5);
        }
        private void ClearButtonClick(object sender, EventArgs e)
        {
            _numberOfBrackets = 0;
            if (richTextBox1.Text.Length == 0)
            {
                HistoryParser.ClearAllInformation();
                flowLayoutPanel1.Controls.Clear();
                flowLayoutPanel1.ResumeLayout();
            }
            richTextBox1.Text = "";
        }
        private void DeleteButtonClick(object sender, EventArgs e)
        {
            if (richTextBox1.Text.Length <= 0)
                return;

            SubstringLastSymbolInRichTextBox(ref richTextBox1);
        }

        private void EqualsButtonClick(object sender, EventArgs e)
        {
            if (LastSymbolIsOperation())
                return;
            if (LastSymbolIsDot())
                richTextBox1.Text += '0';

            double result = Calculate.Calc(richTextBox1.Text);
            _isEqualsButton = true;
            if(result.ToString() != richTextBox1.Text 
                && HistoryParser.SetNewInformation(richTextBox1.Text, result.ToString()))
                AddELementInHistory(richTextBox1.Text + '=' + result.ToString());
            richTextBox1.Text = result.ToString();
        }

        public void AddELementInHistory(string calculations)
        {
            Button button = new Button();
            button.Text = calculations;
            flowLayoutPanel1.Controls.Add(button);
            button.Click += new EventHandler(HistoryButton_Click);
        }

        private void HistoryButton_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            if (button != null)
                richTextBox1.Text = button.Text.Split('=')[0];
        }

        private void LeftBracket_Click(object sender, EventArgs e)
        {
            LeftBracket_Click();
        }

        private void RightBracket_Click(object sender, EventArgs e)
        {
            if (NumberOfBrackets == 0)
            {
                LeftBracket_Click();
                return;
            }

            NumberOfBrackets--;

            SubstringLastSymbolInRichTextBox(ref richTextBox1);

            richTextBox1.Text += ")";
            _isEqualsButton = false;
        }

        private void LeftBracket_Click()
        {
            NumberOfBrackets++;
            richTextBox1.Text += "()";
            _isEqualsButton = false;
        }

        private void AsinButton_Click(object sender, EventArgs e)
        {
            AddTrigonometricOperation("asin");
        }

        private void AtanButton_Click(object sender, EventArgs e)
        {
            AddTrigonometricOperation("atan");
        }

        private void SinButton_Click(object sender, EventArgs e)
        {
            AddTrigonometricOperation("sin");
        }

        private void CosButton_Click(object sender, EventArgs e)
        {
            AddTrigonometricOperation("cos");
        }

        private void LogButton_Click(object sender, EventArgs e)
        {
            AddTrigonometricOperation("log");
        }

        private void AcosButton_Click(object sender, EventArgs e)
        {
            AddTrigonometricOperation("acos");
        }

        private void TanButton_Click(object sender, EventArgs e)
        {
            AddTrigonometricOperation("tan");
        }

        private void SqrtButton_Click(object sender, EventArgs e)
        {
            AddTrigonometricOperation("sqrt");
        }

        private void LnButton_Click(object sender, EventArgs e)
        {
            AddTrigonometricOperation("ln");
        }

        private void ModButton_Click(object sender, EventArgs e)
        {
            AddTrigonometricOperation("mod");
        }
    }
}
