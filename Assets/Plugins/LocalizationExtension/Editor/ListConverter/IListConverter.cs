using System.Collections.Generic;

namespace Tsgcpp.Localization.Extension.Editor
{
    public interface IListConverter<T, TResult>
        where T : class
        where TResult : class
    {
        IReadOnlyList<TResult> Convert(IReadOnlyList<T> list);
    }
}
