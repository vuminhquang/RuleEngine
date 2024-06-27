using RuleEngine.Abstraction;
using RuleEngine.Abstraction.Entities;
using RuleEngine.Domain;

namespace RuleEngineCore;

public partial class Examples
{
    public void RulesExample()
    {
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

        var fields = new List<Field>
        {
            new() { Name = "Age", Value = 30 },
            new() { Name = "Level", Value = 2 },
            new() { Name = "Income", Value = 80000 },
            new() { Name = "Eligible", Value = false }
        };

        // Check the result
        var ruleEngine = new RuleEngine.Application.RuleEngine();
        ruleEngine.ExecuteRules(fields, new List<Rule> { rule });
        Console.WriteLine("Eligible: " + fields.First(f => f.Name == "Eligible").Value); // Output: Eligible: True

        // reset the fields
        fields = new List<Field>
        {
            new() { Name = "Age", Value = 30 },
            new() { Name = "Level", Value = 2 },
            new() { Name = "Income", Value = 80000 },
            new() { Name = "Eligible", Value = false }
        };

        var condition2 = new ComparisonExpression
        {
            LeftExpression = leftExpression,
            RightExpression = rightExpression,
            Operator = OperatorType.GreaterThan
        };
        var rule2 = new Rule
        {
            Name = "Non-Eligibility Rule",
            Condition = condition2,
            Operations = new List<Operation> { setEligibleOperation }
        };
        ruleEngine.ExecuteRules(fields, new List<Rule> { rule2 });
        Console.WriteLine("Eligible: " + fields.First(f => f.Name == "Eligible").Value); // Output: Eligible: False
    }
    
    public void SynchronousExample()
    {
        var ruleEngine = new RuleEngine.Application.RuleEngine();

        var fields = new List<Field>
        {
            new() { Name = "Age", Value = 30 },
            new() { Name = "Level", Value = 2 },
            new() { Name = "Income", Value = 80000 },
            new() { Name = "Eligible", Value = false }
        };

        var rules = GetSampleRules();

        ruleEngine.ExecuteRules(fields, rules);

        Console.WriteLine("Synchronous Eligible: " + fields.First(f => f.Name == "Eligible").Value);
    }

    public async Task AsynchronousExample()
    {
        var ruleEngine = new RuleEngine.Application.RuleEngine();

        var fields = new List<Field>
        {
            new() { Name = "Age", Value = 30 },
            new() { Name = "Level", Value = 2 },
            new() { Name = "Income", Value = 80000 },
            new() { Name = "Eligible", Value = false }
        };

        var rules = GetSampleRules();

        await ruleEngine.ExecuteRulesAsync(fields, rules);

        Console.WriteLine("Asynchronous Eligible: " + fields.First(f => f.Name == "Eligible").Value);
    }

    private IEnumerable<Rule> GetSampleRules()
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

        var setEligibleOperation = new Operation
        {
            FieldName = "Eligible",
            Value = new OperationValue { Value = true, ValueType = typeof(bool) }
        };

        var rule = new Rule
        {
            Name = "Eligibility Rule",
            Condition = condition,
            Operations = new List<Operation> { setEligibleOperation }
        };

        return new List<Rule> { rule };
    }
}