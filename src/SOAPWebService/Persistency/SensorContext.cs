using CloudSOAP.DataContracts;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace CloudSOAP.Persistency
{
    public class SensorContext : DbContext
    {
        public SensorContext() 
            : base("name=SensorContext")
        { }
        public virtual DbSet<SensorData> SensorDatas { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }


    }
}