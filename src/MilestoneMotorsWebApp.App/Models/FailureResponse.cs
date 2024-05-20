namespace MilestoneMotorsWebApp.App.Models
{
    public class FailureResponse
    {
        public int StatusCode { get; init; } = 500;
        public string ErrorMessage { get; init; } =
            "Something went wrong while deleting your account, please try again later.";
        public object? ViewModel { get; init; } = null;
    }
}
