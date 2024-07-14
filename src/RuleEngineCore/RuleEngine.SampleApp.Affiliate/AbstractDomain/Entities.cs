namespace RuleEngine.SampleApp.Affiliate.AbstractDomain;

public class Affiliate
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Type { get; set; } // Affiliate type as string
    public int? ParentAffiliateId { get; set; }
    public Affiliate ParentAffiliate { get; set; }
    public List<Affiliate> ChildAffiliates { get; set; } = new List<Affiliate>();
}

public class Sale
{
    public int Id { get; set; }
    public double Amount { get; set; }
    public int AffiliateId { get; set; }
    public Affiliate Affiliate { get; set; }
}

public class CommissionLevel
{
    public string Type { get; set; } // Affiliate type as string
    public int Level { get; set; }
    public double Percentage { get; set; }
}