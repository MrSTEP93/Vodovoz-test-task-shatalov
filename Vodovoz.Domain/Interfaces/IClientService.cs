using System;
using System.Collections.Generic;
using System.Text;
using Vodovoz.Domain.Entities;

namespace Vodovoz.Domain.Interfaces
{
    public interface IClientService
    {
        Client? GetById(int id);
        IEnumerable<Client> GetAll();
        void Save(Client client);
        void Delete(int id);
    }
}
