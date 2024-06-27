using RuleEngine.Domain;

namespace RuleEngine.Abstraction.Entities;

public class ConstantExpression : Expression
    {
        public ConstantExpressionValue Value { get; set; }

        public override object Evaluate(IList<Field> fields)
        {
            return Value.Value;
        }
    }

    public class ConstantExpressionValue
    {
        private object _value;
        private Type _valueType;
        public int Id { get; set; }

        public Type ValueType => _valueType;

        // on set the Value, get the type of the value and set it to ValueType
        public object Value
        {
            get => _value;
            set
            {
                _value = value;
                _valueType = value.GetType();
            }
        }
    }