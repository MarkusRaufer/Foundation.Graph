namespace Foundation.Graph.Query.Tests;

using Foundation.ComponentModel;
using Edge = Edge<Id>;
using G = Graph<Id, IdNode<Id, Dictionary<string, object?>>, Edge<Id>, NodeSet<Id, IdNode<Id, Dictionary<string, object?>>>, EdgeSet<Id, Edge<Id>>>;
using Node = IdNode<Id, Dictionary<string, object?>>;
using State = Dictionary<string, object?>;

public static class GraphTestUtil
{
    public static G CreateGraph()
    {
        return new G(new NodeSet<Id, Node>(), new EdgeSet<Id, Edge>());
    }

    public static void AddNodesAndEdges(G graph)
    {
        Node? domain;
        {
            var properties = new State
            {
                { nameof(IClassifiable<Any>.StereoType), "domain" },
            };

            domain = IdNode.New(Id.New("Sales"), properties);
            graph.AddNode(domain.Value.Id, domain.Value);
        }

        Node? invoice;
        {
            var id = "I1";
            var properties = new State
            {
                { nameof(ITypedObject<Any>.ObjectType), "Invoice" },
                { "CreatedAt", DateTime.Now },
                { nameof(IClassifiable<Any>.StereoType), "entity" },
            };
            invoice = IdNode.New(Id.New(id), properties);

            graph.AddNode(invoice.Value.Id, invoice.Value);
        }
        Node? invoiceLineItem1;
        {
            var properties = new State
            {
                { "ProductNumber", "P1" },
                { nameof(ITypedObject<Any>.ObjectType), "InvoiceLineItem" },
                { nameof(IClassifiable<Any>.StereoType), "valuetype" },
                { "Quantity", 12.3M },
                { "UnitPrice", 45.67M }
            };
            invoiceLineItem1 = IdNode.New(Id.New(Id.New()), properties);
            graph.AddNode(invoiceLineItem1.Value.Id, invoiceLineItem1.Value);
        }
        Node? invoiceLineItem2;
        {
            var properties = new State
            {
                { "ProductNumber", "P2" },
                { nameof(ITypedObject<Any>.ObjectType), "InvoiceLineItem" },
                { nameof(IClassifiable<Any>.StereoType), "valuetype" },
                { "Quantity", 12.3M },
                { "UnitPrice", 45.67M }
            };
            invoiceLineItem2 = IdNode.New(Id.New(Id.New()), properties);
            graph.AddNode(invoiceLineItem2.Value.Id, invoiceLineItem2.Value);
        }

        {
            var edge = new Edge<Id>(domain.Value.Id, invoice.Value.Id);
            graph.AddEdge(edge);
        }
        {
            var edge = new Edge<Id>(invoice.Value.Id, invoiceLineItem1.Value.Id);
            graph.AddEdge(edge);
        }
        {
            var edge = new Edge<Id>(invoice.Value.Id, invoiceLineItem2.Value.Id);
            graph.AddEdge(edge);
        }
    }
}
