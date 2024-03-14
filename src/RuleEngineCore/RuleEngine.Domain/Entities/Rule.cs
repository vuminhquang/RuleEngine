namespace RuleEngine.Abstraction.Entities;

public class Rule
{
    public int Id { get; set; }
    public string Name { get; set; }
    public Expression Condition { get; set; }
    public ICollection<Operation> Operations { get; set; }
}