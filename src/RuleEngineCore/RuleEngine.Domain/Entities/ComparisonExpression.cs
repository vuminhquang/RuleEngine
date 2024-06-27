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
        
        // check if leftValue and rightValue are comparable to each other
        if (leftComparable.GetType() != rightComparable.GetType())
        {
            switch (leftComparable)
            {
                case int when rightComparable is double:
                    leftComparable = Convert.ToDouble(leftComparable);
                    break;
                case double when rightComparable is int:
                    rightComparable = Convert.ToDouble(rightComparable);
                    break;
                default:
                    throw new ArgumentException($"Field values are not comparable. Left {leftValue} Right {rightValue}");
            }
        }
        
        var comparisonResult = leftComparable.CompareTo(rightComparable);

        return Operator switch
        {
            OperatorType.Equal => comparisonResult == 0,
            OperatorType.GreaterThan => comparisonResult > 0,
            OperatorType.LessThan => comparisonResult < 0,
            OperatorType.GreaterThanOrEqual => comparisonResult >= 0,
            OperatorType.LessThanOrEqual => comparisonResult <= 0,
            OperatorType.NotEqual => comparisonResult != 0,
            _ => throw new ArgumentOutOfRangeException(nameof(fields),
                $"Invalid comparison operator. The requested operator is {Operator}")
        };
    }
}