using System.Collections;
using RuleEngine.Abstraction.Entities;

namespace RuleEngine.Domain.Ext.RemoteFieldExpr;

public class ForEachExpression : Expression
{
    public Expression CollectionExpression { get; set; }
    public Expression ItemExpression { get; set; }

    public override object Evaluate(IList<Field> fields)
    {
        var collection = CollectionExpression.Evaluate(fields);
        if (collection is not IEnumerable<object> enumerable)
            throw new ArgumentException("CollectionExpression must evaluate to an IEnumerable.");

        var results = enumerable.Select(item =>
        {
            var tempFields = fields.ToList();
            tempFields.Add(new Field { Name = "CurrentItem", Value = item });
            return ItemExpression.Evaluate(tempFields);
        }).ToList();

        return results;
    }

    public override async Task<object> EvaluateAsync(IList<Field> fields)
    {
        var collection = await CollectionExpression.EvaluateAsync(fields);
        
        if (collection is not IEnumerable enumerable)
            throw new ArgumentException("CollectionExpression must evaluate to an IEnumerable.");
        
        var results = new List<object>();
        foreach (var item in enumerable)
        {
            var tempFields = fields.ToList();
            tempFields.Add(new Field { Name = "CurrentItem", Value = item });
            var result = await ItemExpression.EvaluateAsync(tempFields);
            results.Add(result);
        }

        return results;
    }
}