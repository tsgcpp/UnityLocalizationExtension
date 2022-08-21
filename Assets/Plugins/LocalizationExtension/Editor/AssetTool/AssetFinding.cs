using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using UnityEditor;

namespace Tsgcpp.Localization.Extension.Editor
{
    public static class AssetFinding
    {
        public static IReadOnlyList<T> FindAssets<T>(DefaultAsset folder) where T : UnityEngine.Object
        {
            return FindAssets<T>(new List<DefaultAsset> { folder });
        }

        public static IReadOnlyList<T> FindAssets<T>(IReadOnlyList<DefaultAsset> folders) where T : UnityEngine.Object
        {
            if (folders == null)
            {
                return new List<T>();
            }

            var folderPathList = folders
                .Where(folder => folder != null)
                .Select(AssetDatabase.GetAssetPath)
                .Where(path => !string.IsNullOrEmpty(path))
                .Where(path => Directory.Exists(path))
                .ToList();

            if (folderPathList.Count <= 0)
            {
                return new List<T>();
            }

            var assets = AssetDatabase
                .FindAssets($"t:{typeof(T).Name}", searchInFolders: folderPathList.ToArray())
                .Select(AssetDatabase.GUIDToAssetPath)
                .OrderBy(path => path)
                .Distinct()
                .Select(AssetDatabase.LoadAssetAtPath<T>)
                .ToList();

            return assets;
        }
    }
}
