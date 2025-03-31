namespace Astor.Fireblocks.Client.Tests;

[TestClass]
public class GetSupportedAssetsShould : Test
{
    [TestMethod]
    public async Task Work()
    {
        await this.Client.GetSupportedAssets();
    }
}