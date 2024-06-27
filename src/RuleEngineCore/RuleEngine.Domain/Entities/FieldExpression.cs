using System.Reflection;
using RuleEngine.Domain;

namespace RuleEngine.Abstraction.Entities;

public class FieldExpression : Expression
{
    public string FieldName { get; set; }

    public override object Evaluate(IList<Field> fields)
    {
        var field = fields.FirstOrDefault(f => f.Name == FieldName) ?? GetNestedField(fields, FieldName);
        return field?.Value;
    }

    public override Task<object> EvaluateAsync(IList<Field> fields)
    {
        return Task.FromResult(Evaluate(fields));
    }

    private Field GetNestedField(IList<Field> fields, string fieldName)
    {
        var parts = fieldName.Split('.');
        if (parts.Length < 2) return null;

        var parentField = fields.FirstOrDefault(f => f.Name == parts[0]);
        if (parentField?.Value == null) return null;

        var parentObject = parentField.Value;
        var property = parentObject.GetType().GetProperty(parts[1], BindingFlags.Public | BindingFlags.Instance);
        if (property == null) return null;

        var value = property.GetValue(parentObject);
        return new Field { Name = fieldName, Value = value };
    }
}