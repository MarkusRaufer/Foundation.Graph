using Foundation.Collections.Generic;

namespace Foundation.Graph.Query.Json;

public class JsonQueryVisitor
{
    protected virtual void Visit(JsonQuery query)
    {
        VisitMethod(query.Method);
        VisitV(query.V);
    }

    public virtual void VisitMethod(string method)
    {
    }

    public virtual void VisitV(Dictionary<string, object?> v)
    {
        if (v.TryGetValue(QueryProperty.OutV, out object? value))
        {
            var withoutOutV = v.Ignore(x => x.Key == QueryProperty.OutV)
                              .ToDictionary();

            VisitVwithoutOutV(withoutOutV);

            if (value is Dictionary<string, object?> @out)
                VisitOutV(@out);
            else
                VisitOutV(value);
        }
        else
        {
            VisitVwithoutOutV(v);
        }
            
    }

    public virtual void VisitVwithoutOutV(Dictionary<string, object?> v)
    {
    }

    public virtual void VisitOutV(Dictionary<string, object?> @out)
    {
        if (@out.TryGetValue(QueryProperty.OutV, out object? value))
        {
            if (value is Dictionary<string, object?> dictionary)
                VisitOutV(dictionary);
            else
                VisitOutV(value);
        }
    }

    public virtual void VisitOutV(object? @out)
    {
    }
}
