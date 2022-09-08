// See https://aka.ms/new-console-template for more information
using ConsoleApp2;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;


var devEnvironmentVariable = Environment.GetEnvironmentVariable("NETCORE_ENVIRONMENT");
var isDevelopment = string.IsNullOrEmpty(devEnvironmentVariable) || devEnvironmentVariable.ToLower() == "development";

var builder = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
               .AddEnvironmentVariables();
               ;

if (isDevelopment) //only add secrets in development
{
    builder.AddUserSecrets<MongoDBConnector>();
}


IConfigurationRoot Configuration = builder.Build();

var services = new ServiceCollection()
   .Configure<MongoDBConnector>(Configuration.GetSection(nameof(MongoDBConnector)))
   .AddOptions()
   .BuildServiceProvider();

try
{
    var myConf = services.GetService<IOptions<MongoDBConnector>>();
    string connectionStringMongo = myConf.Value.connectionString;
    string databaseName   = myConf.Value.databaseName;
    string collectionName = myConf.Value.collectionName;
   

    var mongoClient = new MongoClient(connectionStringMongo);
    var db = mongoClient.GetDatabase(databaseName);
    var collection = db.GetCollection<BsonDocument>(collectionName);
    string filterString = @"{title:'The Princess Bride'}";
    var result = collection.Find(filterString).FirstOrDefault();
    Console.WriteLine(result);
    Console.ReadLine();
}
catch (Exception)
{

    throw;
}

