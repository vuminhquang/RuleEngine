using RuleEngine.SampleApp.Affiliate.AbstractDomain;
using RuleEngine.SampleApp.Affiliate.ConcreteBusiness;

await RunAffiliateCalculator();

static async Task RunAffiliateCalculator()
{
    var affiliateCalculator = new AffiliateCalculator();

    // Configure commission rules
    affiliateCalculator.ConfigureCommissionRules();

    // Example affiliates
    var topAffiliate = new Affiliate { Id = 1, Name = "Top Affiliate", Type = "VIP" };
    var midAffiliate = new Affiliate { Id = 2, Name = "Mid Affiliate", Type = "Premium", ParentAffiliate = topAffiliate };
    var lowAffiliate = new Affiliate { Id = 3, Name = "Low Affiliate", Type = "Standard", ParentAffiliate = midAffiliate };

    // Example sale
    var sale = new Sale { Id = 1, Amount = 1000, Affiliate = lowAffiliate };

    // Calculate commission
    var commission = await affiliateCalculator.CalculateCommissionAsync(sale, lowAffiliate);
    Console.WriteLine($"Total commission for the sale: {commission}");
}