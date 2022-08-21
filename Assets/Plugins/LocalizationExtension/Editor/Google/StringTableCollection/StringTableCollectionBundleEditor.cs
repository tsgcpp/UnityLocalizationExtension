using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;
using UnityEditor.Localization;
using UnityEditor.Localization.Plugins.Google;

namespace Tsgcpp.Localization.Extension.Editor.Google
{
    [CustomEditor(typeof(StringTableCollectionBundle))]
    public sealed class StringTableCollectionBundleEditor : UnityEditor.Editor
    {
        private StringTableCollectionBundle Bundle => target as StringTableCollectionBundle;

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            EditorGUILayout.Space(16);

            DrawToolsWithSheetsServiceProvider();
            EditorGUILayout.Space(8);

            DrawToolsWithServiceAccount();
            EditorGUILayout.Space(8);

            DrawStringTableCollections();
        }

        private void DrawToolsWithSheetsServiceProvider()
        {
            EditorGUILayout.LabelField("Tools (using SheetsServiceProvider)", EditorStyles.boldLabel);
            using var h = new EditorGUILayout.HorizontalScope();
            if (GUILayout.Button("Pull All Locales"))
            {
                var serviceProvider = GetSheetsServiceProvider();
                Bundle.PullAllLocales(serviceProvider: serviceProvider);
            }

            if (GUILayout.Button("Push All Locales"))
            {
                var serviceProvider = GetSheetsServiceProvider();
                Bundle.PushAllLocales(serviceProvider: serviceProvider);
            }
        }

        private void DrawToolsWithServiceAccount()
        {
            EditorGUILayout.LabelField("Tools (using Google Service Account)", EditorStyles.boldLabel);
            using var h = new EditorGUILayout.HorizontalScope();
            if (GUILayout.Button("Pull All Locales"))
            {
                PullWithGoogleServiceAccount();
            }

            if (GUILayout.Button("Push All Locales"))
            {
                PushWithGoogleServiceAccount();
            }
        }

        private bool _showStringTableCollections = true;
        private readonly CacheListConverter<DefaultAsset, StringTableCollection> _stringTableCollectionsConverter =
            new CacheListConverter<DefaultAsset, StringTableCollection>(
                actualConverter: new FolderToCollectionConverter());

        private void DrawStringTableCollections()
        {
            var foldoutStyle = new GUIStyle(EditorStyles.foldout)
            {
                fontStyle = FontStyle.Bold,
            };

            _showStringTableCollections = EditorGUILayout.Foldout(_showStringTableCollections, "Target \"StringTableCollection\"s", foldoutStyle);
            if (!_showStringTableCollections)
            {
                return;
            }

            using var h = new EditorGUILayout.VerticalScope(GUI.skin.box);
            using var g = new EditorGUI.DisabledGroupScope(true);

            var stringTableCollections = _stringTableCollectionsConverter.Convert(Bundle.TargetFolders);
            foreach (var collection in stringTableCollections)
            {
                EditorGUILayout.ObjectField(collection, typeof(StringTableCollection), allowSceneObjects: false);
            }
        }

        private void PullWithGoogleServiceAccount()
        {
            var serviceProvider = CreateServiceAccountSheetsServiceProvider();
            Bundle.PullAllLocales(serviceProvider: serviceProvider);
        }

        private void PushWithGoogleServiceAccount()
        {
            var serviceProvider = CreateServiceAccountSheetsServiceProvider();
            Bundle.PushAllLocales(serviceProvider: serviceProvider);
        }

        private SheetsServiceProvider GetSheetsServiceProvider()
        {
            var sheetsSesrvicesProvider = Bundle.SheetsServiceProvider;
            if (!sheetsSesrvicesProvider)
            {
                EditorUtility.DisplayDialog(
                    title: "SheetsServiceProvider is not set.",
                    message: $"Set SheetsServiceProvider in \"{Bundle.name}\"",
                    ok: "OK");
                throw new NullReferenceException($"sheetsSesrvicesProvider in \"{Bundle.name}\" is null.");
            }

            return sheetsSesrvicesProvider;
        }

        private ServiceAccountSheetsServiceProvider CreateServiceAccountSheetsServiceProvider()
        {
            var sheetsSesrvicesProvider = GetSheetsServiceProvider();
            string path = EditorUtility.OpenFilePanel(
                title: "Specify Google Service Account Key",
                directory: "Assets",
                extension: "json");

            if (string.IsNullOrWhiteSpace(path))
            {
                throw new NullReferenceException($"Google Service Account Key was not specified.");
            }

            string keyJson = File.ReadAllText(path);
            var provider = new ServiceAccountSheetsServiceProvider(
                serviceAccountKeyJson: keyJson,
                applicationName: sheetsSesrvicesProvider.ApplicationName);
            return provider;
        }

        private sealed class FolderToCollectionConverter : IListConverter<DefaultAsset, StringTableCollection>
        {
            public IReadOnlyList<StringTableCollection> Convert(IReadOnlyList<DefaultAsset> list)
            {
                return AssetFinding.FindAssets<StringTableCollection>(list);
            }
        }
    }
}
