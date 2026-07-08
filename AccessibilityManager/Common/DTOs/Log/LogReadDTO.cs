
namespace Common.DTOs.Log
{
    public class LogReadDTO
    {
        public int Id { get; set; }

        public string? Level { get; set; }

        public string? Message { get; set; }

        public DateTime? Timestamp { get; set; }
    }
}
