using RuleEngine.Abstraction.Entities;
using RuleEngine.Domain;
using RuleEngine.SampleApp.Affiliate.AbstractDomain;

namespace RuleEngine.SampleApp.Affiliate.ConcreteBusiness;

public class AffiliateCalculator
{
    private CommissionCalculationExpression _commissionCalculationExpression;

    public void ConfigureCommissionRules()
    {
        var saleAmountExpression = new FieldExpression { FieldName = "Sale.Amount" };
        var commissionLevels = new List<CommissionLevel>
        {
            new CommissionLevel { Type = "Standard", Level = 1, Percentage = 5 },
            new CommissionLevel { Type = "Standard", Level = 2, Percentage = 3 },
            new CommissionLevel { Type = "Premium", Level = 1, Percentage = 10 },
            new CommissionLevel { Type = "Premium", Level = 2, Percentage = 5 },
            new CommissionLevel { Type = "VIP", Level = 1, Percentage = 15 },
            new CommissionLevel { Type = "VIP", Level = 2, Percentage = 7 },
            new CommissionLevel { Type = "VIP", Level = 3, Percentage = 3 }
        };

        _commissionCalculationExpression = new CommissionCalculationExpression
        {
            SaleAmountExpression = saleAmountExpression,
            CommissionLevels = commissionLevels
        };
    }

    public async Task<double> CalculateCommissionAsync(Sale sale, AbstractDomain.Affiliate affiliate)
    {
        var fields = new List<Field>
        {
            new Field { Name = "Sale", Value = sale },
            new Field { Name = "Affiliate", Value = affiliate }
        };

        if (_commissionCalculationExpression == null)
        {
            throw new InvalidOperationException("Commission calculation rule not configured.");
        }

        return (double)await _commissionCalculationExpression.EvaluateAsync(fields);
    }
}