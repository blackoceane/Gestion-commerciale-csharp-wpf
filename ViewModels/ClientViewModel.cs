using L_Joyce.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L_Joyce.ViewModels
{
    public class ClientViewModel:BaseViewModel
    {
        public ObservableCollection<Client> Clients => DataService.Instance().Clients;

      
    }
}


