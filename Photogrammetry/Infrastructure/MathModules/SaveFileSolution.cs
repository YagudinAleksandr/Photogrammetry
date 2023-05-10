using Microsoft.Win32;
using Photogrammetry.Models;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;

namespace Photogrammetry.Infrastructure.MathModules
{
    public class SaveFileSolution
    {
        #region Const
        private const double Ep = 0.0000001;
        private const double Eps = 0.00001;
        private const double Rad = 57.2957795130823208767981548144105;
        #endregion

        #region Private fields
        private readonly SaveFileDialog saveFileDialog;
        private FileStream fileStream;
        private StreamWriter streamWriter;
        private string fileName = string.Empty;
        #endregion

        public SaveFileSolution()
        {
            saveFileDialog= new SaveFileDialog();

            saveFileDialog.Filter = "Text files(*.txt)|*.txt|All files(*.*)|*.*";
        }

        #region Public Methods
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
                streamWriter.Write("Вычисление центральных углов, образованных направлениями на\n");
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
        public void SaveSecondSolution(ObservableCollection<SecondTaskModel> entities)
        {
            int counter = 1;

            saveFileDialog.ShowDialog();

            fileName = saveFileDialog.FileName;

            fileStream = new FileStream(fileName, FileMode.Create);

            streamWriter = new StreamWriter(fileStream);

            try
            {
                streamWriter.WriteLine("Уравнение углов в ромбтческих сетях");
                streamWriter.WriteLine($"----------- Дата создания расчета: {DateTime.Now} ---------------");

                foreach (var val in entities)
                {

                    double temp1, temp2, temp3, temp4, temp5; // [10][11][8][9]ba

                    temp1 = temp2 = temp3 = temp4 = 0.0;

                    Calculate(val.Alpha1, ref temp3, ref temp4, ref temp1, ref temp2);

                    temp5 = temp3;

                    Calculate(val.Gamma2, ref temp3, ref temp4, ref temp1, ref temp2);

                    temp5 *= temp3;

                    Calculate(val.Alpha2Tier, ref temp3, ref temp4, ref temp1, ref temp2);

                    temp5 *= temp3;

                    Calculate(val.Gamma1Tier, ref temp3, ref temp4, ref temp1, ref temp2);

                    temp5 *= temp3;

                    Calculate(val.Gamma1, ref temp3, ref temp4, ref temp1, ref temp2);

                    temp5 /= temp3;

                    Calculate(val.Alpha2, ref temp3, ref temp4, ref temp1, ref temp2);

                    temp5 /= temp3;

                    Calculate(val.Gamma2Tier, ref temp3, ref temp4, ref temp1, ref temp2);

                    temp5 /= temp3;

                    Calculate(val.Alpha1Tier, ref temp3, ref temp4, ref temp1, ref temp2);

                    temp3 = (temp5 / temp3 - 1.0) * (180 / Math.PI);
                    temp4 = temp3 / temp1;
                    temp2 = Math.Sqrt(temp2) * 3.0;

                    streamWriter.WriteLine("-----------------------------------------");
                    streamWriter.WriteLine($"Данные № {counter}");

                    
                    streamWriter.WriteLine($"Xв: {val.Alpha1}");
                    streamWriter.WriteLine($"Yв: {val.Gamma2}");
                    streamWriter.WriteLine($"pв: {val.Alpha2Tier}");
                    streamWriter.WriteLine($"qв: {val.Gamma1Tier}");
                    streamWriter.WriteLine($"Xл: {val.Gamma1}");
                    streamWriter.WriteLine($"Yл: {val.Alpha2}");
                    streamWriter.WriteLine($"pл: {val.Gamma2Tier}");
                    streamWriter.WriteLine($"qл: {val.Alpha1Tier}");
                    

                    streamWriter.WriteLine($"Оптимальное решение № {counter}");
                    streamWriter.WriteLine($"WПдоп = {temp2}");
                    streamWriter.WriteLine($"Wп = {temp3}");
                    streamWriter.WriteLine($"V = {temp4}");

                    counter++;
                }

                streamWriter.Close();
                fileStream.Close();

                MessageBox.Show("Файл успешно сохранен!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        public void SaveThirdSolution(ObservableCollection<ThirdTaskModel> entities)
        {
            int counter = 1;

            saveFileDialog.ShowDialog();

            fileName = saveFileDialog.FileName;

            fileStream = new FileStream(fileName, FileMode.Create);

            streamWriter = new StreamWriter(fileStream);

            try
            {
                streamWriter.WriteLine("Вычисление центральных углов, образованных направлениями на");
                streamWriter.WriteLine($"----------- Дата создания расчета: {DateTime.Now} ---------------");

                foreach (var val in entities)
                {
                    double temp1, temp2, temp3, temp4, temp5;
                    temp1 = temp2 = temp3 = temp4 = temp5 = 0.0;

                    temp1 = val.X2 - val.X1;
                    temp2 = val.Y2 - val.Y1;
                    temp3 = temp1 / temp2;
                    temp4 = Math.Atan(temp3) * Rad;
                    temp5 = temp4;

                    if (temp2 < Eps)
                    {
                        if (Math.Abs(temp1) < Eps)
                            temp5 = temp4 + 180.0;

                        if (Math.Abs(temp1) >= Eps)
                            temp5 = temp4 + 180.0;
                    }
                    else if (temp2 >= Eps)
                    {
                        if (Math.Abs(temp1) >= Eps)
                            temp5 = temp4 + 360.0;
                    }

                    temp1 = val.M * Math.Sqrt(Math.Pow(temp1, 2) + Math.Pow(temp2, 2));



                    streamWriter.WriteLine("-----------------------------------------");
                    streamWriter.WriteLine($"Данные № {counter}");

                    streamWriter.WriteLine($"X1: {val.X1}");
                    streamWriter.WriteLine($"Y1: {val.Y1}");
                    streamWriter.WriteLine($"X2: {val.X2}");
                    streamWriter.WriteLine($"Y2: {val.Y2}");
                    streamWriter.WriteLine($"m: {val.M}");

                    streamWriter.WriteLine($"Оптимальное решение № {counter}");
                    streamWriter.WriteLine($"Дирекционный угол = {temp5}");
                    streamWriter.WriteLine($"Длина первого базиса = {temp1}");

                    counter++;
                }

                streamWriter.Close();
                fileStream.Close();

                MessageBox.Show("Файл успешно сохранен!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        public void SaveFourthSolution(ObservableCollection<FourthTaskModel> entities)
        {
            int counter = 1;

            saveFileDialog.ShowDialog();

            fileName = saveFileDialog.FileName;

            fileStream = new FileStream(fileName, FileMode.Create);

            streamWriter = new StreamWriter(fileStream);

            try
            {
                streamWriter.WriteLine("Вычисление предварительных координат главных точек снимков");
                streamWriter.WriteLine($"----------- Дата создания расчета: {DateTime.Now} ---------------");

                foreach (var val in entities)
                {
                    double alpha2, basisB2, x2, y2, x3, y3;
                    alpha2 = basisB2 = x2 = y2 = x3 = y3 = 0.0;

                    x2 = val.X1Usl + Math.Sin(val.AlphaB1 * (Math.PI / 180)) * val.B1;
                    y2 = val.Y1Usl + Math.Cos(val.AlphaB1 * (Math.PI / 180)) * val.B1;

                    basisB2 = Math.Sin(val.Alpha1 * (Math.PI / 180)) / Math.Sin(val.Gamma1 * (Math.PI / 180)) * val.B1;

                    basisB2 = basisB2 * Math.Sin(val.Gamma2 * (Math.PI / 180)) / Math.Sin(val.Alpha2 * (Math.PI / 180));
                    alpha2 = val.AlphaB1 + 180 + val.Betta1 + val.Betta2;

                    x3 = x2 + Math.Sin(alpha2 * (Math.PI / 180)) * basisB2;
                    y3 = y2 + Math.Cos(alpha2 * (Math.PI / 180)) * basisB2;

                    streamWriter.WriteLine("-----------------------------------------");
                    streamWriter.WriteLine($"Данные {counter}");

                    streamWriter.WriteLine($"Xв: {val.AlphaB1}");
                    streamWriter.WriteLine($"Yв: {val.Alpha1}");
                    streamWriter.WriteLine($"pв: {val.Gamma1}");
                    streamWriter.WriteLine($"qв: {val.Alpha2}");
                    streamWriter.WriteLine($"Xл: {val.Gamma2}");
                    streamWriter.WriteLine($"Yл: {val.B1}");
                    streamWriter.WriteLine($"pл: {val.Betta1}");
                    streamWriter.WriteLine($"qл: {val.Betta2}");
                    streamWriter.WriteLine($"Xп: {val.X1Usl}");
                    streamWriter.WriteLine($"Yп: {val.Y1Usl}");

                    streamWriter.WriteLine($"Оптимальное решение№ {counter}");
                    streamWriter.WriteLine($"X2 = {x2}");
                    streamWriter.WriteLine($"Y2 = {y2}");
                    streamWriter.WriteLine($"X3 = {x3}");
                    streamWriter.WriteLine($"Y3 = {y3}");
                    streamWriter.WriteLine($"Базис B2 = {basisB2}");
                    streamWriter.WriteLine($"Дирекционный угол Alpha2 = {alpha2}");

                    counter++;
                }

                streamWriter.Close();
                fileStream.Close();

                MessageBox.Show("Файл успешно сохранен!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        public void SaveFivethSolution(ObservableCollection<FivethTaskModel> entities)
        {
            int counter = 1;

            saveFileDialog.ShowDialog();

            fileName = saveFileDialog.FileName;

            fileStream = new FileStream(fileName, FileMode.Create);

            streamWriter = new StreamWriter(fileStream);

            try
            {
                streamWriter.WriteLine("Вычисление координат опознаков");
                streamWriter.WriteLine($"----------- Дата создания расчета: {DateTime.Now} ---------------");

                foreach (var val in entities)
                {
                    val.X1 = val.X1 * (Math.PI / 180);
                    val.X2 = val.X2 * (Math.PI / 180);
                    val.Y1 = val.Y1 * (Math.PI / 180);
                    val.Y2 = val.Y2 * (Math.PI / 180);
                    val.Tau1 = val.Tau1 * (Math.PI / 180);
                    val.Tau2 = val.Tau2 * (Math.PI / 180);

                    double Xp, Yp;
                    Xp = Yp = 0.0;

                    Xp = Jungs(true, val) * (180 / Math.PI);
                    Yp = Jungs(false, val) * (180 / Math.PI);

                    streamWriter.WriteLine("-----------------------------------------");
                    streamWriter.WriteLine($"Данные {counter}");

                    streamWriter.WriteLine($"X1: {val.X1 * (180 / Math.PI)}");
                    streamWriter.WriteLine($"Y1: {val.Y1 * (180 / Math.PI)}");
                    streamWriter.WriteLine($"X2: {val.X2 * (180 / Math.PI)}");
                    streamWriter.WriteLine($"Y2: {val.Y2 * (180 / Math.PI)}");
                    streamWriter.WriteLine($"τ2: {val.Tau2 * (180 / Math.PI)}");
                    streamWriter.WriteLine($"τ1: {val.Tau1 * (180 / Math.PI)}");

                    streamWriter.WriteLine($"Оптимальное решение№ {counter}");
                    streamWriter.WriteLine($"Xp = {Xp}");
                    streamWriter.WriteLine($"Yp = {Yp}");

                    counter++;
                }

                streamWriter.Close();
                fileStream.Close();

                MessageBox.Show("Файл успешно сохранен!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        public void SaveSixthSolution(ObservableCollection<SixthTaskModel> entities)
        {
            int counter = 1;

            saveFileDialog.ShowDialog();

            fileName = saveFileDialog.FileName;

            fileStream = new FileStream(fileName, FileMode.Create);

            streamWriter = new StreamWriter(fileStream);

            try
            {
                streamWriter.WriteLine("Вычисление исправленных значений первого бизиса сети и его дирекционного угла");
                streamWriter.WriteLine($"----------- Дата создания расчета: {DateTime.Now} ---------------");

                foreach (var val in entities)
                {
                    double length1, length2, delta1, delta2, delta3, delta4, alpha, betta, al1, al2;
                    length1 = length2 = delta1 = delta2 = delta3 = delta4 = alpha = betta = al1 = al2 = 0.0;

                    delta1 = val.X661Usl - val.X545Usl;
                    delta2 = val.Y661Usl - val.Y545Usl;
                    length1 = Math.Sqrt(Math.Pow(delta1, 2) + Math.Pow(delta2, 2));

                    delta3 = val.X661G - val.X545G;
                    delta4 = val.Y661G - val.Y545G;
                    length2 = Math.Sqrt(Math.Pow(delta3, 2) + Math.Pow(delta4, 2));

                    betta = val.Betta * (length2 / length1 * 100000.0);

                    al1 = Math.Acos(delta3 / length2);
                    al2 = Math.Acos(delta1 / length1);

                    al1 /= Math.PI / 180;
                    al2 /= Math.PI / 180;

                    if (delta4 >= 0.0) ;
                    if (delta4 < 0.0)
                        al1 = 360.0 - al1;

                    if (delta2 >= 0.0) ;
                    if (delta2 < 0.0)
                        al2 = 360.0 - al2;

                    while (al2 < 0.0)
                        al2 += 360.0;

                    while (al2 > 360.0)
                        al2 -= 360.0;

                    alpha = val.Alpha + (al1 - al2);

                    streamWriter.WriteLine("-----------------------------------------");
                    streamWriter.WriteLine($"Данные {counter}");

                    streamWriter.WriteLine($"X545усл: {val.X545Usl}");
                    streamWriter.WriteLine($"Y545усл: {val.Y545Usl}");
                    streamWriter.WriteLine($"X661усл: {val.X661Usl}");
                    streamWriter.WriteLine($"Y661усл: {val.Y661Usl}");
                    streamWriter.WriteLine($"X545г: {val.X545G}");
                    streamWriter.WriteLine($"Y545г: {val.Y545G}");
                    streamWriter.WriteLine($"X661г: {val.X661G}");
                    streamWriter.WriteLine($"Y661г: {val.Y661G}");
                    streamWriter.WriteLine($"180: {val.Betta}");
                    streamWriter.WriteLine($"360: {val.Alpha}");

                    streamWriter.WriteLine($"Оптимальное решение № {counter}");
                    streamWriter.WriteLine($"Betta1исп = {betta}");
                    streamWriter.WriteLine($"Alpha1исп = {alpha}");

                    counter++;
                }

                streamWriter.Close();
                fileStream.Close();

                MessageBox.Show("Файл успешно сохранен!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion

        #region Private methods

        /// <summary>
        /// Метод дополнительного подсчета
        /// </summary>
        /// <param name="alpha1">[0]</param>
        /// <param name="temp1">[8]</param>
        /// <param name="temp2">[9]</param>
        /// <param name="temp3">[10]</param>
        /// <param name="temp4">[11]</param>
        private void Calculate(double alpha1, ref double temp1, ref double temp2, ref double temp3, ref double temp4)
        {
            double temp = alpha1 * (Math.PI / 180);

            temp1 = Math.Sin(temp);
            temp2 = 1.0 / Math.Tan(temp);
            temp3 += temp2;
            temp4 += Math.Pow(temp2, 2);
        }

        /// <summary>
        /// Формула Юнга
        /// </summary>
        /// <param name="type">Тип обработки (True - для XP, False - для YP)</param>
        /// <param name="entity">Сущность в радианах</param>
        /// <returns>Угол в радианах</returns>
        private double Jungs(bool type, FivethTaskModel entity)
        {
            double result, var1,var2,var3,var4;

            var1 = var2 = var3 = var4 = result = 0.0;

            if(type)
            {
                var1 = entity.X1;
                var2 = entity.X2;
                var3 = entity.Y2;
                var4 = entity.Y1;
            }
            else
            {
                var1 = entity.Y1;
                var2 = entity.Y2;
                var3 = entity.X1;
                var4 = entity.X2;
            }

            result = var1 * Math.Cos(entity.Tau2) * Math.Sin(entity.Tau1);
            result += var2 * Math.Cos(entity.Tau1) * Math.Sin(entity.Tau2);
            result -= (var3 - var4) * Math.Sin(entity.Tau1) * Math.Sin(entity.Tau2);

            result = result / Math.Sin(entity.Tau1 + entity.Tau2);

            return result;
        }
        #endregion
    }
}
