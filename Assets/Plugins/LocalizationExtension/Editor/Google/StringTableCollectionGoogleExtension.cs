using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Localization;
using UnityEditor.Localization.Plugins.Google;
using UnityEditor.Localization.Reporting;

namespace Tsgcpp.Localization.Extension.Editor.Google
{
    public static class StringTableCollectionGoogleExtension
    {
        public static void PullAllLocales(
            this StringTableCollection collection,
            IGoogleSheetsService serviceProvider,
            bool removeMissingEntries = false,
            ITaskReporter reporter = null,
            bool createUndo = false)
        {
            var sheetsExtension = GetGoogleSheetsExtension(collection);
            var sheets = new GoogleSheets(serviceProvider);

            try
            {
                sheets.PullIntoStringTableCollection(
                    sheetId: sheetsExtension.SheetId,
                    collection: collection,
                    columnMapping: sheetsExtension.Columns,
                    removeMissingEntries: removeMissingEntries,
                    reporter: reporter,
                    createUndo: createUndo);
            }
            catch (Exception e)
            {
                throw new LocalizationExtensionException($"Failed to pull \"{collection.name}\" from Google Sheets.", e);
            }
        }

        public static void PullAllLocales(
            this IEnumerable<StringTableCollection> collections,
            IGoogleSheetsService serviceProvider,
            bool removeMissingEntries = false,
            ITaskReporter reporter = null,
            bool createUndo = false)
        {
            if (collections == null)
            {
                throw new NullReferenceException("collections is null.");
            }

            bool result = true;
            foreach (var collection in collections)
            {
                try
                {
                    collection.PullAllLocales(
                        serviceProvider: serviceProvider,
                        removeMissingEntries: removeMissingEntries,
                        reporter: reporter,
                        createUndo: createUndo);
                }
                catch (Exception e)
                {
                    result = false;
                    UnityEngine.Debug.LogException(e);
                }
            }

            if (!result)
            {
                throw new LocalizationExtensionException("Failed to pull from Google Sheets.");
            }
        }

        public static void PushAllLocales(
            this StringTableCollection collection,
            IGoogleSheetsService serviceProvider,
            ITaskReporter reporter = null)
        {
            var sheetsExtension = GetGoogleSheetsExtension(collection);
            var sheets = new GoogleSheets(serviceProvider);

            try
            {
                sheets.PushStringTableCollection(
                    sheetId: sheetsExtension.SheetId,
                    collection: collection,
                    columnMapping: sheetsExtension.Columns,
                    reporter: reporter);
            }
            catch (Exception e)
            {
                throw new LocalizationExtensionException($"Failed to push \"{collection.name}\" to Google Sheets.", e);
            }
        }

        public static void PushAllLocales(
            this IEnumerable<StringTableCollection> collections,
            IGoogleSheetsService serviceProvider,
            ITaskReporter reporter = null)
        {
            if (collections == null)
            {
                throw new NullReferenceException("collections is null.");
            }

            bool result = true;
            foreach (var collection in collections)
            {
                try
                {
                    collection.PushAllLocales(
                        serviceProvider: serviceProvider,
                        reporter: reporter);
                }
                catch (Exception e)
                {
                    result = false;
                    UnityEngine.Debug.LogException(e);
                }
            }

            if (!result)
            {
                throw new LocalizationExtensionException("Failed to push to Google Sheets.");
            }
        }

        private static GoogleSheetsExtension GetGoogleSheetsExtension(StringTableCollection collection)
        {
            if (!collection)
            {
                throw new LocalizationExtensionException("Invalid StringTableCollection was used.");
            }

            var googleSheetsExtension = collection.Extensions.OfType<GoogleSheetsExtension>().FirstOrDefault();
            if (googleSheetsExtension == null)
            {
                throw new LocalizationExtensionException($"GoogleSheetsExtension is required in \"{collection.name}\".");
            }

            return googleSheetsExtension;
        }
    }
}
