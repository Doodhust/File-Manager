using System.Text.Json.Serialization;

namespace FileManager
{
    public class FileData
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("full_name")]
        public string FullName { get; set; }

        [JsonPropertyName("last_modified_at")]
        public DateTime LastModifiedAt { get; set; }

        [JsonPropertyName("is_directory")]
        public bool IsDirectory { get; set; }
    }
}
