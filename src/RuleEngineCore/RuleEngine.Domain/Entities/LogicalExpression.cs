using RuleEngine.Domain;

namespace RuleEngine.Abstraction.Entities;

public class LogicalExpression : Expression
{
    public LogicalOperatorType Operator { get; set; }
    public ICollection<int> OperandExpressionIds { get; set; }
    public ICollection<Expression> OperandExpressions { get; set; }

    public override object Evaluate(IList<Field> fields)
    {
        return Operator switch
        {
            LogicalOperatorType.And => OperandExpressions.All(operand => (bool)operand.Evaluate(fields)),
            LogicalOperatorType.Or => OperandExpressions.Any(operand => (bool)operand.Evaluate(fields)),
            _ => throw new ArgumentOutOfRangeException(nameof(fields),
                $"Invalid logical operator. The requested operator is {Operator}")
        };
    }
}