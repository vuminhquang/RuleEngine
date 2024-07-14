using RuleEngine.Abstraction.Entities;
using RuleEngine.Domain;

namespace RuleEngine.SampleApp.Affiliate.AbstractDomain;

using RuleEngine.Abstraction.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class CommissionCalculationExpression : Expression
{
    public Expression SaleAmountExpression { get; set; }
    public List<CommissionLevel> CommissionLevels { get; set; }

    public override object Evaluate(IList<Field> fields)
    {
        var saleAmount = (double)SaleAmountExpression.Evaluate(fields);
        var affiliate = fields.FirstOrDefault(f => f.Name == "Affiliate")?.Value as Affiliate;

        return CalculateCommission(affiliate, saleAmount, CommissionLevels);
    }

    public override async Task<object> EvaluateAsync(IList<Field> fields)
    {
        var saleAmount = (double)await SaleAmountExpression.EvaluateAsync(fields);
        var affiliate = fields.FirstOrDefault(f => f.Name == "Affiliate")?.Value as Affiliate;

        return CalculateCommission(affiliate, saleAmount, CommissionLevels);
    }

    private double CalculateCommission(Affiliate affiliate, double saleAmount, List<CommissionLevel> commissionLevels)
    {
        double totalCommission = 0;
        int level = 0;

        while (affiliate != null && level < commissionLevels.Count)
        {
            var commissionLevel = commissionLevels.FirstOrDefault(cl => cl.Type == affiliate.Type && cl.Level == level + 1);
            if (commissionLevel != null)
            {
                totalCommission += saleAmount * (commissionLevel.Percentage / 100);
            }
            affiliate = affiliate.ParentAffiliate;
            level++;
        }

        return totalCommission;
    }
}