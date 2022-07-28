using System.Collections.Generic;
using UnityEditor;

namespace Tsgcpp.Localization.Extension.Editor
{
    public interface ITargetFoldersProvider
    {
        IReadOnlyList<DefaultAsset> TargetFolders { get; }
    }
}
