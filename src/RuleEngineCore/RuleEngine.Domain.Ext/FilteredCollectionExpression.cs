using System.ComponentModel.DataAnnotations.Schema;
using RuleEngine.Abstraction.Entities;

namespace RuleEngine.Domain.Ext.RemoteFieldExpr;

/// <summary>
/// Helper class to Merge the Collection and filter Condition in to a single expression
/// </summary>
public class FilteredCollectionExpression : Expression
{
    [NotMapped]
    public List<object> Collection { get; set; }
    public Expression Condition { get; set; }
    
    public int ConditionId { get; set; }

    public override object Evaluate(IList<Field> fields)
    {
        var results = new List<object>();
        foreach (var item in Collection)
        {
            var tempFields = fields.ToList();
            tempFields.Add(new Field { Name = "CurrentItem", Value = item });

            var conditionResult = Condition.Evaluate(tempFields);
            if (conditionResult is not (true)) continue;
            results.Add(item);
        }

        return results;
    }

    public override async Task<object> EvaluateAsync(IList<Field> fields)
    {
        var results = new List<object>();
        foreach (var item in Collection)
        {
            var tempFields = fields.ToList();
            tempFields.Add(new Field { Name = "CurrentItem", Value = item });

            var conditionResult = await Condition.EvaluateAsync(tempFields);
            if (conditionResult is not (true)) continue;
            results.Add(item);
        }

        return results;
    }
}