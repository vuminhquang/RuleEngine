using RuleEngine.Domain;

namespace RuleEngine.Abstraction.Entities;

public class ArithmeticExpression : Expression
{
    public int LeftExpressionId { get; set; }
    public Expression LeftExpression { get; set; }
    public int RightExpressionId { get; set; }
    public Expression RightExpression { get; set; }
    public ArithmeticOperatorType Operator { get; set; }

    public override object Evaluate(IList<Field> fields)
    {
        var leftValue = LeftExpression.Evaluate(fields);
        var rightValue = RightExpression.Evaluate(fields);

        if (leftValue is not IConvertible leftConvertible || rightValue is not IConvertible rightConvertible)
            throw new ArgumentException("Field values are not convertible to numbers.");
        var leftNum = Convert.ToDouble(leftConvertible);
        var rightNum = Convert.ToDouble(rightConvertible);

        return Operator switch
        {
            ArithmeticOperatorType.Add => leftNum + rightNum,
            ArithmeticOperatorType.Subtract => leftNum - rightNum,
            ArithmeticOperatorType.Multiply => leftNum * rightNum,
            ArithmeticOperatorType.Divide => leftNum / rightNum,
            _ => throw new ArgumentOutOfRangeException(nameof(fields),
                $"Invalid arithmetic operator. The requested operator is {Operator}")
        };
    }
}