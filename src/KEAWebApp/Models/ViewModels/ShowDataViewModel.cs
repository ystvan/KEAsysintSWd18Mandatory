using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KEAWebApp.Models.ViewModels
{
    public class ShowDataViewModel
    {
        public IEnumerable<Data> Datas { get; } = new List<Data>();

        public ShowDataViewModel(List<Data> datas)
        {
            Datas = datas;
        }
    }
}
