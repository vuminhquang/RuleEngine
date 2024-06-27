using RuleEngine.Domain;
using RuleEngineExpressionVersion.Expression;

namespace RuleEngine.Abstraction.Entities;

public abstract class Expression : IExpression
{
    public int Id { get; set; }
    public ExpressionType Type { get; set; }
    public int? RuleId { get; set; }
    public Rule Rule { get; set; }

    public abstract object Evaluate(IList<Field> fields);

    public virtual Task<object> EvaluateAsync(IList<Field> fields)
    {
        return Task.FromResult(Evaluate(fields));
    }
}