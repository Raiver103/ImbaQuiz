public class MongoLoggingSettings
{
    public const string SectionName = "MongoLogging";

    public string ConnectionString { get; set; }
    public string CollectionName { get; set; }
}
