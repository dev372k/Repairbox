namespace RepairBox.API.Models
{
    public class JSONResponse
    {
        public bool Status { get; set; } = false;
        public string Message { get; set; } = string.Empty;
        public object Data { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;
        public string ErrorDescription { get; set; } = string.Empty;

    }
}
