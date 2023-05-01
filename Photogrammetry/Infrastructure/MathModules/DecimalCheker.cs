using System;

namespace Photogrammetry.Infrastructure.MathModules
{
    public static class DecimalCheker
    {
        public static double[] CheckDecimal(params string[] values)
        {
            double[] result = new double[values.Length];

            for (int i = 0; i < values.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(values[i]))
                    throw new Exception("В полях не может содержаться пустая строка или пробелы!");

                if (!double.TryParse(values[i].ToString(System.Globalization.CultureInfo.GetCultureInfo("en-US")), out result[i]))
                    throw new Exception($"Значение {values[i]} не соответствует десятичному типу");
            }

            return result;
        }
    }
}
