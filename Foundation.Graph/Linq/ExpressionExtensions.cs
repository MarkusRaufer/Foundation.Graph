using Foundation.Linq.Expressions;
using System.Linq.Expressions;

namespace Foundation.Graph.Linq;

public static class ExpressionExtensions
{
    public static IEnumerable<TExpression> OfType<TExpression>(this IEnumerable<Expression> expressions)
        where TExpression : Expression => OfType<Expression, TExpression>(expressions);

    public static IEnumerable<TTargetExpression> OfType<TExpression, TTargetExpression>(this IEnumerable<TExpression> expressions)
        where TExpression : Expression
        where TTargetExpression : Expression
    {
        var isParameterType = typeof(TTargetExpression) == typeof(ParameterExpression);
        foreach (var expression in expressions)
        {
            if (isParameterType && expression.NodeType == ExpressionType.Parameter)
            {
                if (expression is IdExpression<TTargetExpression> idExpression)
                {
                    yield return idExpression.Expression;
                    continue;
                }
            }

            if (expression is TTargetExpression t) yield return t;
        }
    }
}
