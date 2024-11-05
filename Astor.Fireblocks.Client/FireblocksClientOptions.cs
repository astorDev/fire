using System.ComponentModel.DataAnnotations;

namespace Astor.Fireblocks.Client;

public record FireblocksClientOptions
{
    [Required]
    public required string Url { get; set; }

    [Required]
    public required string ApiKey { get; set; }

    [Required]
    public required string ApiSecret { get; set; }
}