using Photogrammetry.Infrastructure.MathModules;
using Photogrammetry.Models;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.ObjectModel;
using System.Windows;

namespace Photogrammetry.ViewModels
{
    public class FourthTaskPageViewModel : BindableBase
    {
        /// <summary>
        /// Указатель навигации
        /// </summary>
        private readonly IRegionManager iregionManager;
        /// <summary>
        /// Коллекция значений
        /// </summary>
        public ObservableCollection<FourthTaskModel> DataOfStereopairs { get; set; }

        #region Properties for task
        private int indexOfElement;

        private FourthTaskModel _entity;
        public FourthTaskModel Entity { get { return _entity; } set { SetProperty(ref _entity, value); } }

        string valOne, valTwo, valThree, valFour, valFive, valSix, valSeven, valEight, valNine, valTen, valEleven, valTwelve;
        public string ValOne { get { return valOne; } set { SetProperty(ref valOne, value); } }
        public string ValTwo { get { return valTwo; } set { SetProperty(ref valTwo, value); } }
        public string ValThree { get { return valThree; } set { SetProperty(ref valThree, value); } }
        public string ValFour { get { return valFour; } set { SetProperty(ref valFour, value); } }
        public string ValFive { get { return valFive; } set { SetProperty(ref valFive, value); } }
        public string ValSix { get { return valSix; } set { SetProperty(ref valSix, value); } }
        public string ValSeven { get { return valSeven; } set { SetProperty(ref valSeven, value); } }
        public string ValEight { get { return valEight; } set { SetProperty(ref valEight, value); } }
        public string ValNine { get { return valNine; } set { SetProperty(ref valNine, value); } }
        public string ValTen { get { return valTen; } set { SetProperty(ref valTen, value); } }
        #endregion

        public FourthTaskPageViewModel(IRegionManager regionManager)
        {
            iregionManager = regionManager;
            DataOfStereopairs = new ObservableCollection<FourthTaskModel>();
        }

        #region Commands

        private DelegateCommand _resetDataCommnad;
        private DelegateCommand _insertDataCommand;
        private DelegateCommand _backToNavMenuCommand;
        private DelegateCommand _calculateCommand;
        private DelegateCommand<FourthTaskModel> _deleteDataFromCollectionCommand;
        private DelegateCommand<FourthTaskModel> _editDataFromCollectionCommand;


        public DelegateCommand ResetDataCommnad =>
            _resetDataCommnad ?? (_resetDataCommnad = new DelegateCommand(ExecuteResetDataCommnad));
        public DelegateCommand InsertDataCommand =>
            _insertDataCommand ?? (_insertDataCommand = new DelegateCommand(ExecuteInsertDataCommand));
        public DelegateCommand BackToNavMenuCommand =>
            _backToNavMenuCommand ?? (_backToNavMenuCommand = new DelegateCommand(ExecuteBackToNavMenuCommand));
        public DelegateCommand CalculateCommand =>
            _calculateCommand ?? (_calculateCommand = new DelegateCommand(ExecuteCalculateCommand));
        public DelegateCommand<FourthTaskModel> DeleteDataFromCollectionCommand =>
            _deleteDataFromCollectionCommand ?? (_deleteDataFromCollectionCommand = new DelegateCommand<FourthTaskModel>(ExecuteDeleteDataFromCollectionCommand));
        public DelegateCommand<FourthTaskModel> EditDataFromCollectionCommand =>
            _editDataFromCollectionCommand ?? (_editDataFromCollectionCommand = new DelegateCommand<FourthTaskModel>(ExecuteEditDataFromCollectionCommand));

        void ExecuteResetDataCommnad()
        {
            ValOne = ValTwo = ValThree = ValFour = ValFive = ValSix = ValSeven = ValEight = ValNine = ValTen = string.Empty;
            Entity = null;
        }

        void ExecuteInsertDataCommand()
        {
            if (!CheckField(out string err))
                MessageBox.Show(err, "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
            else
            {
                try
                {
                    double[] vals = DecimalCheker.CheckDecimal(ValOne, ValTwo, ValThree, ValFour, ValFive, ValSix, ValSeven, ValEight, ValNine, ValTen);
                    if (_entity == null)
                    {
                        DataOfStereopairs.Add(new FourthTaskModel
                        {
                            AlphaB1 = vals[0],
                            Alpha1 = vals[1],
                            Gamma1 = vals[2],
                            Alpha2 = vals[3],
                            Gamma2 = vals[4],
                            B1 = vals[5],
                            Betta1 = vals[6],
                            Betta2 = vals[7],
                            X1Usl = vals[8],
                            Y1Usl = vals[9]
                        });
                    }
                    else
                    {
                        _entity.AlphaB1 = vals[0];
                        _entity.Alpha1 = vals[1];
                        _entity.Gamma1 = vals[2];
                        _entity.Alpha2 = vals[3];
                        _entity.Gamma2 = vals[4];
                        _entity.B1 = vals[5];
                        _entity.Betta1 = vals[6];
                        _entity.Betta2 = vals[7];
                        _entity.X1Usl = vals[8];
                        _entity.Y1Usl = vals[9];

                        DataOfStereopairs.RemoveAt(indexOfElement);
                        DataOfStereopairs.Insert(indexOfElement, _entity);
                    }
                    ValOne = ValTwo = ValThree = ValFour = ValFive = ValSix = ValSeven = ValEight = ValNine = ValTen = string.Empty;
                    Entity = null;

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка в данных", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

            }
        }

        void ExecuteBackToNavMenuCommand()
        {
            iregionManager.RequestNavigate("ContentRegion", "Toolbar");
        }

        void ExecuteCalculateCommand()
        {
            if (DataOfStereopairs.Count <= 0)
            {
                MessageBox.Show("Нет данных для вычисления!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            SaveFileSolution saveFile = new SaveFileSolution();
            saveFile.SaveFourthSolution(DataOfStereopairs);
        }

        void ExecuteDeleteDataFromCollectionCommand(FourthTaskModel val)
        {
            indexOfElement = DataOfStereopairs.IndexOf(val);

            if (indexOfElement > -1 && indexOfElement < DataOfStereopairs.Count)
                DataOfStereopairs.RemoveAt(indexOfElement);
        }

        void ExecuteEditDataFromCollectionCommand(FourthTaskModel val)
        {
            Entity = val;
            indexOfElement = DataOfStereopairs.IndexOf(val);

            ValOne = string.Format(val.AlphaB1.ToString(), ".", ",");
            ValTwo = string.Format(val.Alpha1.ToString(), ".", ",");
            ValThree = string.Format(val.Gamma1.ToString(), ".", ",");
            ValFour = string.Format(val.Alpha2.ToString(), ".", ",");
            ValFive = string.Format(val.Gamma2.ToString(), ".", ",");
            ValSix = string.Format(val.B1.ToString(), ".", ",");
            ValSeven = string.Format(val.Betta1.ToString(), ".", ",");
            ValEight = string.Format(val.Betta2.ToString(), ".", ",");
            ValNine = string.Format(val.X1Usl.ToString(), ".", ",");
            ValTen = string.Format(val.Y1Usl.ToString(), ".", ",");
        }

        private bool CheckField(out string message)
        {
            if (string.IsNullOrWhiteSpace(ValOne))
            {
                message = "Значение αB1 не может быть пустым!";
                return false;
            }

            if (string.IsNullOrWhiteSpace(ValTwo))
            {
                message = "Значение α1 не может быть пустым!";
                return false;
            }

            if (string.IsNullOrWhiteSpace(ValThree))
            {
                message = "Значение y1 не может быть пустым!";
                return false;
            }

            if (string.IsNullOrWhiteSpace(ValFour))
            {
                message = "Значение α2 не может быть пустым!";
                return false;
            }

            if (string.IsNullOrWhiteSpace(ValFive))
            {
                message = "Значение y2 не может быть пустым!";
                return false;
            }

            if (string.IsNullOrWhiteSpace(ValSix))
            {
                message = "Значение B1 не может быть пустым!";
                return false;
            }

            if (string.IsNullOrWhiteSpace(ValSeven))
            {
                message = "Значение β1 не может быть пустым!";
                return false;
            }

            if (string.IsNullOrWhiteSpace(ValEight))
            {
                message = "Значение β2 не может быть пустым!";
                return false;
            }

            if (string.IsNullOrWhiteSpace(ValNine))
            {
                message = "Значение X1усл не может быть пустым!";
            }

            if (string.IsNullOrWhiteSpace(ValTen))
            {
                message = "Значение Y1усл не может быть пустым!";
                return false;
            }

            message = string.Empty;
            return true;
        }
        #endregion
    }
}
