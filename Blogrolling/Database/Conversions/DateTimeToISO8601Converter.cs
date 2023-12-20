using System.Globalization;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Blogrolling.Database.Conversions;

public class DateTimeToISO8601Converter(ConverterMappingHints? mappingHints = null)
    : ValueConverter<DateTime?, string?>(AsString, FromString, mappingHints)
{
    private static readonly Expression<Func<DateTime?, string?>> AsString = time => time != null ? time.Value.ToString("O") : null;

    private static readonly Expression<Func<string?, DateTime?>> FromString = s => 
        s != null ? DateTime.Parse(s, CultureInfo.InvariantCulture) : null;
}