using System;
using System.Windows;
using System.Windows.Controls;

namespace zad2
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private double previousNumber;
        private string currentOperation;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            string buttonContent = button.Content.ToString();

            switch (buttonContent)
            {
                case "C":
                    ClearCalculator();
                    break;
                case "=":
                    CalculateResult();
                    break;
                case ",":
                    displayLabel.Content += ",";
                    break;
                case "√":
                    Operation(Math.Sqrt, "√");
                    break;
                case "1/x":
                    Operation(x => 1 / x, "1/x");
                    break;
                case "x!":
                    Operation(Factorial, "!");
                    break;
                case "log10":
                    Operation(Math.Log10, "log10");
                    break;
                case "ln":
                    Operation(Math.Log, "ln");
                    break;
                case "log2":
                    Operation(x => Math.Log(x, 2), "log2");
                    break;
                case "Floor":
                    Operation(Math.Floor, "Floor");
                    break;
                case "Ceiling":
                    Operation(Math.Ceiling, "Ceiling");
                    break;
                default:
                    if (double.TryParse(buttonContent, out double number))
                    {
                        Update(number);
                    }
                    else
                    {
                        currentOperation = buttonContent;
                        previousNumber = double.Parse(displayLabel.Content.ToString());
                        displayLabel.Content = "0";
                    }
                    break;
            }
        }

        private void Update(double number)
        {
            if (displayLabel.Content.ToString() == "0")
            {
                displayLabel.Content = number.ToString();
            }
            else
            {
                displayLabel.Content += number.ToString();
            }
        }

        private void ClearCalculator()
        {
            displayLabel.Content = "0";
            previousLabel.Content = "";
            previousNumber = 0;
            currentOperation = null;
        }

        private void CalculateResult()
        {
            if (currentOperation != null)
            {
                double currentNumber = double.Parse(displayLabel.Content.ToString());
                double result = 0;

                switch (currentOperation)
                {
                    case "+":
                        result = previousNumber + currentNumber;
                        break;
                    case "-":
                        result = previousNumber - currentNumber;
                        break;
                    case "×":
                        result = previousNumber * currentNumber;
                        break;
                    case "÷":
                        if (currentNumber == 0)
                        {
                            MessageBox.Show("Nie można dzielić przez zero.");
                            ClearCalculator();
                            return;
                        }
                        result = previousNumber / currentNumber;
                        break;
                    case "^":
                        result = Math.Pow(previousNumber, currentNumber);
                        break;
                    case "mod":
                        result = previousNumber % currentNumber;
                        break;
                    default:
                        break;
                }

                displayLabel.Content = result.ToString();
                previousLabel.Content = "(" + previousNumber + currentOperation.ToString() + currentNumber.ToString() + ") = ";
                previousNumber = result;
                currentOperation = null;
            }
        }

        private void Operation(Func<double, double> operation, string operationText)
        {
            double currentNumber = double.Parse(displayLabel.Content.ToString());
            double result = operation(currentNumber);
            displayLabel.Content = result.ToString();
        }

        private double Factorial(double n)
        {
            if (n < 0 || n % 1 != 0)
            {
                MessageBox.Show("Silnia jest zdefiniowana tylko dla nieujemnych liczb całkowitych");
                ClearCalculator();
                return 0;
            }

            if (n == 0)
            {
                return 1;
            }

            double result = 1;
            for (int i = 1; i <= n; i++)
            {
                result *= i;
            }

            return result;
        }
    }
}
