using System.Xml.Serialization;
using RuleEngine.Abstraction.Entities;
using RuleEngine.Domain.Services;

namespace RuleEngine.Infrastructure.InMemPersistence;

public class InFileRulePersistence : IRulePersistence
{
    private static List<Rule> _rules = new();

    public IEnumerable<Rule> GetRules()
    {
        return _rules;
        // TODO: get back the rules from temp files
    }

    public void SaveRule(Rule rule)
    {
        if (rule.Id == 0)
        {
            rule.Id = _rules.Count + 1;
            _rules.Add(rule);
        }
        else
        {
            var existingRule = _rules.FirstOrDefault(r => r.Id == rule.Id);
            if (existingRule == null) return;
            existingRule.Name = rule.Name;
            existingRule.Condition = rule.Condition;
            existingRule.Operations = rule.Operations;
        }
        
        // dump _rules to temp file
        DumpRulesToTempFile();
    }

    public void DeleteRule(int ruleId)
    {
        var rule = _rules.FirstOrDefault(r => r.Id == ruleId);
        if (rule != null)
        {
            _rules.Remove(rule);
        }
        
        // dump _rules to temp file
        DumpRulesToTempFile();
    }
    
    private static void DumpRulesToTempFile()
    {
        var tempFile = Path.GetTempFileName();
        using var writer = new StreamWriter(tempFile);
        var serializer = new XmlSerializer(typeof(List<Rule>));
        serializer.Serialize(writer, _rules);
    }
}