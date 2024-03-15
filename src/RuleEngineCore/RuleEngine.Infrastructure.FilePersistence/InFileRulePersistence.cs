using System.Xml.Serialization;
using RuleEngine.Abstraction.Entities;
using RuleEngine.Domain.Services;

namespace RuleEngine.Infrastructure.InMemPersistence;

public class InFileRulePersistence : IRulePersistence
{
    private List<Rule> _rules = [];

    public IEnumerable<Rule> GetRules()
    {
        var tempFile = Path.GetTempFileName();
        using var reader = new StreamReader(tempFile);
        var serializer = new XmlSerializer(typeof(List<Rule>));
        // deserialize the rules from temp file
        var deserializedObj = serializer.Deserialize(reader);
        // if deserialized rules are not null, return them, otherwise return an empty list
        return deserializedObj != null ? (List<Rule>)deserializedObj : [];
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
    
    private void DumpRulesToTempFile()
    {
        var tempFile = Path.GetTempFileName();
        using var writer = new StreamWriter(tempFile);
        var serializer = new XmlSerializer(typeof(List<Rule>));
        serializer.Serialize(writer, _rules);
    }
}