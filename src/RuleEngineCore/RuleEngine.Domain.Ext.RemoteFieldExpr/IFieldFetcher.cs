namespace RuleEngine.Domain.Ext.RemoteFieldExpr;

public interface IFieldFetcher
{
    Task<object> FetchFieldValueAsync(string fieldName);
}