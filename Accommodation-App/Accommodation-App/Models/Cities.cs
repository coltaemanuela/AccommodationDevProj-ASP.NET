using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace Accommodation_App.Models
{
    public class Cities
    {
        public Cities()
        {
        }
        public int City_Id
        {
            get;
            set;
        }
        public string Name
        {
            get;
            set;
        }
    }

    public class CityContext : DbContext
    {
        public CityContext() : base() { }
        public DbSet<Cities> CityDb { set; get; }

    }
}