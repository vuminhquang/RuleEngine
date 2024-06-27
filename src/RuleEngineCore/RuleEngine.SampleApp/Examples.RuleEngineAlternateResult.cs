using RuleEngine.Abstraction.Entities;
using RuleEngine.Domain;
using RuleEngine.Domain.Ext.RemoteFieldExpr;

namespace RuleEngineCore;

public partial class Examples
{
    public async Task AlternateResultExample()
    {
        // After check, instead of just write OK, check the result, if:
        // Name is Sara then "OK", John, Mike or "Tom" then No, "Anna" then "yes"
        
        // Define a collection of persons
        var persons = new List<Person>
        {
            new() { Name = "John", Age = 18, Nation = "America", Salary = 2500 },
            new() { Name = "Anna", Age = 17, Nation = "Canada", Salary = 1500 },
            new() { Name = "Sara", Age = 20, Nation = "America", Salary = 3000 },
            new() { Name = "Mike", Age = 15, Nation = "America", Salary = 1000 },
            new() { Name = "Tom", Age = 25, Nation = "Australia", Salary = 4000 }
        };

        // Convert persons to a list of objects
        var personObjects = persons.Cast<object>().ToList();

        // Define a FilteredCollectionExpression to filter persons based on the given conditions
        var filteredCollectionExpression = new FilteredCollectionExpression
        {
            Collection = personObjects,
            Condition = new LogicalExpression
            {
                Operator = LogicalOperatorType.Or,
                OperandExpressions = new List<Expression>
                {
                    new LogicalExpression
                    {
                        Operator = LogicalOperatorType.And,
                        OperandExpressions = new List<Expression>
                        {
                            new ComparisonExpression
                            {
                                LeftExpression = new FieldExpression { FieldName = "CurrentItem.Age" },
                                RightExpression = new ConstantExpression { Value = new ConstantExpressionValue { Value = 16 } },
                                Operator = OperatorType.GreaterThan
                            },
                            new ComparisonExpression
                            {
                                LeftExpression = new FieldExpression { FieldName = "CurrentItem.Nation" },
                                RightExpression = new ConstantExpression { Value = new ConstantExpressionValue { Value = "America" } },
                                Operator = OperatorType.Equal
                            }
                        }
                    },
                    new LogicalExpression
                    {
                        Operator = LogicalOperatorType.And,
                        OperandExpressions = new List<Expression>
                        {
                            new ComparisonExpression
                            {
                                LeftExpression = new FieldExpression { FieldName = "CurrentItem.Salary" },
                                RightExpression = new ConstantExpression { Value = new ConstantExpressionValue { Value = 2000 } },
                                Operator = OperatorType.GreaterThan
                            },
                            new ComparisonExpression
                            {
                                LeftExpression = new ArithmeticExpression
                                {
                                    LeftExpression = new FieldExpression { FieldName = "CurrentItem.Salary" },
                                    RightExpression = new FieldExpression { FieldName = "CurrentItem.Age" },
                                    Operator = ArithmeticOperatorType.Multiply
                                },
                                RightExpression = new ConstantExpression { Value = new ConstantExpressionValue { Value = 5000 } },
                                Operator = OperatorType.GreaterThan
                            }
                        }
                    }
                }
            }
        };

        // Define an action to apply on each item of the filtered collection based on the name
        var actionOnFilteredItem = new SwitchExpression
        {
            Condition = new FieldExpression { FieldName = "CurrentItem.Name" },
            Cases = new Dictionary<object, Expression>
            {
                { "Sara", new ConstantExpression { Value = new ConstantExpressionValue { Value = "OK" } } },
                { "John", new ConstantExpression { Value = new ConstantExpressionValue { Value = "Nope" } } },
                { "Mike", new ConstantExpression { Value = new ConstantExpressionValue { Value = "No" } } },
                { "Tom", new ConstantExpression { Value = new ConstantExpressionValue { Value = "No" } } },
                { "Anna", new ConstantExpression { Value = new ConstantExpressionValue { Value = "Yes" } } }
            },
            DefaultCase = new ConstantExpression { Value = new ConstantExpressionValue { Value = "Unknown" } }
        };

        // Define the MergedForEachExpression to apply the action on the filtered collection
        var mergedForEachExpression = new MergedForEachExpression
        {
            FilteredCollectionExpression = filteredCollectionExpression,
            ActionExpression = actionOnFilteredItem
        };

        // Evaluate the MergedForEachExpression
        var result = (List<object>)await mergedForEachExpression.EvaluateAsync(new List<Field>());
        Console.WriteLine("MergedForEach result: " + string.Join(", ", result)); // Expected Output: OK, No, No, No, Yes
    }
}