using System;
using System.Windows.Forms;

namespace Examenu3yu4
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                double initialGuess = double.Parse(txt1.Text); // Valor inicial (xn)
                double tolerance = 1e-10; // Tolerancia más pequeña para mayor precisión
                int maxIterations = 1000; // Aumenta el número máximo de iteraciones

                double result = NewtonRaphsonMethod(initialGuess, tolerance, maxIterations);

                lblResult.Text = $"Raíz: {result}";
            }
            catch (Exception ex)
            {
                lblResult.Text = $"Error: {ex.Message}";
            }
        }

        private double NewtonRaphsonMethod(double initialGuess, double tolerance, int maxIterations)
        {
            double x = initialGuess;
            for (int i = 0; i < maxIterations; i++)
            {
                double fx = Function(x);
                double fpx = Derivative(x);

                if (Math.Abs(fpx) < tolerance)
                    throw new Exception("La derivada está cerca de cero, no se puede dividir.");

                double xNew = x - fx / fpx;

                // Mensaje de diagnóstico para cada iteración
                MessageBox.Show($"Iteración {i + 1}:\n" +
                                $"x = {x}\n" +
                                $"f(x) = {fx}\n" +
                                $"f'(x) = {fpx}\n" +
                                $"xNew = {xNew}");

                if (Math.Abs(xNew - x) < tolerance)
                    return xNew;

                x = xNew;
            }

            throw new Exception("Se alcanzó el número máximo de iteraciones sin convergencia.");
        }

        private double Function(double x)
        {
            // Define la función f(x) = 19 * x^7 - 6 * x^2 + 19
            return 19 * Math.Pow(x, 7) - 6 * Math.Pow(x, 2) + 19;
        }

        private double Derivative(double x)
        {
            // Define la derivada f'(x) = 133 * x^6 - 12 * x
            return 133 * Math.Pow(x, 6) - 12 * x;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Limpia los TextBox y el Label de resultado
            txt1.Clear();
            txt2.Clear();
            txt3.Clear();
            lblResult.Text = "";
        }
    }
}
