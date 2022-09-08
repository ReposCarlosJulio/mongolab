using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2
{
    public class MongoDBConnector
    {
        public string connectionString { get; set; }
        public string databaseName { get; set; }
        public string collectionName { get; set; }
    }
}
