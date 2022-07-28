using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Localization;
using UnityEditor.Localization.Plugins.Google;
using UnityEditor.Localization.Reporting;

namespace Tsgcpp.Localization.Extension.Editor
{
    [CreateAssetMenu(fileName = nameof(StringTableCollectionBundle), menuName = "Tsgcpp/Localization Extension/" + nameof(StringTableCollectionBundle), order = 1)]
    public sealed class StringTableCollectionBundle : ScriptableObject, ITargetFoldersProvider
    {
        [SerializeField] private List<DefaultAsset> _targetFolders;
        public IReadOnlyList<DefaultAsset> TargetFolders => _targetFolders;

        public IReadOnlyList<StringTableCollection> StringTableCollections =>
            AssetFinding.FindAssetsInFolders<StringTableCollection>(TargetFolders);

        public void PullAllLocales(
            IGoogleSheetsService serviceProvider,
            bool removeMissingEntries = false,
            ITaskReporter reporter = null,
            bool createUndo = false)
        {
            StringTableCollections.PullAllLocales(
                serviceProvider: serviceProvider,
                removeMissingEntries: removeMissingEntries,
                reporter: reporter,
                createUndo: createUndo);
        }

        public void PushAllLocales(
            IGoogleSheetsService serviceProvider,
            bool removeMissingEntries = false,
            ITaskReporter reporter = null,
            bool createUndo = false)
        {
            StringTableCollections.PushAllLocales(
                serviceProvider: serviceProvider,
                reporter: reporter);
        }
    }
}
