using System;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using NCalc;

namespace Examenu3yu4
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            // Código opcional para ejecutar cuando se carga el formulario
            // Puedes dejarlo vacío o agregar cualquier inicialización adicional aquí
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                double initialGuess = double.Parse(txt1.Text); // Valor inicial (xn)
                double tolerance = 1e-10; // Tolerancia para mayor precisión
                int maxIterations = 1000; // Máximo de iteraciones

                // Obtiene las expresiones ingresadas por el usuario para la función y sus derivadas
                string functionExpression = txt2.Text;
                string derivativeExpression = txt3.Text;
                string secondDerivativeExpression = txt4.Text; // Segunda derivada

                double result = ImprovedNewtonRaphsonMethod(functionExpression, derivativeExpression, secondDerivativeExpression, initialGuess, tolerance, maxIterations);

                lblResult.Text = $"Raíz: {result}";
            }
            catch (Exception ex)
            {
                lblResult.Text = $"Error: {ex.Message}";
            }
        }

        private double ImprovedNewtonRaphsonMethod(string func, string deriv, string secondDeriv, double initialGuess, double tolerance, int maxIterations)
        {
            double x = initialGuess;
            for (int i = 0; i < maxIterations; i++)
            {
                double fx = EvaluateExpression(func, x);
                double fpx = EvaluateExpression(deriv, x);
                double fppx = EvaluateExpression(secondDeriv, x);

                if (Math.Abs(fpx) < tolerance)
                    throw new Exception("La derivada está cerca de cero, no se puede dividir.");

                // Aplicando la fórmula del método de Newton-Raphson mejorado
                double denominator = Math.Pow(fpx, 2) - fx * fppx;
                if (Math.Abs(denominator) < tolerance)
                    throw new Exception("El denominador está cerca de cero, no se puede dividir.");

                double xNew = x - (fx * fpx) / denominator;

                // Mensaje de diagnóstico para cada iteración
                MessageBox.Show($"Iteración {i + 1}:\n" +
                                $"x = {x}\n" +
                                $"f(x) = {fx}\n" +
                                $"f'(x) = {fpx}\n" +
                                $"f''(x) = {fppx}\n" +
                                $"xNew = {xNew}");

                if (Math.Abs(xNew - x) < tolerance)
                    return xNew;

                x = xNew;
            }

            throw new Exception("Se alcanzó el número máximo de iteraciones sin convergencia.");
        }

        private double EvaluateExpression(string expression, double x)
        {
            try
            {
                // Reemplaza potencias escritas con "^" a la forma "Pow(base, exponente)"
                expression = Regex.Replace(expression, @"(\d+(\.\d+)?|\b\.\d+|\b[xX])\s*\^\s*(\d+(\.\d+)?)", m =>
                {
                    string baseNum = m.Groups[1].Value;
                    string exponent = m.Groups[3].Value;
                    return $"Pow({baseNum}, {exponent})";
                });

                // Crea una expresión de NCalc
                Expression e = new Expression(expression);

                // Asigna el valor de la variable "x"
                e.Parameters["x"] = x;

                // Configura la función Pow() en NCalc
                e.EvaluateFunction += (name, args) =>
                {
                    if (name == "Pow" && args.Parameters.Length == 2)
                    {
                        double baseNum = Convert.ToDouble(args.Parameters[0].Evaluate());
                        double exponent = Convert.ToDouble(args.Parameters[1].Evaluate());
                        args.Result = Math.Pow(baseNum, exponent);
                    }
                };

                var result = e.Evaluate();

                if (result == null)
                    throw new Exception("La evaluación de la expresión devolvió un resultado nulo.");

                return Convert.ToDouble(result);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al evaluar la expresión: {ex.Message}");
                throw new Exception("Error al evaluar la expresión. Verifique la sintaxis.");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Limpia los TextBox y el Label de resultado
            txt1.Clear();
            txt2.Clear();
            txt3.Clear();
            txt4.Clear(); // Limpia el campo de la segunda derivada
            lblResult.Text = "";
        }

        // Métodos TextChanged vacíos para evitar errores
        private void txt1_TextChanged(object sender, EventArgs e)
        {
        }

        private void txt2_TextChanged(object sender, EventArgs e)
        {
        }

        private void txt3_TextChanged(object sender, EventArgs e)
        {
        }
    }
}
