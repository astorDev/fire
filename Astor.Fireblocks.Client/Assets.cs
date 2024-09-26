namespace Astor.Fireblocks.Client;

public partial class FireblocksUris
{
    public const string SupportedAssets = "supported_assets";
    public static string V1SupportedAssets => $"{V1}/{SupportedAssets}";
}

public partial class FireblocksClient
{
    public async Task<AssetType[]> GetSupportedAssets() => await GetAsync<AssetType[]>(FireblocksUris.V1SupportedAssets);
}

public record AssetType(
    string Name
);