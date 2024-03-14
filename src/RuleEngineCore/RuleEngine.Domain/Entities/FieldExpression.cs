using RuleEngine.Domain;

namespace RuleEngine.Abstraction.Entities;

public class FieldExpression : Expression
{
    public string FieldName { get; set; }
    
    public override object Evaluate(IList<Field> fields)
    {
        var field = fields.FirstOrDefault(f => f.Name == FieldName);
        return field?.Value;
    }
}