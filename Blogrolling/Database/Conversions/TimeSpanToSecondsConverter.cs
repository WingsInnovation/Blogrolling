using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Blogrolling.Database.Conversions;

public class TimeSpanToSecondsConverter(
    ConverterMappingHints? mappingHints = null)
    : ValueConverter<TimeSpan?, long?>(AsSeconds, FromSeconds, mappingHints)
{
    private static readonly Expression<Func<TimeSpan?, long?>> AsSeconds = time => time != null ? time.Value.Seconds : null;

    private static readonly Expression<Func<long?, TimeSpan?>> FromSeconds = seconds => seconds != null ? TimeSpan.FromSeconds(seconds.Value) : null;
}