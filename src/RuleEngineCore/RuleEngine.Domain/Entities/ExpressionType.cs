namespace RuleEngine.Abstraction.Entities;

public enum ExpressionType
{
    Constant,
    Field,
    Arithmetic,
    Comparison,
    Logical,
    ForEach,
    FilteredCollection,
    Switch
}