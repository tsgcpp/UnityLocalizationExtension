using Google.Apis.Sheets.v4;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using UnityEditor.Localization.Plugins.Google;

namespace Tsgcpp.Localization.Extension.Editor.Google
{
    public class ServiceAccountSheetsServiceProvider : IGoogleSheetsService
    {
        private readonly string _serviceAccountKeyJson;
        private readonly string _applicationName;

        public ServiceAccountSheetsServiceProvider(
            string serviceAccountKeyJson,
            string applicationName)
        {
            _serviceAccountKeyJson = serviceAccountKeyJson;
            _applicationName = applicationName;
        }

        public SheetsService Service => GetSheetsService();

        private SheetsService GetSheetsService()
        {
            var credential = GoogleCredential.FromJson(_serviceAccountKeyJson);
            var initializer = new BaseClientService.Initializer
            {
                HttpClientInitializer = credential,
                ApplicationName = _applicationName,
            };
            var sheetsService = new SheetsService(initializer);
            return sheetsService;
        }
    }
}
