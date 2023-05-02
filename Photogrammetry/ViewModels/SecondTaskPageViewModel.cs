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
    public class SecondTaskPageViewModel : BindableBase
    {
        /// <summary>
        /// Указатель навигации
        /// </summary>
        private readonly IRegionManager iregionManager;
        /// <summary>
        /// Коллекция значений
        /// </summary>
        public ObservableCollection<SecondTaskModel> DataOfStereopairs { get; set; }
        public SecondTaskPageViewModel(IRegionManager regionManager)
        {
            iregionManager = regionManager;
            DataOfStereopairs = new ObservableCollection<SecondTaskModel>();
        }

        #region Properties for task
        private int indexOfElement;

        private SecondTaskModel _entity;
        public SecondTaskModel Entity { get { return _entity; } set { SetProperty(ref _entity, value); } }

        string valOne, valTwo, valThree, valFour, valFive, valSix, valSeven, valEight, valNine, valTen, valEleven, valTwelve;
        public string ValOne { get { return valOne; } set { SetProperty(ref valOne, value); } }
        public string ValTwo { get { return valTwo; } set { SetProperty(ref valTwo, value); } }
        public string ValThree { get { return valThree; } set { SetProperty(ref valThree, value); } }
        public string ValFour { get { return valFour; } set { SetProperty(ref valFour, value); } }
        public string ValFive { get { return valFive; } set { SetProperty(ref valFive, value); } }
        public string ValSix { get { return valSix; } set { SetProperty(ref valSix, value); } }
        public string ValSeven { get { return valSeven; } set { SetProperty(ref valSeven, value); } }
        public string ValEight { get { return valEight; } set { SetProperty(ref valEight, value); } }
        #endregion

        #region Commands
        private DelegateCommand _resetDataCommnad;
        private DelegateCommand _insertDataCommand;
        private DelegateCommand _backToNavMenuCommand;
        private DelegateCommand _calculateCommand;
        private DelegateCommand<SecondTaskModel> _deleteDataFromCollectionCommand;
        private DelegateCommand<SecondTaskModel> _editDataFromCollectionCommand;


        public DelegateCommand ResetDataCommnad =>
            _resetDataCommnad ?? (_resetDataCommnad = new DelegateCommand(ExecuteResetDataCommnad));
        public DelegateCommand InsertDataCommand =>
            _insertDataCommand ?? (_insertDataCommand = new DelegateCommand(ExecuteInsertDataCommand));
        public DelegateCommand BackToNavMenuCommand =>
            _backToNavMenuCommand ?? (_backToNavMenuCommand = new DelegateCommand(ExecuteBackToNavMenuCommand));
        public DelegateCommand CalculateCommand =>
            _calculateCommand ?? (_calculateCommand = new DelegateCommand(ExecuteCalculateCommand));
        public DelegateCommand<SecondTaskModel> DeleteDataFromCollectionCommand =>
            _deleteDataFromCollectionCommand ?? (_deleteDataFromCollectionCommand = new DelegateCommand<SecondTaskModel>(ExecuteDeleteDataFromCollectionCommand));
        public DelegateCommand<SecondTaskModel> EditDataFromCollectionCommand =>
            _editDataFromCollectionCommand ?? (_editDataFromCollectionCommand = new DelegateCommand<SecondTaskModel>(ExecuteEditDataFromCollectionCommand));

        void ExecuteResetDataCommnad()
        {
            ValOne = ValTwo = ValThree = ValFour = ValFive = ValSix = ValSeven = ValEight = string.Empty;
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
                    double[] vals = DecimalCheker.CheckDecimal(ValOne, ValTwo, ValThree, ValFour, ValFive, ValSix, ValSeven, ValEight);
                    if (_entity == null)
                    {
                        DataOfStereopairs.Add(new SecondTaskModel
                        {
                            Alpha1 = vals[0],
                            Gamma2 = vals[1],
                            Alpha2Tier = vals[2],
                            Gamma1Tier = vals[3],
                            Gamma1 = vals[4],
                            Alpha2 = vals[5],
                            Gamma2Tier = vals[6],
                            Alpha1Tier = vals[7]
                        });
                    }
                    else
                    {
                        _entity.Alpha1 = vals[0];
                        _entity.Gamma2 = vals[1];
                        _entity.Alpha2Tier = vals[2];
                        _entity.Gamma1Tier = vals[3];
                        _entity.Gamma1 = vals[4];
                        _entity.Alpha2 = vals[5];
                        _entity.Gamma2Tier = vals[6];
                        _entity.Alpha1Tier = vals[7];

                        DataOfStereopairs.RemoveAt(indexOfElement);
                        DataOfStereopairs.Insert(indexOfElement, _entity);
                    }
                    ValOne = ValTwo = ValThree = ValFour = ValFive = ValSix = ValSeven = ValEight = string.Empty;
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
            saveFile.SaveSecondSolution(DataOfStereopairs);
        }

        void ExecuteDeleteDataFromCollectionCommand(SecondTaskModel val)
        {
            indexOfElement = DataOfStereopairs.IndexOf(val);

            if (indexOfElement > -1 && indexOfElement < DataOfStereopairs.Count)
                DataOfStereopairs.RemoveAt(indexOfElement);
        }

        void ExecuteEditDataFromCollectionCommand(SecondTaskModel val)
        {
            Entity = val;
            indexOfElement = DataOfStereopairs.IndexOf(val);

            ValOne = string.Format(val.Alpha1.ToString(), ".", ",");
            ValTwo = string.Format(val.Gamma2.ToString(), ".", ",");
            ValThree = string.Format(val.Alpha2Tier.ToString(), ".", ",");
            ValFour = string.Format(val.Gamma1Tier.ToString(), ".", ",");
            ValFive = string.Format(val.Gamma1.ToString(), ".", ",");
            ValSix = string.Format(val.Alpha2.ToString(), ".", ",");
            ValSeven = string.Format(val.Gamma2Tier.ToString(), ".", ",");
            ValEight = string.Format(val.Alpha1Tier.ToString(), ".", ",");
            
        }

        private bool CheckField(out string message)
        {
            if (string.IsNullOrWhiteSpace(ValOne))
            {
                message = "Значение Xв не может быть пустым!";
                return false;
            }

            if (string.IsNullOrWhiteSpace(ValTwo))
            {
                message = "Значение Yв не может быть пустым!";
                return false;
            }

            if (string.IsNullOrWhiteSpace(ValThree))
            {
                message = "Значение pв не может быть пустым!";
                return false;
            }

            if (string.IsNullOrWhiteSpace(ValFour))
            {
                message = "Значение qв не может быть пустым!";
                return false;
            }

            if (string.IsNullOrWhiteSpace(ValFive))
            {
                message = "Значение Xл не может быть пустым!";
                return false;
            }

            if (string.IsNullOrWhiteSpace(ValSix))
            {
                message = "Значение Yл не может быть пустым!";
                return false;
            }

            if (string.IsNullOrWhiteSpace(ValSeven))
            {
                message = "Значение рл не может быть пустым!";
                return false;
            }

            if (string.IsNullOrWhiteSpace(ValEight))
            {
                message = "Значение qл не может быть пустым!";
                return false;
            }

            

            message = string.Empty;
            return true;
        }
        #endregion
    }
}
