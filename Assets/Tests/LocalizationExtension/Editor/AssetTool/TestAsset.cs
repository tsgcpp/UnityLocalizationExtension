using UnityEngine;

namespace Tests.Tsgcpp.Localization.Extension.Editor
{
    // [CreateAssetMenu(fileName = nameof(TestAsset), menuName = "Tsgcpp/Tests/TestAssetFinding/" + nameof(TestAsset), order = 1)]
    public sealed class TestAsset : ScriptableObject
    {
        [SerializeField] private string _label;
        public string Label => _label;
    }
}
