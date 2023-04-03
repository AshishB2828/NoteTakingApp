namespace NoteTakingApp.Models.DTO
{
    public class AjaxResponse
    {
        public int StatusCode { get; set; }
        public string? ResponseMessage { get; set; }
        public object? ResponseData { get; set; }
        public bool IsSuccess { get; set; }
    }
}
