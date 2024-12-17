using System.Collections.Immutable;
using System.Text.Json.Serialization;
using MimeDetective.Engine;
using MimeDetective.Storage;

namespace MimeDetective.BlazorWasm;

[JsonSourceGenerationOptions(WriteIndented = true)]
[JsonSerializable(typeof(ImmutableList<Category>))]
[JsonSerializable(typeof(DefinitionMatchSerializable))]
[JsonSerializable(typeof(IEnumerable<DefinitionMatchSerializable>))]
public partial class MySerializerContext : JsonSerializerContext { }

public class DefinitionMatchSerializable(DefinitionMatch DefinitionMatch)
{
    public decimal Percentage { get; set; } = DefinitionMatch.Percentage;

    public long Points { get; set; } = DefinitionMatch.Points;

    public FileType File { get; set; } = DefinitionMatch.Definition.File;
}
