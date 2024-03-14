using RuleEngine.Abstraction.Entities;

namespace RuleEngine.Domain.Services;

public interface IRulePersistence
{
    IEnumerable<Rule> GetRules();
    void SaveRule(Rule rule);
    void DeleteRule(int ruleId);
}