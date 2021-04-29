using System;
using System.Collections.Generic;
using System.Text;

namespace Scala.DBProg.Core.Services
{
    class Helper
    {
        public static string GetConnectionString()
        {
            return @"Data Source=(local)\SQLEXPRESS;Initial Catalog=ScalaBoeken; Integrated security=true;";
        }
        public static string HandleQuotes(string value)
        {
            return value.Trim().Replace("'", "''");
        }
    }
}
