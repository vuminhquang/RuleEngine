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

        switch (Operator)
        {
            case ArithmeticOperatorType.Add:
                return leftNum + rightNum;
            case ArithmeticOperatorType.Subtract:
                return leftNum - rightNum;
            case ArithmeticOperatorType.Multiply:
                return leftNum * rightNum;
            case ArithmeticOperatorType.Divide:
                return leftNum / rightNum;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}