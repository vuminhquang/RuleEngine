using System.ComponentModel.DataAnnotations.Schema;
using RuleEngine.Abstraction.Entities;

namespace RuleEngine.Domain.Ext.RemoteFieldExpr;

public class SwitchExpression : Expression
{
    public Expression Condition { get; set; }
    public int ConditionId { get; set; }
    
    [NotMapped]
    public Dictionary<object, Expression> Cases { get; set; } = new();
        
    [NotMapped]
    public Expression DefaultCase { get; set; }

    public override object Evaluate(IList<Field> fields)
    {
        var conditionValue = Condition.Evaluate(fields);
        if (Cases.TryGetValue(conditionValue, out var caseExpression))
        {
            return caseExpression.Evaluate(fields);
        }

        return DefaultCase?.Evaluate(fields);
    }

    public override async Task<object> EvaluateAsync(IList<Field> fields)
    {
        var conditionValue = await Condition.EvaluateAsync(fields);
        if (Cases.TryGetValue(conditionValue, out var caseExpression))
        {
            return await caseExpression.EvaluateAsync(fields);
        }

        return DefaultCase != null ? await DefaultCase.EvaluateAsync(fields) : null;
    }
}