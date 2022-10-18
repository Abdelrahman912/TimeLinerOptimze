using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeLinerOptimize.App.UI.Services;
using TimeLinerOptimize.App.ViewModels;
using TimeLinerOptimze.Core.Dtos;
using TimeLinerOptimze.Core.Repositories;

namespace TimeLinerOptimize.App
{
    public class AppViewModel
    {
        public MainViewModel MainVM { get;  }
        public AppViewModel()
        {
            MainVM = new MainViewModel(Services.FileOpenService,
                                       Services.FolderSaveService,
                                       new CsvRepository());
        }
    }
}
