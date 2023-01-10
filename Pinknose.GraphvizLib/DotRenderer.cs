using Pinknose.GraphvizLib.Attributes;
using System.Text;

namespace Pinknose.GraphvizLib
{
    public abstract class DotRenderer
    {
        #region Methods

        public override bool Equals(object? obj)
        {
            if (obj == null) return false;

            return GetHashCode() == obj.GetHashCode();
        }

        public override int GetHashCode()
        {
            int hash = 0;

            foreach (var propInfo in this.GetType().GetProperties())
            {
                var prop = propInfo.GetValue(this);

                if (prop != null && propInfo.Name != nameof(Guid))
                {
                    var tempHash = prop.GetHashCode();

                    hash += tempHash;
                }
            }

            return hash;
        }

        internal abstract string RenderDot(Graph graph, int indent);

        internal string RenderMultiLineAttributes(int indent)
        {
            string indentText = new string(' ', indent);

            var sb = new StringBuilder();

            var attributes = GetAttributes();

            foreach (var attr in attributes)
            {
                sb.AppendLine($"{indentText}{attr.Key}={attr.Value};");
            }

            return sb.ToString();
        }

        internal string RenderSingleLineAttributes()
        {
            var sb = new StringBuilder();
            sb.Append("[");

            var attributes = GetAttributes();

            foreach (var attr in attributes)
            {
                sb.Append($"{attr.Key}={attr.Value},");
            }

            sb.Append("]");

            return sb.ToString();
        }

        private IDictionary<string, string> GetAttributes()
        {
            Dictionary<string, string> attributes = new Dictionary<string, string>();

            var propertyInfos = this.GetType().GetProperties().Where(p => p.GetCustomAttributes(typeof(AttributeNameAttribute), true).Any());

            foreach (var propertyInfo in propertyInfos)
            {
                var attributeName = ((AttributeNameAttribute)propertyInfo.GetCustomAttributes(typeof(AttributeNameAttribute), true).Single()).Name;
                string? attrbuteValue = null;

                var value = propertyInfo.GetValue(this);

                if (value != null)
                {
                    // Underlying type if nullable, or just the type if not.
                    var underlyingType = Nullable.GetUnderlyingType(propertyInfo.PropertyType) ?? propertyInfo.PropertyType;

                    if (underlyingType != null && underlyingType.IsEnum)
                    {
                        if (underlyingType == typeof(Color))
                        {
                            attrbuteValue = (value.ToString() ?? throw new ArgumentNullException()).ToLower().SurroundInQuotes();
                        }
                        else
                        {
                            attrbuteValue = ((Enum)value).GetDisplayValue().SurroundInQuotes();
                        }
                    }
                    else if (underlyingType == typeof(bool))
                    {
                        attrbuteValue = (value.ToString() ?? throw new ArgumentNullException()).ToLower().SurroundInQuotes();
                    }
                    else if (underlyingType == typeof(Label) || value.GetType() == typeof(Label))
                    {
                        var tempVal = (Label)value;
                        attrbuteValue = (tempVal.ToString() ?? throw new ArgumentNullException()).SurroundInQuotes(!tempVal.IsHtml);
                    }
                    else
                    {
                        attrbuteValue = ((value.ToString() ?? throw new ArgumentNullException()).SurroundInQuotes() ?? throw new ArgumentNullException());
                    }

                    if (attrbuteValue is not null)
                    {
                        attributes.Add(attributeName, attrbuteValue);
                    }
                }
            }

            return attributes;
        }

        #endregion Methods
    }
}