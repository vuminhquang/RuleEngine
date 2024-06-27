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
            new() { Name = "Numbers", Value = new List<int> { 1, 2, 3, 4, 5 } }
        };

        // Define a CollectionExpression to extract the list of numbers
        var collectionExpression = new FieldExpression { FieldName = "Numbers" };

        // Define an ItemExpression to multiply each item by 2
        var itemExpression = new ArithmeticExpression
        {
            LeftExpression = new FieldExpression { FieldName = "CurrentItem" },
            RightExpression = new ConstantExpression { Value = new ConstantExpressionValue { Value = 2 } },
            Operator = ArithmeticOperatorType.Multiply
        };

        // Define the ForEachExpression
        var forEachExpression = new ForEachExpression
        {
            CollectionExpression = collectionExpression,
            ItemExpression = itemExpression
        };

        // Evaluate the ForEachExpression
        var result = (List<object>)await forEachExpression.EvaluateAsync(fields);
        Console.WriteLine("ForEach result: " + string.Join(", ", result)); // Output: ForEach result: 2, 4, 6, 8, 10
    }
}