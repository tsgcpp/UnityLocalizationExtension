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
        public void FindAssets_WithDefaultAssetArg_ReturnsAsset_IfFolderContainsAsset()
        {
            // Arrange
            var folder = AssetDatabase.LoadAssetAtPath<DefaultAsset>("Assets/Tests/LocalizationExtension/Editor/Resources/Foo02/Bar01/Baz01");

            // Act
            var actual = AssetFinding.FindAssets<TestAsset>(folder);

            // Assert
            Assert.That(actual.Count, Is.EqualTo(2));
            Assert.That(actual[0].Label, Is.EqualTo("02"));
            Assert.That(actual[1].Label, Is.EqualTo("04"));
        }

        [Test]
        public void FindAssets_WithDefaultAssetArg_ReturnsNoAssets_IfArgIsNull()
        {
            // Arrange
            DefaultAsset folder = null;

            // Act
            var actual = AssetFinding.FindAssets<TestAsset>(folder);

            // Assert
            Assert.That(actual.Count, Is.EqualTo(0));
        }

        [Test]
        public void FindAssets_WithListArg_ReturnsAsset_IfFolderContainsAsset_WhenOneFolder()
        {
            // Arrange
            var folders = new List<DefaultAsset>
            {
                AssetDatabase.LoadAssetAtPath<DefaultAsset>("Assets/Tests/LocalizationExtension/Editor/Resources/Foo02/Bar01/Baz01"),
            };

            // Act
            var actual = AssetFinding.FindAssets<TestAsset>(folders);

            // Assert
            Assert.That(actual.Count, Is.EqualTo(2));
            Assert.That(actual[0].Label, Is.EqualTo("02"));
            Assert.That(actual[1].Label, Is.EqualTo("04"));
        }

        [Test]
        public void FindAssets_WithListArg_ReturnsAsset_IfFolderContainsAsset_WhenMultipleFolders()
        {
            // Arrange
            var folders = new List<DefaultAsset>
            {
                AssetDatabase.LoadAssetAtPath<DefaultAsset>("Assets/Tests/LocalizationExtension/Editor/Resources/Foo01"),
                AssetDatabase.LoadAssetAtPath<DefaultAsset>("Assets/Tests/LocalizationExtension/Editor/Resources/Foo02"),
                AssetDatabase.LoadAssetAtPath<DefaultAsset>("Assets/Tests/LocalizationExtension/Editor/Resources/Foo03"),
            };

            // Act
            var actual = AssetFinding.FindAssets<TestAsset>(folders);

            // Assert
            Assert.That(actual.Count, Is.EqualTo(4));
            Assert.That(actual[0].Label, Is.EqualTo("01"));
            Assert.That(actual[1].Label, Is.EqualTo("02"));
            Assert.That(actual[2].Label, Is.EqualTo("04"));
            Assert.That(actual[3].Label, Is.EqualTo("03"));
        }

        [Test]
        public void FindAssets_WithListArg_ReturnsUniquedAsset_IfAssetsAreDupulicated()
        {
            // Arrange
            var folders = new List<DefaultAsset>
            {
                AssetDatabase.LoadAssetAtPath<DefaultAsset>("Assets/Tests/LocalizationExtension/Editor/Resources/Foo02/Bar01"),
                AssetDatabase.LoadAssetAtPath<DefaultAsset>("Assets/Tests/LocalizationExtension/Editor/Resources/Foo02/Bar01/Baz01"),
                AssetDatabase.LoadAssetAtPath<DefaultAsset>("Assets/Tests/LocalizationExtension/Editor/Resources/Foo02"),
            };

            // Act
            var actual = AssetFinding.FindAssets<TestAsset>(folders);

            // Assert
            Assert.That(actual.Count, Is.EqualTo(3));
            Assert.That(actual[0].Label, Is.EqualTo("02"));
            Assert.That(actual[1].Label, Is.EqualTo("04"));
            Assert.That(actual[2].Label, Is.EqualTo("03"));
        }

        [Test]
        public void FindAssets_WithListArg_ReturnsNoAssets_IfAssetsDontExist()
        {
            // Arrange
            var folders = new List<DefaultAsset>
            {
                AssetDatabase.LoadAssetAtPath<DefaultAsset>("Assets/Tests/LocalizationExtension/Editor/Resources/Foo03"),
            };

            // Act
            var actual = AssetFinding.FindAssets<TestAsset>(folders);

            // Assert
            Assert.That(actual.Count, Is.EqualTo(0));
        }

        [Test]
        public void FindAssets_WithListArg_ReturnsNoAssets_IfNullList()
        {
            // Arrange
            var folders = new List<DefaultAsset>
            {
                null,
            };

            // Act
            var actual = AssetFinding.FindAssets<TestAsset>(folders);

            // Assert
            Assert.That(actual.Count, Is.EqualTo(0));
        }

        [Test]
        public void FindAssets_WithListArg_ReturnsNoAssets_IfArgIsNull()
        {
            // Arrange
            List<DefaultAsset> folders = null;

            // Act
            var actual = AssetFinding.FindAssets<TestAsset>(folders);

            // Assert
            Assert.That(actual.Count, Is.EqualTo(0));
        }

        [Test]
        public void FindAssets_WithListArg_ReturnsNoAssets_IfFoldersIsNone()
        {
            // Arrange
            var folders = new List<DefaultAsset>
            {
            };

            // Act
            var actual = AssetFinding.FindAssets<TestAsset>(folders);

            // Assert
            Assert.That(actual.Count, Is.EqualTo(0));
        }
    }
}
