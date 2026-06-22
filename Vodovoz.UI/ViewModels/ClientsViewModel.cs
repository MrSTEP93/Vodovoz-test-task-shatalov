using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Vodovoz.Domain.Entities;
using Vodovoz.Domain.Interfaces;
using Vodovoz.UI.Common;

namespace Vodovoz.UI.ViewModels
{
    public class ClientsViewModel : ViewModelBase
    {
        private readonly IClientService _clientService;
        private ObservableCollection<Client> _clients = null!;
        public ObservableCollection<Client> Clients
        {
            get => _clients;
            set => SetProperty(ref _clients, value);
        }

        public ClientsViewModel(IClientService clientService)
        {
            _clientService = clientService;
            LoadClients();
        }

        private void LoadClients()
        {
            var list = _clientService.GetAll();
            Clients = new ObservableCollection<Client>(list);

        }
    }
}
