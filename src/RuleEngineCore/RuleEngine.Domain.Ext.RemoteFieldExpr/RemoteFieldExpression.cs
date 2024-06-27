using RuleEngine.Abstraction.Entities;

namespace RuleEngine.Domain.Ext.RemoteFieldExpr;

public class RemoteFieldExpression : Expression
{
    public string FieldName { get; set; }
    public IFieldFetcher FieldFetcher { get; set; }

    public override object Evaluate(IList<Field> fields)
    {
        throw new NotSupportedException("Synchronous evaluation is not supported for RemoteFieldExpression.");
    }

    public override async Task<object> EvaluateAsync(IList<Field> fields)
    {
        var fieldValue = await FieldFetcher.FetchFieldValueAsync(FieldName);
        return fieldValue;
    }
}