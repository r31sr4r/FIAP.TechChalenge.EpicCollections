using FIAP.TechChalenge.EpicCollections.Api.Extensions.String;
using System.Text.Json;

namespace FIAP.TechChalenge.EpicCollections.Api.Configurations.Policies;

public class JsonSnakeCasePolicy : JsonNamingPolicy
{
    public override string ConvertName(string name)
        => name.ToSnakeCase();
}
