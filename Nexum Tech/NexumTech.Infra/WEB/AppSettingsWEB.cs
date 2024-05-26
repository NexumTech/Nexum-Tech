namespace NexumTech.Infra.WEB
{
    public class AppSettingsWEB
    {
        public string ApiBaseURL { get; set; }
        public string AuthenticateURL { get; set; }
        public Fiware Fiware { get; set; }
        public string GoogleAuthURL { get; set; }
        public string SignupURL { get; set; }
        public string GetUserInfoURL { get; set; }
        public string UpdateProfileURL { get; set; }
        public string RequestToChangePasswordURL { get; set; }
        public string PasswordResetTokenValidationURL { get; set; }
        public string UpdatePasswordURL { get; set;}
        public string CreateCompanyURL { get; set; }
        public string GetCompanyURL { get; set; }
        public string UpdateCompanyURL { get; set; }
        public string DeleteCompanyURL { get; set; }
    }

    public class Fiware
    {
        public string ApiFiwareRealTimeChartURL { get; set; }
        public string ApiFiwareHistoricalChartURL { get; set; }
    }
}
