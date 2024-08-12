namespace CompetitionTask.Models
{
    public class LoginModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class TestCaseData
    {
        public string TestCase { get; set; }
        public LoginModel Data { get; set; }
    }
}
