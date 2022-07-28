using System;
using System.IO;
using UnityEngine;
using UnityEditor;
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

            DrawToolsWithSheetsServiceProvider();
            EditorGUILayout.Space(8);

            DrawToolsWithServiceAccount();
            EditorGUILayout.Space(8);
        }

        private void DrawToolsWithSheetsServiceProvider()
        {
            EditorGUILayout.LabelField("Tools (using SheetsServiceProvider)", EditorStyles.boldLabel);
            GUILayout.BeginHorizontal();
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
            GUILayout.EndHorizontal();
        }

        private void DrawToolsWithServiceAccount()
        {
            EditorGUILayout.LabelField("Tools (using Google Service Account)", EditorStyles.boldLabel);
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Pull All Locales"))
            {
                PullWithGoogleServiceAccount();
            }

            if (GUILayout.Button("Push All Locales"))
            {
                PushWithGoogleServiceAccount();
            }
            GUILayout.EndHorizontal();
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
    }
}
