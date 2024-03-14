using RuleEngine.Abstraction;
using RuleEngine.Domain;

namespace RuleEngineExpressionVersion.Expression;

public interface IExpression
{
    public object Evaluate(IList<Field> fields);
}