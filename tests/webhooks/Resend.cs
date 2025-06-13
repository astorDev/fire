namespace Fire.Blocks.Tests;

[TestClass]
public class Resend : Test
{
    [TestMethod]
    public async Task ResendWebhooks()
    {
        var resended = await Client.PostWebhooksResend();
        resended.MessageCount.ShouldBeGreaterThan(-1);
    }
}