using System;
using UnityEngine;
using UnityEditor;
using Tsgcpp.Localization.Extension.Editor.Google;

namespace Tsgcpp.Localization.Extension.Example.Editor
{
    public static class ExampleLocalizationSynchronizationMenu
    {
        #region Pull

        /// <summary>
        /// Environment variable name for Google Sheets API key (Json format).
        /// </summary>
        private const string EnvironmentGoogleServiceAccountKey = "UNITY_LOCALIZATION_GOOGLE_SERVICE_ACCOUNT_KEY";

        [MenuItem("Localization Extension Example/Pull All Localization Tables", false, priority = 1)]
        internal static void PullAllLocalizationTablesMenu()
        {
            PullAllLocalizationTables();
        }

        internal static void PullAllLocalizationTables()
        {
            Bundle.PullAllLocales();
        }

        /// <summary>
        /// Pull all StringTables in StringTableCollectionBundle from Google Spreadsheet.
        /// </summary>
        /// <remarks>
        /// This method can also be used in CI.
        /// </remarks>
        internal static void PullAllLocalizationTablesWithGoogleServiceAccount()
        {
            var bundle = Bundle;
            var provider = GetServiceAccountSheetsServiceProvider(bundle);
            bundle.PullAllLocales(provider);
        }

        #endregion

        #region Push

        [MenuItem("Localization Extension Example/Push All Localization Tables", false, priority = 2)]
        internal static void PushAllLocalizationTablesMenu()
        {
            PushAllLocalizationTables();
        }

        internal static void PushAllLocalizationTables()
        {
            Bundle.PushAllLocales();
        }

        /// <summary>
        /// Push all StringTables in StringTableCollectionBundle to Google Spreadsheet.
        /// </summary>
        /// <remarks>
        /// This method can also be used in CI.
        /// </remarks>
        internal static void PushAllLocalizationTablesWithGoogleServiceAccount()
        {
            var bundle = Bundle;
            var provider = GetServiceAccountSheetsServiceProvider(bundle);
            bundle.PushAllLocales(provider);
        }

        #endregion

        internal static ServiceAccountSheetsServiceProvider GetServiceAccountSheetsServiceProvider(
            StringTableCollectionBundle bundle)
        {
            var serviceAccountKeyJson = Environment.GetEnvironmentVariable(EnvironmentGoogleServiceAccountKey);
            if (string.IsNullOrEmpty(serviceAccountKeyJson))
            {
                throw new InvalidOperationException($"Environment variable \"{EnvironmentGoogleServiceAccountKey}\" is not set.");
            }

            var provider = new ServiceAccountSheetsServiceProvider(
                serviceAccountKeyJson: serviceAccountKeyJson,
                applicationName: bundle.SheetsServiceProvider.ApplicationName);
            return provider;
        }

        private const string BundlePath = "Assets/Example/StringTableCollectionBundle/StringTableCollectionBundle.asset";

        public static StringTableCollectionBundle Bundle => GetStringTableCollectionBundle(BundlePath);

        private static StringTableCollectionBundle GetStringTableCollectionBundle(string path)
        {
            var bundle = AssetDatabase.LoadAssetAtPath<StringTableCollectionBundle>(path);
            if (!bundle)
            {
                throw new NullReferenceException($"StringTableCollectionBundle is not found in \"{path}\"");
            }

            return bundle;
        }

    }
}
