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
    public class FirstTaskPageViewModel : BindableBase
    {
        /// <summary>
        /// Указатель навигации
        /// </summary>
        private readonly IRegionManager iregionManager;
        /// <summary>
        /// Коллекция значений
        /// </summary>
        public ObservableCollection<FirstTaskModel> DataOfStereopairs { get; set; }

        #region Properties for task
        private int indexOfElement;

        private FirstTaskModel _entity;
        public FirstTaskModel Entity { get { return _entity; } set { SetProperty(ref _entity, value); } }

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
        public string ValEleven { get { return valEleven; } set { SetProperty(ref valEleven, value); } }
        public string ValTwelve { get { return valTwelve; } set { SetProperty(ref valTwelve, value); } }
        #endregion

        public FirstTaskPageViewModel(IRegionManager regionManager)
        {
            iregionManager = regionManager;
            DataOfStereopairs = new ObservableCollection<FirstTaskModel>();
        }

        #region Commands

        private DelegateCommand _resetDataCommnad;
        private DelegateCommand _insertDataCommand;
        private DelegateCommand _backToNavMenuCommand;
        private DelegateCommand _calculateCommand;
        private DelegateCommand<FirstTaskModel> _deleteDataFromCollectionCommand;
        private DelegateCommand<FirstTaskModel> _editDataFromCollectionCommand;
        
        
        public DelegateCommand ResetDataCommnad =>
            _resetDataCommnad ?? (_resetDataCommnad = new DelegateCommand(ExecuteResetDataCommnad));
        public DelegateCommand InsertDataCommand =>
            _insertDataCommand ?? (_insertDataCommand = new DelegateCommand(ExecuteInsertDataCommand));
        public DelegateCommand BackToNavMenuCommand =>
            _backToNavMenuCommand ?? (_backToNavMenuCommand = new DelegateCommand(ExecuteBackToNavMenuCommand));
        public DelegateCommand CalculateCommand =>
            _calculateCommand ?? (_calculateCommand = new DelegateCommand(ExecuteCalculateCommand));
        public DelegateCommand<FirstTaskModel> DeleteDataFromCollectionCommand =>
            _deleteDataFromCollectionCommand ?? (_deleteDataFromCollectionCommand = new DelegateCommand<FirstTaskModel>(ExecuteDeleteDataFromCollectionCommand));
        public DelegateCommand<FirstTaskModel> EditDataFromCollectionCommand =>
            _editDataFromCollectionCommand ?? (_editDataFromCollectionCommand = new DelegateCommand<FirstTaskModel>(ExecuteEditDataFromCollectionCommand));

        void ExecuteResetDataCommnad()
        {
            ValOne = ValTwo = ValThree = ValFour = ValFive = ValSix = ValSeven = ValEight = ValNine = ValTen = ValEleven = ValTwelve = string.Empty;
            Entity = null;
        }

        void ExecuteInsertDataCommand()
        {
            if(!CheckField(out string err))
                MessageBox.Show(err, "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
            else
            {
                try
                {
                    double[] vals = DecimalCheker.CheckDecimal(ValOne, ValTwo, ValThree, ValFour, ValFive, ValSix, ValSeven, ValEight, ValNine, ValTen, ValEleven, ValTwelve);
                    if (_entity == null)
                    {
                        DataOfStereopairs.Add(new FirstTaskModel
                        {
                            Xb = vals[0],
                            Yb = vals[1],
                            Pb = vals[2],
                            Qb = vals[3],
                            Xl = vals[4],
                            Yl = vals[5],
                            Pl = vals[6],
                            Ql = vals[7],
                            Xp = vals[8],
                            Yp = vals[9],
                            Pp = vals[10],
                            Qp = vals[11],
                        });
                    }
                    else
                    {
                        _entity.Xb = vals[0];
                        _entity.Yb = vals[1];
                        _entity.Pb = vals[2];
                        _entity.Qb = vals[3];
                        _entity.Xl = vals[4];
                        _entity.Yl = vals[5];
                        _entity.Pl = vals[6];
                        _entity.Ql = vals[7];
                        _entity.Xp = vals[8];
                        _entity.Yp = vals[9];
                        _entity.Pp = vals[10];
                        _entity.Qp = vals[11];

                        DataOfStereopairs.RemoveAt(indexOfElement);
                        DataOfStereopairs.Insert(indexOfElement, _entity);
                    }
                    ValOne = ValTwo = ValThree = ValFour = ValFive = ValSix = ValSeven = ValEight = ValNine = ValTen = ValEleven = ValTwelve = string.Empty;
                    Entity = null;

                }
                catch(Exception ex)
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
            if(DataOfStereopairs.Count<= 0)
            {
                MessageBox.Show("Нет данных для вычисления!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            SaveFileSolution saveFile = new SaveFileSolution();
            saveFile.SaveFirstSolution(DataOfStereopairs);
        }

        void ExecuteDeleteDataFromCollectionCommand(FirstTaskModel val)
        {
            indexOfElement = DataOfStereopairs.IndexOf(val);

            if (indexOfElement > -1 && indexOfElement < DataOfStereopairs.Count)
                DataOfStereopairs.RemoveAt(indexOfElement);
        }

        void ExecuteEditDataFromCollectionCommand(FirstTaskModel val)
        {
            Entity = val;
            indexOfElement = DataOfStereopairs.IndexOf(val);

            ValOne = string.Format(val.Xb.ToString(), ".", ",");
            ValTwo = string.Format(val.Yb.ToString(), ".", ",");
            ValThree = string.Format(val.Pb.ToString(), ".", ",");
            ValFour = string.Format(val.Qb.ToString(), ".", ",");
            ValFive = string.Format(val.Xl.ToString(), ".", ",");
            ValSix = string.Format(val.Yl.ToString(), ".", ",");
            ValSeven = string.Format(val.Pl.ToString(), ".", ",");
            ValEight = string.Format(val.Ql.ToString(), ".", ",");
            ValNine = string.Format(val.Xp.ToString(), ".", ",");
            ValTen = string.Format(val.Yp.ToString(), ".", ",");
            ValEleven = string.Format(val.Pp.ToString(), ".", ",");
            ValTwelve = string.Format(val.Qp.ToString(), ".", ",");
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

            if (string.IsNullOrWhiteSpace(ValNine))
            {
                message = "Значение Xп не может быть пустым!";
            }

            if (string.IsNullOrWhiteSpace(ValTen))
            {
                message = "Значение Yп не может быть пустым!";
                return false;
            }
                
            if (string.IsNullOrWhiteSpace(ValEleven))
            {
                message = "Значение рп не может быть пустым!";
                return false;
            }
                
            if (string.IsNullOrWhiteSpace(ValTwelve))
            {
                message = "Значение qп не может быть пустым!";
                return false;
            }

            message = string.Empty;
            return true;
        }
        #endregion

    }
}
