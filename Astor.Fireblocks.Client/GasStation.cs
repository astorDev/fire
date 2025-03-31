namespace Astor.Fireblocks.Client;

public partial class FireblocksUris
{
    public const string GasStation = "gas_station";
    public static string V1GasStation => $"{V1}/{GasStation}";
}

public partial class FireblocksClient
{
    public async Task<GasStation> GetGasStation() => await GetAsync<GasStation>(FireblocksUris.V1GasStation);
}

public record GasStation(
    Dictionary<string, string> Balance,
    GasStationConfiguration Configuration
);

public record GasStationConfiguration(
    string GasThreshold,
    string GasCap,
    string? MaxGasPrice
);