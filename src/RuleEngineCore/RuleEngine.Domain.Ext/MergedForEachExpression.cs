using System.Collections;
using RuleEngine.Abstraction.Entities;

namespace RuleEngine.Domain.Ext.RemoteFieldExpr;

/// <summary>
/// ForEach Expression but merges the Collection and filter Condition in to a single expression
/// </summary>
public class MergedForEachExpression : Expression
{
    public Expression FilteredCollectionExpression { get; set; }
    public Expression ActionExpression { get; set; }

    public override object Evaluate(IList<Field> fields)
    {
        var filteredCollection = FilteredCollectionExpression.Evaluate(fields);
        if (filteredCollection is not IEnumerable<object> enumerable)
            throw new ArgumentException("FilteredCollectionExpression must evaluate to an IEnumerable.");

        var results = new List<object>();
        foreach (var item in enumerable)
        {
            var tempFields = fields.ToList();
            tempFields.Add(new Field { Name = "CurrentItem", Value = item });

            var actionResult = ActionExpression.Evaluate(tempFields);
            results.Add(actionResult);
        }

        return results;
    }

    public override async Task<object> EvaluateAsync(IList<Field> fields)
    {
        var filteredCollection = await FilteredCollectionExpression.EvaluateAsync(fields);

        if (filteredCollection is not IEnumerable enumerable)
            throw new ArgumentException("FilteredCollectionExpression must evaluate to an IEnumerable.");

        var results = new List<object>();
        foreach (var item in enumerable)
        {
            var tempFields = fields.ToList();
            tempFields.Add(new Field { Name = "CurrentItem", Value = item });

            var actionResult = await ActionExpression.EvaluateAsync(tempFields);
            results.Add(actionResult);
        }

        return results;
    }
}