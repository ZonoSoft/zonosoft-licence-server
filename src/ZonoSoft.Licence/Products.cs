namespace ZonoSoft.Licence;

/// <summary>
/// ZonoSoft's software that licences are made for. Add a line here for each new app: its id goes into the licence
/// and its signed token ("prd"), so a key only works in the app it was made for.
/// </summary>
public static class Products
{
    public sealed record Product(string Id, string Name, string Covers);

    public const string Default = "zonosoft-hr";

    public static readonly IReadOnlyList<Product> All =
    [
        new("zonosoft-hr", "ZonoSoft HR", "ZonoSoft HR and the ZonoSoft HR Team app"),
    ];

    /// <summary>Ids products had before they were renamed (Shifenza became ZonoSoft HR in v0.39.0).</summary>
    public static readonly IReadOnlyDictionary<string, string> PreviousIds = new Dictionary<string, string>
    {
        ["shifenza"] = "zonosoft-hr",
    };

    private static string Current(string? id)
    {
        var tidy = (id ?? "").Trim().ToLowerInvariant();
        return PreviousIds.TryGetValue(tidy, out var now) ? now : tidy;
    }

    public static Product? Find(string? id) => All.FirstOrDefault(p => p.Id == Current(id));

    public static string NameOf(string? id) => Find(id)?.Name ?? id ?? "";

    /// <summary>
    /// The product id put in a token for this app version. Apps from before ZonoSoft HR's rename (v0.38 and earlier)
    /// only know the old id, so they're given it until they update; otherwise their licence would stop working.
    /// </summary>
    public static string TokenId(string product, string? appVersion)
    {
        if (product != "zonosoft-hr") return product;
        return Version.TryParse(appVersion, out var v) && v >= new Version(0, 39, 0) ? product : "shifenza";
    }
}
