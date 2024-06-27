using RuleEngineCore;

var examples = new Examples();
examples.ExpressionExample();
examples.RulesExample();
examples.PersistenceExample();
await examples.ForEachExpressionExample();
await examples.MergedForEachExpressionExample2();
await examples.AlternateResultExample();