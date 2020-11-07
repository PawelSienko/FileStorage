namespace FileUploader.BusinessLogic
{
    public static class Constants
    {
        public static class ConnectionStrings
        {
            public const string ContainerPropertyName = "StorageConnectionString";
        }

        public const string ContainerName = "files";

        public static class Metadata
        {
            public const string DateTimeUtc = "DateTimeUtc";
        }

        public static class ViewDataProperties
        {
            public static string Cursor { get; set; }
        }
    }
}
