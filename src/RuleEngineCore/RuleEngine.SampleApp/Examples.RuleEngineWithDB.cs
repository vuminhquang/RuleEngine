using Microsoft.EntityFrameworkCore;
using RuleEngine.Abstraction;
using RuleEngine.Abstraction.Entities;
using RuleEngine.Domain;
using RuleEngine.Domain.Services;
using RuleEngine.Infrastructure.EF;

namespace RuleEngineCore;

public partial class Examples
{
    public void PersistenceExample()
{
    // Configure the in-memory database options
    var options = new DbContextOptionsBuilder<RuleDbContext>()
        .UseInMemoryDatabase(databaseName: "RuleDatabase")
        .Options;

    // Create a new instance of the in-memory database context
    var dbContext = new RuleDbContext(options);

    IRulePersistence rulePersistence = new EfRulePersistence(dbContext);

    CreateRules(rulePersistence);

    var rules = rulePersistence.GetRules();
    
    var ruleEngine = new RuleEngine.Application.RuleEngine();
    
    var fields = new List<Field>
    {
        new() { Name = "Age", Value = 30 },
        new() { Name = "Level", Value = 2 },
        new() { Name = "Income", Value = 80000 },
        new() { Name = "Eligible", Value = false }
    };
    
    
    // Execute the rules
    ruleEngine.ExecuteRules(fields, rules);

    // Print the updated fields
    foreach (var field in fields)
    {
        Console.WriteLine($"{field.Name}: {field.Value}");
    }
}

    private static void CreateRules(IRulePersistence rulePersistence)
    {
        // Create rules in DB
        // Create expressions
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
        
        var condition2 = new ComparisonExpression
        {
            LeftExpression = leftExpression,
            RightExpression = new ConstantExpression { Value = new ConstantExpressionValue { Value = (double) 100} },
            Operator = OperatorType.GreaterThan
        };

        // Create operations
        var setEligibleOperation = new Operation
            { FieldName = "Eligible", Value = new OperationValue { Value = true, ValueType = typeof(bool) } };

        // Create a rule
        var rule = new Rule
        {
            Name = "Eligibility Rule",
            Condition = condition,
            Operations = new List<Operation> { setEligibleOperation }
        };
        
        // Save the rule
        rulePersistence.SaveRule(rule);
        
        var rule2 = new Rule
        {
            Name = "Eligibility Rule 2",
            Condition = condition2,
            Operations = new List<Operation> { setEligibleOperation }
        };
        
        // Save the rule
        rulePersistence.SaveRule(rule2);
    }
}