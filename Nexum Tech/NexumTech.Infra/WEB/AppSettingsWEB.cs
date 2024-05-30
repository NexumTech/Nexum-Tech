using System.Runtime.CompilerServices;

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
        public string GetEmployeesURL { get; set; }
        public string RemoveEmployeeURL { get; set; }
        public string AddEmployeeURL { get; set; }
        public string CheckCompanyOwnerURL { get; set; }
        public string CreateDeviceURL { get; set; }
        public string GetDevicesURL { get; set; }
        public string RemoveDeviceURL {  get; set; }
        public string GetCompaniesURL { get; set; }
    }

    public class Fiware
    {
        public string ApiFiwareRealTimeChartURL { get; set; }
        public string ApiFiwareHistoricalChartURL { get; set; }
        public string ApiFiwareProvisioningDeviceURL { get; set; }
        public string ApiFiwareRegisteringDeviceURL { get; set; }
        public string ApiFiwareSubscribingDeviceURL { get; set; }
        public string ApiFiwareRemoveDeviceURL { get; set; }
    }
}
