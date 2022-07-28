using System;

namespace Tsgcpp.Localization.Extension.Editor
{
    public sealed class LocalizationExtensionException : Exception
    {
        public LocalizationExtensionException(string message) : base(message)
        {
        }

        public LocalizationExtensionException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
