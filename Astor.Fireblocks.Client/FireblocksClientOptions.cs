using System.ComponentModel.DataAnnotations;

namespace Astor.Fireblocks.Client;

public record FireblocksClientOptions
{
    [Required]
    public string Url { get; set; }

    [Required]
    public string ApiKey { get; set; }

    [Required]
    public string ApiSecret { get; set; }
}