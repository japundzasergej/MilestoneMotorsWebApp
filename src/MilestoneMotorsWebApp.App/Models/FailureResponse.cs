namespace MilestoneMotorsWebApp.App.Models
{
    public class FailureResponse
    {
        public int StatusCode { get; set; } = 500;
        public string ErrorMessage { get; set; } =
            "Something went wrong while deleting your account, please try again later.";
        public object? ViewModel { get; set; } = null;
    }
}
