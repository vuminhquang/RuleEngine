# Rules Engine

A flexible and extensible rules engine for evaluating and executing business rules using expressions. This engine allows defining complex rules and actions using a variety of expressions like logical, arithmetic, comparison, and custom expressions such as `ForEachExpression`, `FilteredCollectionExpression`, and `SwitchExpression`.

## Features

- **Expression-Based Rules**: Define rules using a variety of expressions (logical, arithmetic, comparison, etc.).
- **Custom Expressions**: Support for custom expressions like `ForEachExpression`, `FilteredCollectionExpression`, and `SwitchExpression`.
- **Async Support**: Evaluate rules asynchronously.
- **Entity Framework Integration**: Store rules and expressions in a relational database using Entity Framework Core.
- **Flexible Configuration**: Easily configure and extend the engine to support new types of expressions.

## Installation

1. **Clone the Repository**:
    ```sh
    git clone https://github.com/your-repo/rules-engine.git
    cd rules-engine
    ```

2. **Install Dependencies**:
    ```sh
    dotnet restore
    ```

3. **Build the Project**:
    ```sh
    dotnet build
    ```

4. **Update Database**:
    Make sure to have a database configured and update it with migrations.
    ```sh
    dotnet ef database update
    ```

## Usage

### Define Expressions

Expressions are the building blocks of rules. Here are some examples of different types of expressions:

- **ConstantExpression**: Represents a constant value.
- **FieldExpression**: Represents a field from the input data.
- **ArithmeticExpression**: Represents an arithmetic operation between two expressions.
- **ComparisonExpression**: Represents a comparison operation between two expressions.
- **LogicalExpression**: Represents a logical operation (AND, OR) between multiple expressions.
- **ForEachExpression**: Iterates over a collection and applies a condition and an action.
- **FilteredCollectionExpression**: Filters a collection based on a condition.
- **SwitchExpression**: Evaluates different cases based on a condition.

### Example: Defining and Evaluating a Rule

```csharp
public async Task EvaluateRuleExample()
{
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

    // Define a complex action to apply on each item of the filtered collection based on the name and salary
    var actionOnFilteredItem = new LogicalExpression
    {
        Operator = LogicalOperatorType.Or,
        OperandExpressions = new List<Expression>
        {
            new ComparisonExpression
            {
                LeftExpression = new FieldExpression { FieldName = "CurrentItem.Salary" },
                RightExpression = new ConstantExpression { Value = new ConstantExpressionValue { Value = 3000 } },
                Operator = OperatorType.GreaterThan,
                // This will result in "No" if the salary is greater than 3000
            },
            new SwitchExpression
            {
                Condition = new FieldExpression { FieldName = "CurrentItem.Name" },
                Cases = new Dictionary<object, Expression>
                {
                    { "Sara", new ConstantExpression { Value = new ConstantExpressionValue { Value = "OK" } } },
                    { "John", new ConstantExpression { Value = new ConstantExpressionValue { Value = "No" } } },
                    { "Mike", new ConstantExpression { Value = new ConstantExpressionValue { Value = "No" } } },
                    { "Tom", new ConstantExpression { Value = new ConstantExpressionValue { Value = "No" } } },
                    { "Anna", new ConstantExpression { Value = new ConstantExpressionValue { Value = "Yes" } } }
                },
                DefaultCase = new ConstantExpression { Value = new ConstantExpressionValue { Value = "Unknown" } }
            }
        }
    };

    // Define the MergedForEachExpression to apply the action on the filtered collection
    var mergedForEachExpression = new MergedForEachExpression
    {
        FilteredCollectionExpression = filteredCollectionExpression,
        ActionExpression = actionOnFilteredItem
    };

    // Evaluate the MergedForEachExpression
    var result = (List<object>)await mergedForEachExpression.EvaluateAsync(new List<Field>());
    Console.WriteLine("MergedForEach result: " + string.Join(", ", result)); // Expected Output: No, Yes, No, OK, No
}
```

## Contributing

Contributions are welcome! Please open an issue or submit a pull request if you have any improvements or bug fixes.

## License

This project is licensed under the MIT License.

## Acknowledgements

- [Entity Framework Core](https://docs.microsoft.com/en-us/ef/core/)
- [Newtonsoft.Json](https://www.newtonsoft.com/json)
