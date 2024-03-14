using RuleEngine.Domain;

namespace RuleEngine.Abstraction.Entities;

public class LogicalExpression : Expression
{
    public LogicalOperatorType Operator { get; set; }
    public ICollection<int> OperandExpressionIds { get; set; }
    public ICollection<Expression> OperandExpressions { get; set; }
    
    public override object Evaluate(IList<Field> fields)
    {
        switch (Operator)
        {
            case LogicalOperatorType.And:
                return OperandExpressions.All(operand => (bool)operand.Evaluate(fields));
            case LogicalOperatorType.Or:
                return OperandExpressions.Any(operand => (bool)operand.Evaluate(fields));
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}