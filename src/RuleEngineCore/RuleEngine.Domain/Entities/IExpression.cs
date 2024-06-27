using RuleEngine.Domain;

namespace RuleEngineExpressionVersion.Expression;

public interface IExpression
{
    object Evaluate(IList<Field> fields);
    Task<object> EvaluateAsync(IList<Field> fields);
}