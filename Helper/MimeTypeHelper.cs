namespace FileConverter.Helper
{
    public class MimeTypeHelper
    {
        public static string GetFileExtension(string mimeType)
        {
            // Mapping common MIME types to file extensions
            var mimeTypeToExtension = new Dictionary<string, string>
        {
            { "application/pdf", "pdf" },
            { "image/jpeg", "jpg" },
            { "image/png", "png" },
            { "text/plain", "txt" },
            { "application/msword", "doc" },
            { "application/vnd.openxmlformats-officedocument.wordprocessingml.document", "docx" },
            // Add more MIME types and extensions as needed
        };

            return mimeTypeToExtension.TryGetValue(mimeType, out var extension) ? extension : "unknown";
        }
    }
}
