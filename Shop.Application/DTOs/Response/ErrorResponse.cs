namespace Shop.Application.DTOs.Response;

public class ErrorResponse
{
    public string Error { get; set; }
    public int StatusCode { get; set; }
    public string Type { get; set; }
    public DateTime Timestamp { get; set; }
    public string TraceId { get; set; }
    public object Details { get; set; }

    public ErrorResponse(string error, int statusCode, string type = null, object details = null)
    {
        Error = error;
        StatusCode = statusCode;
        Type = type ?? "Error";
        Timestamp = DateTime.UtcNow;
        Details = details;
    }
}