namespace RuleEngine.Abstraction.Entities;

public class Operation
{
    public int Id { get; set; }
    public string FieldName { get; set; }
    public OperationValue Value { get; set; }
    public int RuleId { get; set; }
    public Rule Rule { get; set; }
}

public class OperationValue
{
    public int Id { get; set; }
    public object Value { get; set; }
    public Type ValueType { get; set; }
}