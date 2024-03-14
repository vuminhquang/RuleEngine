using RuleEngine.Abstraction;
using RuleEngine.Abstraction.Entities;
using RuleEngine.Domain;

namespace RuleEngineCore;

public partial class Examples
{
    public void ExpressionExample()
    {
        var ageExpression = new FieldExpression { FieldName = "Age" };
        var levelExpression = new FieldExpression { FieldName = "Level" };
        var incomeExpression = new FieldExpression { FieldName = "Income" };
    
        var leftExpression = new ArithmeticExpression
        {
            LeftExpression = new ArithmeticExpression
            {
                LeftExpression = ageExpression,
                RightExpression = levelExpression,
                Operator = ArithmeticOperatorType.Add
            },
            RightExpression = levelExpression,
            Operator = ArithmeticOperatorType.Multiply
        };
    
        var rightExpression = new ArithmeticExpression
        {
            LeftExpression = incomeExpression,
            RightExpression = levelExpression,
            Operator = ArithmeticOperatorType.Divide
        };
    
        var condition = new ComparisonExpression
        {
            LeftExpression = leftExpression,
            RightExpression = rightExpression,
            Operator = OperatorType.LessThan
        };
    
        var fields = new List<Field>
        {
            new() { Name = "Age", Value = 30 },
            new() { Name = "Level", Value = 2 },
            new() { Name = "Income", Value = 80000 }
        };
    
    
        // Evaluate the condition
        var result = (bool)condition.Evaluate(fields);
        Console.WriteLine($"Result: {result}"); // Output: Result: True
        
        var condition2 = new ComparisonExpression
        {
            LeftExpression = leftExpression,
            RightExpression = rightExpression,
            Operator = OperatorType.GreaterThan
        };
        var result2 = (bool)condition2.Evaluate(fields);
        Console.WriteLine($"Result: {result2}"); // Output: Result: False
    }
}