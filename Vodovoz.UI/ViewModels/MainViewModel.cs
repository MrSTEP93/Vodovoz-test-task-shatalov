using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Vodovoz.UI.Common;

namespace Vodovoz.UI.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private object _currentViewModel;
        public object CurrentViewModel
        {
            get => _currentViewModel;
            set => SetProperty(ref _currentViewModel, value);
        }

        public ICommand ShowEmployeesCommand { get; }
        public ICommand ShowClientsCommand { get; }
        public ICommand ShowOrdersCommand { get; }

        public MainViewModel()
        {
            // Пока используем строки-заглушки. Позже заменим их на реальные ViewModel
            ShowEmployeesCommand = new RelayCommand(() =>
                CurrentViewModel = "Здесь будет экран Сотрудников");

            ShowClientsCommand = new RelayCommand(() =>
                CurrentViewModel = "Здесь будет экран Контрагентов");

            ShowOrdersCommand = new RelayCommand(() =>
                CurrentViewModel = "Здесь будет экран Заказов");


        }
    }
}
