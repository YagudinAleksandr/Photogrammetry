using Microsoft.Win32;
using Photogrammetry.Models;
using Prism.Services.Dialogs;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;

namespace Photogrammetry.Infrastructure.MathModules
{
    public class SaveFileSolution
    {
        private const double Ep = 0.0000001;

        private readonly SaveFileDialog saveFileDialog;
        private FileStream fileStream;
        private StreamWriter streamWriter;

        private string fileName = string.Empty;
        
        public SaveFileSolution()
        {
            saveFileDialog= new SaveFileDialog();

            saveFileDialog.Filter = "Text files(*.txt)|*.txt|All files(*.*)|*.*";
        }

        public void SaveFirstSolution(ObservableCollection<FirstTaskModel> entities)
        {
            int ko = 0, counter = 1;
            double Zi, Za, Zb, Z1, Z2, Z3, Z4, Z5, Z6, Z7, Z15, answer, temp;

            saveFileDialog.ShowDialog();

            fileName = saveFileDialog.FileName;

            fileStream = new FileStream(fileName, FileMode.Create);

            streamWriter = new StreamWriter(fileStream);

            try
            {
                streamWriter.Write("Вычисление центральных углов, образованных направлениями на");
                streamWriter.WriteLine("связующие, трансформационные, и геодезические точки");
                streamWriter.WriteLine($"----------- Дата создания расчета: {DateTime.Now} ---------------");

                foreach (var val in entities)
                {
                    Zi = val.Yl - val.Yb - val.Ql + val.Qb;

                    Za = Zi * Zi;

                    Zb = val.Xl - val.Xb - val.Pl + val.Pb;

                    Z1 = Zb * Zb;

                    Z2 = Z1 + Za;

                    Z3 = val.Yp - val.Yb - val.Qp + val.Qb;

                    Z4 = Z3 * Z3;

                    Z5 = val.Xp - val.Xb - val.Pp + val.Pb;

                    Z6 = Z5 * Z5;

                    Z15 = (Z6 + Z4) * Z2;

                    if(Z15<Ep)
                    {
                        ko = 3;
                        Z15 = Z15 * (-1.0);
                    }

                    Z7 = Math.Sqrt(Z15);

                    if(ko==3)
                    {
                        ko = 0;
                        Z7 = Z7 * (-1.0);
                    }

                    temp = (Zi * Z3 + Zb * Z5) / Z7;

                    answer = Math.Acos(temp) * 180 / Math.PI;

                    streamWriter.WriteLine("-----------------------------------------");
                    streamWriter.WriteLine($"Данные для угла № {counter}");

                    streamWriter.WriteLine($"Xв: {val.Xb}");
                    streamWriter.WriteLine($"Yв: {val.Yb}");
                    streamWriter.WriteLine($"pв: {val.Pb}");
                    streamWriter.WriteLine($"qв: {val.Qb}");
                    streamWriter.WriteLine($"Xл: {val.Xl}");
                    streamWriter.WriteLine($"Yл: {val.Yl}");
                    streamWriter.WriteLine($"pл: {val.Pl}");
                    streamWriter.WriteLine($"qл: {val.Ql}");
                    streamWriter.WriteLine($"Xп: {val.Xp}");
                    streamWriter.WriteLine($"Yп: {val.Yp}");
                    streamWriter.WriteLine($"pп: {val.Pp}");
                    streamWriter.WriteLine($"qп: {val.Qp}");

                    streamWriter.WriteLine($"Оптимальное решение: Угол№ {counter} = {answer}");

                    counter++;
                }

                streamWriter.Close();
                fileStream.Close();

                MessageBox.Show("Файл успешно сохранен!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
