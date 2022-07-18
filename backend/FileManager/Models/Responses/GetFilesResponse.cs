using System.Text.Json.Serialization;

namespace FileManager.Models.Responses
{
    public class GetFilesResponse
    {
        [JsonPropertyName("files")]
        public List<FileData> Files { get; set; }

        [JsonPropertyName("parent_path")]
        public string? ParentPath { get; set; }
    }
}
