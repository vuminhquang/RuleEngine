using System.Collections;
using RuleEngine.Abstraction.Entities;

namespace RuleEngine.Domain.Ext.RemoteFieldExpr;

public class ForEachExpression : Expression
{
    public Expression CollectionExpression { get; set; }
    public Expression ConditionExpression { get; set; }
    public Expression ActionExpression { get; set; }
    
    // To store to the database
    public int CollectionExpressionId { get; set; }
    public int ConditionExpressionId { get; set; }
    public int ActionExpressionId { get; set; }

    public override object Evaluate(IList<Field> fields)
    {
        var collection = CollectionExpression.Evaluate(fields);
        if (collection is not IEnumerable enumerable)
            throw new ArgumentException("CollectionExpression must evaluate to an IEnumerable.");

        var results = new List<object>();
        foreach (var item in enumerable)
        {
            var tempFields = fields.ToList();
            tempFields.Add(new Field { Name = "CurrentItem", Value = item });

            var conditionResult = ConditionExpression.Evaluate(tempFields);
            if (conditionResult is not (true)) continue;
            var actionResult = ActionExpression.Evaluate(tempFields);
            results.Add(actionResult);
        }

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

            var conditionResult = await ConditionExpression.EvaluateAsync(tempFields);
            if (conditionResult is not (true)) continue;
            var actionResult = await ActionExpression.EvaluateAsync(tempFields);
            results.Add(actionResult);
        }

        return results;
    }
}