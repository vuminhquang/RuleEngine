using RuleEngine.Domain;

namespace RuleEngine.Abstraction.Entities;

public class ComparisonExpression : Expression
{
    public int LeftExpressionId { get; set; }
    public Expression LeftExpression { get; set; }
    public int RightExpressionId { get; set; }
    public Expression RightExpression { get; set; }
    public OperatorType Operator { get; set; }
    
    public override object Evaluate(IList<Field> fields)
    {
        var leftValue = LeftExpression.Evaluate(fields);
        var rightValue = RightExpression.Evaluate(fields);

        if (leftValue is not IComparable leftComparable || rightValue is not IComparable rightComparable)
            throw new ArgumentException("Field values are not comparable.");
        var comparisonResult = leftComparable.CompareTo(rightComparable);

        switch (Operator)
        {
            case OperatorType.Equal:
                return comparisonResult == 0;
            case OperatorType.GreaterThan:
                return comparisonResult > 0;
            case OperatorType.LessThan:
                return comparisonResult < 0;
            case OperatorType.GreaterThanOrEqual:
                return comparisonResult >= 0;
            case OperatorType.LessThanOrEqual:
                return comparisonResult <= 0;
            case OperatorType.NotEqual:
                return comparisonResult != 0;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}