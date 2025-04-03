namespace Fire.Blocks;

[TestClass]
public class GetTransactionsShould : Test
{
    [TestMethod]
    public async Task ReturnLimitedTransactionsBefore()
    {
        var transactions = await this.Client.GetTransactions(new(
            Before: FireblocksTime.NowPlus(TimeSpan.FromSeconds(10)),
            Limit: 3
        ));
        
        Console.WriteLine(transactions.Length);
    }
}