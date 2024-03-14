using RuleEngine.Abstraction;
using RuleEngine.Abstraction.Entities;
using RuleEngine.Abstraction.Services;
using RuleEngine.Domain;

namespace RuleEngine.Application;

public class RuleEngine : IRuleEngine
{
    public void ExecuteRules(IEnumerable<Field> fields, IEnumerable<Rule> rules)
    {
        var listFields = fields.ToList();

        foreach (var rule in rules)
        {
            var conditionResult = EvaluateCondition(rule.Condition, listFields);
            if (conditionResult)
            {
                ApplyOperations(rule.Operations, listFields);
            }
        }
    }

    private static bool EvaluateCondition(Expression condition, IEnumerable<Field> fields)
    {
        var result = condition.Evaluate(fields.ToList());
        return result is bool boolResult ? boolResult : Convert.ToBoolean(result);
    }

    private static void ApplyOperations(IEnumerable<Operation> operations, IEnumerable<Field> fields)
    {
        var fieldsDictionary = fields.ToDictionary(f => f.Name, f => f);
        foreach (var operation in operations)
        {
            if (fieldsDictionary.TryGetValue(operation.FieldName, out var field))
            {
                field.Value = operation.Value.Value;
            }
        }
    }
}