using System.Linq.Expressions;
using System.Reflection;

namespace Infrastructure.Extensions
{
    public static class ExpressionExtensions
    {
        // Hrm. this could potentially infinitely loop if there's no member access in the expression
        public static bool TrySetMemberAccessed(this LambdaExpression self, object target, object value)
        {
            var part = self.Body;

            while (part != null) {
                switch (part.NodeType) {
                    case ExpressionType.Call:
                        part = ((MethodCallExpression)part).Object;
                        break;
                    case ExpressionType.ArrayIndex:
                        part = ((BinaryExpression)part).Left;
                        break;
                    case ExpressionType.MemberAccess:
                        return TrySetMember(((MemberExpression)part).Member, target, value);
                    default:
                        break;
                }
            }

            return false;
        }

        private static bool TrySetMember(MemberInfo member, object target, object value)
        {
            var success = false;

            if (member is PropertyInfo) {
                ((PropertyInfo)member).SetValue(target, value, null);
                success = true;
            } else if (member is FieldInfo) {
                ((FieldInfo)member).SetValue(target, value);
                success = true;
            }

            return success;
        }
    }
}