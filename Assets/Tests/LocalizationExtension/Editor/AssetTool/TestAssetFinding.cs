using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEditor;
using Tsgcpp.Localization.Extension.Editor;

namespace Tests.Tsgcpp.Localization.Extension.Editor
{
    public class TestAssetFinding
    {
        [Test]
        public void FindAssetsInFolders_ReturnsTextAsset_IfFolderContainsTextAsset_WhenOneFolder()
        {
            // Arrange
            var folders = new List<DefaultAsset>
            {
                AssetDatabase.LoadAssetAtPath<DefaultAsset>("Assets/Tests/LocalizationExtension/Editor/Resources/Foo02/Bar01/Baz01"),
            };

            // Act
            var actual = AssetFinding.FindAssetsInFolders<TextAsset>(folders);

            // Assert
            Assert.That(actual.Count, Is.EqualTo(2));
            Assert.That(actual[0].text, Is.EqualTo("02"));
            Assert.That(actual[1].text, Is.EqualTo("04"));
        }

        [Test]
        public void FindAssetsInFolders_ReturnsTextAsset_IfFolderContainsTextAsset_WhenMultipleFolders()
        {
            // Arrange
            var folders = new List<DefaultAsset>
            {
                AssetDatabase.LoadAssetAtPath<DefaultAsset>("Assets/Tests/LocalizationExtension/Editor/Resources/Foo01"),
                AssetDatabase.LoadAssetAtPath<DefaultAsset>("Assets/Tests/LocalizationExtension/Editor/Resources/Foo02"),
                AssetDatabase.LoadAssetAtPath<DefaultAsset>("Assets/Tests/LocalizationExtension/Editor/Resources/Foo03"),
            };

            // Act
            var actual = AssetFinding.FindAssetsInFolders<TextAsset>(folders);

            // Assert
            Assert.That(actual.Count, Is.EqualTo(4));
            Assert.That(actual[0].text, Is.EqualTo("01"));
            Assert.That(actual[1].text, Is.EqualTo("02"));
            Assert.That(actual[2].text, Is.EqualTo("04"));
            Assert.That(actual[3].text, Is.EqualTo("03"));
        }

        [Test]
        public void FindAssetsInFolders_ReturnsUniquedTextAsset_IfTextAssetsAreDupulicated()
        {
            // Arrange
            var folders = new List<DefaultAsset>
            {
                AssetDatabase.LoadAssetAtPath<DefaultAsset>("Assets/Tests/LocalizationExtension/Editor/Resources/Foo02/Bar01"),
                AssetDatabase.LoadAssetAtPath<DefaultAsset>("Assets/Tests/LocalizationExtension/Editor/Resources/Foo02/Bar01/Baz01"),
                AssetDatabase.LoadAssetAtPath<DefaultAsset>("Assets/Tests/LocalizationExtension/Editor/Resources/Foo02"),
            };

            // Act
            var actual = AssetFinding.FindAssetsInFolders<TextAsset>(folders);

            // Assert
            Assert.That(actual.Count, Is.EqualTo(3));
            Assert.That(actual[0].text, Is.EqualTo("02"));
            Assert.That(actual[1].text, Is.EqualTo("04"));
            Assert.That(actual[2].text, Is.EqualTo("03"));
        }

        [Test]
        public void FindAssetsInFolders_ReturnsNoTextAssets_IfTextAssetsDontExist()
        {
            // Arrange
            var folders = new List<DefaultAsset>
            {
                AssetDatabase.LoadAssetAtPath<DefaultAsset>("Assets/Tests/LocalizationExtension/Editor/Resources/Foo03"),
            };

            // Act
            var actual = AssetFinding.FindAssetsInFolders<TextAsset>(folders);

            // Assert
            Assert.That(actual.Count, Is.EqualTo(0));
        }

        [Test]
        public void FindAssetsInFolders_ReturnsNoTextAssets_IfFoldersIsNone()
        {
            // Arrange
            var folders = new List<DefaultAsset>
            {
            };

            // Act
            var actual = AssetFinding.FindAssetsInFolders<TextAsset>(folders);

            // Assert
            Assert.That(actual.Count, Is.EqualTo(0));
        }

        [Test]
        public void FindAssetsInFolders_ThrowsNullReferenceException_IfArgIsNull()
        {
            Assert.Throws<NullReferenceException>(() => AssetFinding.FindAssetsInFolders<TextAsset>(null));
        }
    }
}
