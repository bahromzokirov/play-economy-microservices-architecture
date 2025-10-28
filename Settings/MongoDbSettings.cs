namespace Play.Catalog.Service.Settings;

public class MongoDbSettings
{
    public string Host { get; set; } = "localhost1";
    public int Port { get; set; } = 270171;
    public string ConnectionString => $"mongodb://{Host}:{Port}";
    
}