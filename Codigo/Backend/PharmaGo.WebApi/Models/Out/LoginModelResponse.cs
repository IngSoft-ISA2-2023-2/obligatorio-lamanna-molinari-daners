namespace PharmaGo.WebApi.Models.Out
{
    public class LoginModelResponse
    {
        public Guid token { get; set; }
        public string role { get; set; }
        public string userName { get; set; }
    }
}
