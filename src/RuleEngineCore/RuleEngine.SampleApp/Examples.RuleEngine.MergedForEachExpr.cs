using RuleEngine.Abstraction.Entities;
using RuleEngine.Domain;
using RuleEngine.Domain.Ext.RemoteFieldExpr;

namespace RuleEngineCore;

public partial class Examples
{
    public async Task MergedForEachExpressionExample()
    {
        // Define a collection of fields
        var fields = new List<Field>
        {
            new() { Name = "Value1", Value = 5 },
            new() { Name = "Value2", Value = 15 },
            new() { Name = "Value3", Value = 25 },
            new() { Name = "Value4", Value = 35 }
        };

        // Define a FilteredCollectionExpression to filter the list of field values (e.g., value > 10)
        var filteredCollectionExpression = new FilteredCollectionExpression
        {
            Collection = fields.Select(f => f.Value).ToList(),
            Condition = new ComparisonExpression
            {
                LeftExpression = new FieldExpression { FieldName = "CurrentItem" },
                RightExpression = new ConstantExpression { Value = new ConstantExpressionValue { Value = 10 } },
                Operator = OperatorType.GreaterThan
            }
        };

        // Define an action to apply on each item of the filtered collection (e.g., multiply by 2)
        var actionOnFilteredItem = new ArithmeticExpression
        {
            LeftExpression = new FieldExpression { FieldName = "CurrentItem" },
            RightExpression = new ConstantExpression { Value = new ConstantExpressionValue { Value = 2 } },
            Operator = ArithmeticOperatorType.Multiply
        };

        // Define the MergedForEachExpression to apply the action on the filtered collection
        var mergedForEachExpression = new MergedForEachExpression
        {
            FilteredCollectionExpression = filteredCollectionExpression,
            ActionExpression = actionOnFilteredItem
        };

        // Evaluate the MergedForEachExpression
        var result = (List<object>)await mergedForEachExpression.EvaluateAsync(new List<Field>());
        Console.WriteLine("MergedForEach result: " + string.Join(", ", result)); // Output: MergedForEach result: 30, 50, 70
    }
    
    public async Task MergedForEachExpressionExample2()
    {
        /*
         * FilteredCollectionExpression: Combines the collection and the conditions used to filter the collection.
           Condition 1: Age > 16 and Nation == "America".
           Condition 2: Salary > 2000 and Salary * Age > 5000.
           ActionExpression: Marks the filtered items as "OK".
         */
        
        // Define a collection of persons
        var persons = new List<Person>
        {
            new() { Name = "John", Age = 18, Nation = "America", Salary = 2500 },
            new() { Name = "Anna", Age = 17, Nation = "Canada", Salary = 1500 },
            new() { Name = "Mike", Age = 20, Nation = "America", Salary = 3000 },
            new() { Name = "Sara", Age = 15, Nation = "America", Salary = 1000 },
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

        // Define an action to apply on each item of the filtered collection (e.g., mark as "OK")
        var actionOnFilteredItem = new ConstantExpression
        {
            Value = new ConstantExpressionValue { Value = "OK" }
        };

        // Define the MergedForEachExpression to apply the action on the filtered collection
        var mergedForEachExpression = new MergedForEachExpression
        {
            FilteredCollectionExpression = filteredCollectionExpression,
            ActionExpression = actionOnFilteredItem
        };

        // Evaluate the MergedForEachExpression
        var result = (List<object>)await mergedForEachExpression.EvaluateAsync(new List<Field>());
        Console.WriteLine("MergedForEach result: " + string.Join(", ", result)); // Output: MergedForEach result: OK, OK, OK
    }
    
    public class Person
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public string Nation { get; set; }
        public double Salary { get; set; }
    }
}