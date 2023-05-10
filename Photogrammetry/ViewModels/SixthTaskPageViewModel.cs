using Photogrammetry.Infrastructure.MathModules;
using Photogrammetry.Models;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace Photogrammetry.ViewModels
{
    public class SixthTaskPageViewModel : BindableBase
    {
        /// <summary>
        /// Указатель навигации
        /// </summary>
        private readonly IRegionManager iregionManager;
        /// <summary>
        /// Коллекция значений
        /// </summary>
        public ObservableCollection<SixthTaskModel> DataOfStereopairs { get; set; }

        #region Properties for task
        private int indexOfElement;

        private SixthTaskModel _entity;
        public SixthTaskModel Entity { get { return _entity; } set { SetProperty(ref _entity, value); } }

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

        public SixthTaskPageViewModel(IRegionManager regionManager)
        {
            iregionManager = regionManager;
            DataOfStereopairs = new ObservableCollection<SixthTaskModel>();
        }

        #region Commands

        private DelegateCommand _resetDataCommnad;
        private DelegateCommand _insertDataCommand;
        private DelegateCommand _backToNavMenuCommand;
        private DelegateCommand _calculateCommand;
        private DelegateCommand<SixthTaskModel> _deleteDataFromCollectionCommand;
        private DelegateCommand<SixthTaskModel> _editDataFromCollectionCommand;


        public DelegateCommand ResetDataCommnad =>
            _resetDataCommnad ?? (_resetDataCommnad = new DelegateCommand(ExecuteResetDataCommnad));
        public DelegateCommand InsertDataCommand =>
            _insertDataCommand ?? (_insertDataCommand = new DelegateCommand(ExecuteInsertDataCommand));
        public DelegateCommand BackToNavMenuCommand =>
            _backToNavMenuCommand ?? (_backToNavMenuCommand = new DelegateCommand(ExecuteBackToNavMenuCommand));
        public DelegateCommand CalculateCommand =>
            _calculateCommand ?? (_calculateCommand = new DelegateCommand(ExecuteCalculateCommand));
        public DelegateCommand<SixthTaskModel> DeleteDataFromCollectionCommand =>
            _deleteDataFromCollectionCommand ?? (_deleteDataFromCollectionCommand = new DelegateCommand<SixthTaskModel>(ExecuteDeleteDataFromCollectionCommand));
        public DelegateCommand<SixthTaskModel> EditDataFromCollectionCommand =>
            _editDataFromCollectionCommand ?? (_editDataFromCollectionCommand = new DelegateCommand<SixthTaskModel>(ExecuteEditDataFromCollectionCommand));

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
                        DataOfStereopairs.Add(new SixthTaskModel
                        {
                            X545Usl = vals[0],
                            Y545Usl = vals[1],
                            X661Usl = vals[2],
                            Y661Usl = vals[3],
                            X545G = vals[4],
                            Y545G = vals[5],
                            X661G = vals[6],
                            Y661G = vals[7],
                            Betta = vals[8],
                            Alpha = vals[9]
                        });
                    }
                    else
                    {
                        _entity.X545Usl = vals[0];
                        _entity.Y545Usl = vals[1];
                        _entity.X661Usl = vals[2];
                        _entity.Y661Usl = vals[3];
                        _entity.X545G = vals[4];
                        _entity.Y545G = vals[5];
                        _entity.X661G = vals[6];
                        _entity.Y661G = vals[7];
                        _entity.Betta = vals[8];
                        _entity.Alpha = vals[9];

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
            saveFile.SaveSixthSolution(DataOfStereopairs);
        }

        void ExecuteDeleteDataFromCollectionCommand(SixthTaskModel val)
        {
            indexOfElement = DataOfStereopairs.IndexOf(val);

            if (indexOfElement > -1 && indexOfElement < DataOfStereopairs.Count)
                DataOfStereopairs.RemoveAt(indexOfElement);
        }

        void ExecuteEditDataFromCollectionCommand(SixthTaskModel val)
        {
            Entity = val;
            indexOfElement = DataOfStereopairs.IndexOf(val);

            ValOne = string.Format(val.X545Usl.ToString(), ".", ",");
            ValTwo = string.Format(val.Y545Usl.ToString(), ".", ",");
            ValThree = string.Format(val.X661Usl.ToString(), ".", ",");
            ValFour = string.Format(val.Y661Usl.ToString(), ".", ",");
            ValFive = string.Format(val.X545G.ToString(), ".", ",");
            ValSix = string.Format(val.Y545G.ToString(), ".", ",");
            ValSeven = string.Format(val.X661G.ToString(), ".", ",");
            ValEight = string.Format(val.Y661G.ToString(), ".", ",");
            ValNine = string.Format(val.Betta.ToString(), ".", ",");
            ValTen = string.Format(val.Alpha.ToString(), ".", ",");
        }

        private bool CheckField(out string message)
        {
            if (string.IsNullOrWhiteSpace(ValOne))
            {
                message = "Значение X545усл не может быть пустым!";
                return false;
            }

            if (string.IsNullOrWhiteSpace(ValTwo))
            {
                message = "Значение Y545усл не может быть пустым!";
                return false;
            }

            if (string.IsNullOrWhiteSpace(ValThree))
            {
                message = "Значение X661усл не может быть пустым!";
                return false;
            }

            if (string.IsNullOrWhiteSpace(ValFour))
            {
                message = "Значение Y661усл не может быть пустым!";
                return false;
            }

            if (string.IsNullOrWhiteSpace(ValFive))
            {
                message = "Значение X545г не может быть пустым!";
                return false;
            }

            if (string.IsNullOrWhiteSpace(ValSix))
            {
                message = "Значение Y545г не может быть пустым!";
                return false;
            }

            if (string.IsNullOrWhiteSpace(ValSeven))
            {
                message = "Значение X661г не может быть пустым!";
                return false;
            }

            if (string.IsNullOrWhiteSpace(ValEight))
            {
                message = "Значение Y661г не может быть пустым!";
                return false;
            }

            if (string.IsNullOrWhiteSpace(ValNine))
            {
                message = "Значение 180 не может быть пустым!";
            }

            if (string.IsNullOrWhiteSpace(ValTen))
            {
                message = "Значение 360 не может быть пустым!";
                return false;
            }

            message = string.Empty;
            return true;
        }
        #endregion
    }
}
