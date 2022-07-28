using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;
using UnityEditor.Localization;
using UnityEditor.Localization.Plugins.Google;
using UnityEditor.Localization.Reporting;

namespace Tsgcpp.Localization.Extension.Editor.Google
{
    [CreateAssetMenu(fileName = nameof(StringTableCollectionBundle), menuName = "Tsgcpp/Localization Extension/" + nameof(StringTableCollectionBundle), order = 1)]
    public sealed class StringTableCollectionBundle : ScriptableObject, ITargetFoldersProvider
    {
        [Tooltip("SheetsServiceProvider for pull and push.")]
        [SerializeField] private SheetsServiceProvider _sheetsServiceProvider;
        public SheetsServiceProvider SheetsServiceProvider => _sheetsServiceProvider;

        [Tooltip("The folders to search for StringTableCollection assets.")]
        [SerializeField] private List<DefaultAsset> _targetFolders;
        public IReadOnlyList<DefaultAsset> TargetFolders => _targetFolders;

        public IReadOnlyList<StringTableCollection> StringTableCollections =>
            AssetFinding.FindAssetsInFolders<StringTableCollection>(TargetFolders);

        public void PullAllLocales(
            bool removeMissingEntries = false,
            ITaskReporter reporter = null,
            bool createUndo = false)
        {
            PullAllLocales(
                serviceProvider: SheetsServiceProvider,
                removeMissingEntries: removeMissingEntries,
                reporter: reporter,
                createUndo: createUndo);
        }

        public void PullAllLocales(
            IGoogleSheetsService serviceProvider,
            bool removeMissingEntries = false,
            ITaskReporter reporter = null,
            bool createUndo = false)
        {
            ValidateProperties();
            StringTableCollections.PullAllLocales(
                serviceProvider: serviceProvider,
                removeMissingEntries: removeMissingEntries,
                reporter: reporter,
                createUndo: createUndo);
        }

        public void PushAllLocales(ITaskReporter reporter = null)
        {
            PushAllLocales(
                serviceProvider: SheetsServiceProvider,
                reporter: reporter);
        }

        public void PushAllLocales(
            IGoogleSheetsService serviceProvider,
            ITaskReporter reporter = null)
        {
            ValidateProperties();
            StringTableCollections.PushAllLocales(
                serviceProvider: serviceProvider,
                reporter: reporter);
        }

        private void ValidateProperties()
        {
            if (TargetFolders == null)
            {
                throw new NullReferenceException("TargetFolders is null.");
            }

            if (TargetFolders.Any(folder => folder == null))
            {
                throw new NullReferenceException("TargetFolders contains null.");
            }

            if (TargetFolders.Any(folder => !AssetDatabase.IsValidFolder(AssetDatabase.GetAssetPath(folder))))
            {
                throw new ArgumentException("TargetFolders contains invalid folder.");
            }
        }
    }
}
