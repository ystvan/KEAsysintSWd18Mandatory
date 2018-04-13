using CloudSOAP.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudSOAP.Repository
{
    public interface IDataRepository
    {
        List<SensorData> GetAllData();        
        SensorData AddNewData(SensorData entry);
       
    }
}
