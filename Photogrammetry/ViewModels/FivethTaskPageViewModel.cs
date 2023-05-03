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
    public class FivethTaskPageViewModel : BindableBase
    {
        /// <summary>
        /// Указатель навигации
        /// </summary>
        private readonly IRegionManager iregionManager;
        /// <summary>
        /// Коллекция значений
        /// </summary>
        public ObservableCollection<FivethTaskModel> DataOfStereopairs { get; set; }

        #region Properties for task
        private int indexOfElement;

        private FivethTaskModel _entity;
        public FivethTaskModel Entity { get { return _entity; } set { SetProperty(ref _entity, value); } }

        string valOne, valTwo, valThree, valFour, valFive, valSix, valSeven, valEight, valNine, valTen, valEleven, valTwelve;
        public string ValOne { get { return valOne; } set { SetProperty(ref valOne, value); } }
        public string ValTwo { get { return valTwo; } set { SetProperty(ref valTwo, value); } }
        public string ValThree { get { return valThree; } set { SetProperty(ref valThree, value); } }
        public string ValFour { get { return valFour; } set { SetProperty(ref valFour, value); } }
        public string ValFive { get { return valFive; } set { SetProperty(ref valFive, value); } }
        public string ValSix { get { return valSix; } set { SetProperty(ref valSix, value); } }
        #endregion

        public FivethTaskPageViewModel(IRegionManager regionManager)
        {
            iregionManager = regionManager;
            DataOfStereopairs = new ObservableCollection<FivethTaskModel>();
        }

        #region Commands

        private DelegateCommand _resetDataCommnad;
        private DelegateCommand _insertDataCommand;
        private DelegateCommand _backToNavMenuCommand;
        private DelegateCommand _calculateCommand;
        private DelegateCommand<FivethTaskModel> _deleteDataFromCollectionCommand;
        private DelegateCommand<FivethTaskModel> _editDataFromCollectionCommand;


        public DelegateCommand ResetDataCommnad =>
            _resetDataCommnad ?? (_resetDataCommnad = new DelegateCommand(ExecuteResetDataCommnad));
        public DelegateCommand InsertDataCommand =>
            _insertDataCommand ?? (_insertDataCommand = new DelegateCommand(ExecuteInsertDataCommand));
        public DelegateCommand BackToNavMenuCommand =>
            _backToNavMenuCommand ?? (_backToNavMenuCommand = new DelegateCommand(ExecuteBackToNavMenuCommand));
        public DelegateCommand CalculateCommand =>
            _calculateCommand ?? (_calculateCommand = new DelegateCommand(ExecuteCalculateCommand));
        public DelegateCommand<FivethTaskModel> DeleteDataFromCollectionCommand =>
            _deleteDataFromCollectionCommand ?? (_deleteDataFromCollectionCommand = new DelegateCommand<FivethTaskModel>(ExecuteDeleteDataFromCollectionCommand));
        public DelegateCommand<FivethTaskModel> EditDataFromCollectionCommand =>
            _editDataFromCollectionCommand ?? (_editDataFromCollectionCommand = new DelegateCommand<FivethTaskModel>(ExecuteEditDataFromCollectionCommand));

        void ExecuteResetDataCommnad()
        {
            ValOne = ValTwo = ValThree = ValFour = ValFive = ValSix = string.Empty;
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
                    double[] vals = DecimalCheker.CheckDecimal(ValOne, ValTwo, ValThree, ValFour, ValFive, ValSix);
                    if (_entity == null)
                    {
                        DataOfStereopairs.Add(new FivethTaskModel
                        {
                            X1 = vals[0],
                            Y1 = vals[1],
                            X2 = vals[2],
                            Y2 = vals[3],
                            Tau2 = vals[4],
                            Tau1 = vals[5]
                        });
                    }
                    else
                    {
                        _entity.X1 = vals[0];
                        _entity.Y1 = vals[1];
                        _entity.X2 = vals[2];
                        _entity.Y2 = vals[3];
                        _entity.Tau2 = vals[4];
                        _entity.Tau1 = vals[5];

                        DataOfStereopairs.RemoveAt(indexOfElement);
                        DataOfStereopairs.Insert(indexOfElement, _entity);
                    }
                    ValOne = ValTwo = ValThree = ValFour = ValFive = ValSix = string.Empty;
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
            saveFile.SaveFivethSolution(DataOfStereopairs);
        }

        void ExecuteDeleteDataFromCollectionCommand(FivethTaskModel val)
        {
            indexOfElement = DataOfStereopairs.IndexOf(val);

            if (indexOfElement > -1 && indexOfElement < DataOfStereopairs.Count)
                DataOfStereopairs.RemoveAt(indexOfElement);
        }

        void ExecuteEditDataFromCollectionCommand(FivethTaskModel val)
        {
            Entity = val;
            indexOfElement = DataOfStereopairs.IndexOf(val);

            ValOne = string.Format(val.X1.ToString(), ".", ",");
            ValTwo = string.Format(val.Y1.ToString(), ".", ",");
            ValThree = string.Format(val.X2.ToString(), ".", ",");
            ValFour = string.Format(val.Y2.ToString(), ".", ",");
            ValFive = string.Format(val.Tau2.ToString(), ".", ",");
            ValSix = string.Format(val.Tau1.ToString(), ".", ",");
        }

        private bool CheckField(out string message)
        {
            if (string.IsNullOrWhiteSpace(ValOne))
            {
                message = "Значение X1 не может быть пустым!";
                return false;
            }

            if (string.IsNullOrWhiteSpace(ValTwo))
            {
                message = "Значение Y1 не может быть пустым!";
                return false;
            }

            if (string.IsNullOrWhiteSpace(ValThree))
            {
                message = "Значение X2 не может быть пустым!";
                return false;
            }

            if (string.IsNullOrWhiteSpace(ValFour))
            {
                message = "Значение Y1 не может быть пустым!";
                return false;
            }

            if (string.IsNullOrWhiteSpace(ValFive))
            {
                message = "Значение τ2 не может быть пустым!";
                return false;
            }

            if (string.IsNullOrWhiteSpace(ValSix))
            {
                message = "Значение τ1 не может быть пустым!";
                return false;
            }

            message = string.Empty;
            return true;
        }
        #endregion
    }
}
