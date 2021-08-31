using System;

namespace BigHelp.Math
{
    public class Functions
    {
        /// <summary>
        /// Projeta (ou normaliza) o valor de uma escala númerica em outra.
        /// </summary>
        /// <param name="value">O valor que se deseja obter a projeção na nova escala</param>
        /// <param name="minValue">Valor minimo da escala atual</param>
        /// <param name="maxValue">Valor máximo da escala atual</param>
        /// <param name="minProjection">Valor minimo da escala projetada (ou normalizada). Default: 0</param>
        /// <param name="maxProjection">Valor máximo da escala projetada (ou normalizada). Default: 100</param>
        /// <returns>O valor de <value>value</value> projetado (ou normalizado).</returns>
        public static int Normalize(int value, int minValue, int maxValue, int minProjection = 0, int maxProjection = 100)
        {
            double projectedValue = minProjection + ((value - (double)minValue) / (maxValue - minValue) * (maxProjection - minProjection));
            return Convert.ToInt32(System.Math.Round(projectedValue, 0));
        }

        /// <summary>
        /// Projeta (ou normaliza) o valor de uma escala númerica em outra.
        /// </summary>
        /// <param name="value">O valor que se deseja obter a projeção na nova escala</param>
        /// <param name="minValue">Valor minimo da escala atual</param>
        /// <param name="maxValue">Valor máximo da escala atual</param>
        /// <param name="minProjection">Valor minimo da escala projetada (ou normalizada). Default: 0</param>
        /// <param name="maxProjection">Valor máximo da escala projetada (ou normalizada). Default: 1</param>
        /// <returns>O valor de <value>value</value> projetado (ou normalizado).</returns>
        public static double Normalize(double value, double minValue, double maxValue, double minProjection = 0, double maxProjection = 1)
        {
            var projectedValue = minProjection + ((value - minValue) / (maxValue - minValue) * (maxProjection - minProjection));
            return projectedValue;
        }
    }
}
