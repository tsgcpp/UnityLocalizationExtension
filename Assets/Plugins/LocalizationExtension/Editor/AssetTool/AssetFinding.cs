using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using UnityEditor;

namespace Tsgcpp.Localization.Extension.Editor
{
    public static class AssetFinding
    {
        public static IReadOnlyList<T> FindAssetsInFolders<T>(IReadOnlyList<DefaultAsset> folders) where T : UnityEngine.Object
        {
            if (folders == null)
            {
                throw new NullReferenceException("folders is null.");
            }

            var folderPathList = folders
                .Where(folder => folder != null)
                .Select(AssetDatabase.GetAssetPath)
                .Where(path => !string.IsNullOrEmpty(path))
                .Where(path => Directory.Exists(path))
                .ToList();

            var assets = AssetDatabase
                .FindAssets($"t:{typeof(T).Name}")
                .Select(AssetDatabase.GUIDToAssetPath)
                .Where(path => folderPathList.Any(folderPath => path.StartsWith(folderPath)))
                .OrderBy(path => path)
                .Distinct()
                .Select(AssetDatabase.LoadAssetAtPath<T>)
                .ToList();

            return assets;
        }
    }
}
