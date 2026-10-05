namespace WebApp.Models
{
    public class ErrorVM
    {
        public string? Error { get; set; }

        public bool ErrorProvided => !string.IsNullOrEmpty(Error);
    }
}
