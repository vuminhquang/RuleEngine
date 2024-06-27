using RuleEngine.Abstraction.Entities;
using RuleEngine.Domain;
using RuleEngine.Domain.Ext.RemoteFieldExpr;
using RuleEngine.Infrastructure.Ext.RemoteFieldExpr;

namespace RuleEngineCore;

public partial class Examples
{
    public async Task RemoteFieldExpressionExample()
    {
        var httpClient = new HttpClient();
        var fieldFetcher = new ApiFieldFetcher(httpClient, "https://api.example.com/fields");

        var remoteFieldExpression = new RemoteFieldExpression
        {
            FieldName = "RemoteField",
            FieldFetcher = fieldFetcher
        };

        var fields = new List<Field>();

        var result = await remoteFieldExpression.EvaluateAsync(fields);
        Console.WriteLine($"Remote field value: {result}");
    }
    
    public async Task ForEachExpressionExample()
    {
        // Define a collection of fields
        var fields = new List<Field>
        {
            new() { Name = "Value1", Value = 5 },
            new() { Name = "Value2", Value = 15 },
            new() { Name = "Value3", Value = 25 },
            new() { Name = "Value4", Value = 35 }
        };

        // Define a CollectionExpression to extract the list of field values
        var collectionExpression = new ConstantExpression
        {
            Value = new ConstantExpressionValue { Value = fields.Select(f => f.Value).ToList() }
        };

        // Define a condition to choose fields (e.g., value > 10)
        var conditionToChooseField = new ComparisonExpression
        {
            LeftExpression = new FieldExpression { FieldName = "CurrentItem" },
            RightExpression = new ConstantExpression { Value = new ConstantExpressionValue { Value = 10 } },
            Operator = OperatorType.GreaterThan
        };

        // Define an action to apply on chosen fields (e.g., multiply by 2)
        var actionOnChosenField = new ArithmeticExpression
        {
            LeftExpression = new FieldExpression { FieldName = "CurrentItem" },
            RightExpression = new ConstantExpression { Value = new ConstantExpressionValue { Value = 2 } },
            Operator = ArithmeticOperatorType.Multiply
        };

        // Define the ForEachExpression to filter and apply the action
        var forEachExpression = new ForEachExpression
        {
            CollectionExpression = collectionExpression,
            ConditionExpression = conditionToChooseField,
            ActionExpression = actionOnChosenField
        };

        // Evaluate the ForEachExpression
        var result = (List<object>)await forEachExpression.EvaluateAsync(new List<Field>());
        Console.WriteLine("ForEach result: " + string.Join(", ", result)); // Output: ForEach result: 30, 50, 70
    }
}