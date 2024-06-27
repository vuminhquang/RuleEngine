using RuleEngine.Abstraction.Entities;
using RuleEngine.Domain;

namespace RuleEngine.Abstraction.Services;

public interface IRuleEngine
{
    void ExecuteRules(IEnumerable<Field> fields, IEnumerable<Rule> rules);
    Task ExecuteRulesAsync(IEnumerable<Field> fields, IEnumerable<Rule> rules);
}