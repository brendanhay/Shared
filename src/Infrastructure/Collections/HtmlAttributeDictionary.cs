using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Collections
{
    public class HtmlAttributeDictionary : Dictionary<string, string>
    {
        public override string ToString()
        {
            var builder = new StringBuilder();

            foreach (var pair in this) {
                builder.Append(pair.Key).Append("=\"").Append(pair.Value).Append('"');
            }

            return builder.ToString();
        }
    }
}
