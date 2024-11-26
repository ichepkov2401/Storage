using System.Collections;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace Storage.BusinessLogic;

public class Grouping<TKey, TValue>(TKey key, List<TValue> values) : IGrouping<TKey, TValue>
{
    public IEnumerator<TValue> GetEnumerator()
        => values.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
        => values.GetEnumerator();

    public TKey Key { get; } = key;
}