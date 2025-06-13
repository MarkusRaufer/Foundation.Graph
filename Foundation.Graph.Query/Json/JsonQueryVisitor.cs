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
        if (v.TryGetValue(QueryProperty.Out, out object? value))
        {
            var withoutOut = v.Ignore(x => x.Key == QueryProperty.Out)
                              .ToDictionary();

            VisitVwithoutOut(withoutOut);

            if (value is Dictionary<string, object?> @out)
                VisitOut(@out);
            else
                VisitOut(value);
        }
        else
        {
            VisitVwithoutOut(v);
        }
            
    }

    public virtual void VisitVwithoutOut(Dictionary<string, object?> v)
    {
    }

    public virtual void VisitOut(Dictionary<string, object?> @out)
    {
        if (@out.TryGetValue(QueryProperty.Out, out object? value))
        {
            if (value is Dictionary<string, object?> dictionary)
                VisitOut(dictionary);
            else
                VisitOut(value);
        }
    }

    public virtual void VisitOut(object? @out)
    {
    }
}
