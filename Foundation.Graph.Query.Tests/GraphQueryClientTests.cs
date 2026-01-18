using Foundation;
using Foundation.Collections.Generic;
using Foundation.ComponentModel;
using Shouldly;

namespace Foundation.Graph.Query.Tests;

using Edge = Edge<Id>;
using G = Graph<Id, IdNode<Id, Dictionary<string, object?>>, Edge<Id>, NodeSet<Id, IdNode<Id, Dictionary<string, object?>>>, EdgeSet<Id, Edge<Id>>>;
using Node = IdNode<Id, Dictionary<string, object?>>;

public class GraphQueryClientTests
{
    [Fact]
    public void OutV_Find_Should_ReturnInvoiceLineItems_When_QueryFoundInvoiceLineItems()
    {
        // Arrange
        var graph = GraphTestUtil.CreateGraph();
        GraphTestUtil.AddNodesAndEdges(graph);

        var client = GraphQueryClient.New<Id, Node, Edge, G>(graph);

        // Act
        var lineItems = client.NewQuery()
                              .V(x => x.Id == Id.New("I1"))
                              .OutV((Node x) => true)
                              .Find()
                              .Execute()
                              .ToArray();
        // Assert
        lineItems.Length.ShouldBe(2);
        var li1 = lineItems[0];
        var exists = li1.Value.State.TryGetValue(nameof(ITypedObject<Any>.ObjectType), out var objectType);
        exists.ShouldBeTrue();
        
        objectType.ShouldNotBeNull();
        var objectTypeValue = "InvoiceLineItem";
        objectType.ShouldBe(objectTypeValue);

        var li2 = lineItems[1];
        exists = li2.Value.State.TryGetValue(nameof(ITypedObject<Any>.ObjectType), out objectType);
        exists.ShouldBeTrue();

        objectType.ShouldNotBeNull();
        objectType.ShouldBe(objectTypeValue);

        li1.Key.ShouldNotBe(li2.Key);
        li1.Value.Id.ShouldNotBe(li2.Value.Id);
    }

    [Fact]
    public void OutV_FindPath_Should_ReturnPathIncludingInvoiceAndInvoiceLineItems_When_QueryFoundInvoiceLineItems()
    {
        // Arrange
        var graph = GraphTestUtil.CreateGraph();
        GraphTestUtil.AddNodesAndEdges(graph);

        var client = GraphQueryClient.New<Id, Node, Edge, G>(graph);

        // Act
        var nodes = client.NewQuery()
                          .V(x => x.Id == Id.New("I1"))
                          .OutV((Node x) => true)
                          .FindPath()
                          .Execute()
                          .ToArray();

        // Assert
        nodes.Length.ShouldBe(3);

        var (match, noMatch) = nodes.Partition(x => x.Key == Id.New("I1"));
        var node1 = match.First();
        var exists = node1.Value.State.TryGetValue(nameof(ITypedObject<Any>.ObjectType), out var objectType);
        exists.ShouldBeTrue();

        objectType.ShouldNotBeNull();
        var objectTypeValue = "Invoice";
        objectType.ShouldBe(objectTypeValue);

        var ili1 = noMatch.Nth(0).OrThrow();
        exists = ili1.Value.State.TryGetValue(nameof(ITypedObject<Any>.ObjectType), out objectType);
        exists.ShouldBeTrue();
        objectType.ShouldNotBeNull();
        objectType.ShouldBe("InvoiceLineItem");

        var ili2 = noMatch.Nth(1).OrThrow();
        exists = ili2.Value.State.TryGetValue(nameof(ITypedObject<Any>.ObjectType), out objectType);
        exists.ShouldBeTrue();
        objectType.ShouldNotBeNull();
        objectType.ShouldBe("InvoiceLineItem");
    }

    [Fact]
    public void Repeat_OutV_Should_ReturnPathWithAllNodes_When_QueryWhereStateKeyIsObjectType()
    {
        // Arrange
        var graph = GraphTestUtil.CreateGraph();
        GraphTestUtil.AddNodesAndEdges(graph);

        var client = GraphQueryClient.New<Id, Node, Edge, G>(graph);

        var invoiceLineItemObjectType = "InvoiceLineItem";
        // Act
        var nodes = client.NewQuery()
                          .V(x => x.Id == Id.New("Sales"))
                          .Repeat(x => x.OutV((Node x) => true), x => $"{x.State[nameof(ITypedObject<Any>.ObjectType)]}" == invoiceLineItemObjectType)
                          .FindPath()
                          .Execute()
                          .ToArray();

        // Assert
        nodes.Length.ShouldBe(4);

        var domainId = Id.New("Sales");
        var (match, noMatch) = nodes.Partition(x => x.Key == domainId);
        var node1 = match.First();
        var exists = node1.Value.State.TryGetValue(nameof(IClassifiable<Any>.StereoType), out var stereoType);
        exists.ShouldBeTrue();
        stereoType.ShouldNotBeNull();
        stereoType.ShouldBe("domain");
        node1.Key.ShouldBe(domainId);
        node1.Value.Id.ShouldBe(domainId);

        (match, noMatch) = noMatch.Partition(x => x.Key == Id.New("I1"));
        var node2 = match.First();
        exists = node2.Value.State.TryGetValue(nameof(ITypedObject<Any>.ObjectType), out var objectType);
        exists.ShouldBeTrue();

        objectType.ShouldNotBeNull();
        var objectTypeValue = "Invoice";
        objectType.ShouldBe(objectTypeValue);

        var ili1 = noMatch.Nth(0).OrThrow();
        exists = ili1.Value.State.TryGetValue(nameof(ITypedObject<Any>.ObjectType), out objectType);
        exists.ShouldBeTrue();
        objectType.ShouldNotBeNull();
        objectType.ShouldBe(invoiceLineItemObjectType);

        var ili2 = noMatch.Nth(1).OrThrow();
        exists = ili2.Value.State.TryGetValue(nameof(ITypedObject<Any>.ObjectType), out objectType);
        exists.ShouldBeTrue();
        objectType.ShouldNotBeNull();
        objectType.ShouldBe(invoiceLineItemObjectType);
    }

    [Fact]
    public void Repeat_OutV_Should_ReturnPathWithAllNodes_When_QueryStateTryGetValueOfObjectTypeKey()
    {
        // Arrange
        var objectTypeKey = nameof(ITypedObject<Any>.ObjectType);
        var graph = GraphTestUtil.CreateGraph();        
        GraphTestUtil.AddNodesAndEdges(graph);
        var invoiceLineItemObjectType = "InvoiceLineItem";

        var invoiceLineItem = graph.Nodes.First(x => x.State.TryGetValue(objectTypeKey, out var objType) && $"{objType}" == invoiceLineItemObjectType);

        var client = GraphQueryClient.New<Id, Node, Edge, G>(graph);

        // Act
        var nodes = client.NewQuery()
                          .V(x => x.Id == Id.New("Sales"))
                          .Repeat(x => x.OutV((Node x) => true), x => x.Id == invoiceLineItem.Id)
                          .FindPath()
                          .Execute()
                          .ToArray();

        // Assert
        nodes.Length.ShouldBe(3);

        var domainId = Id.New("Sales");
        var (match, noMatch) = nodes.Partition(x => x.Key == domainId);
        var node1 = match.First();
        var exists = node1.Value.State.TryGetValue(nameof(IClassifiable<Any>.StereoType), out var stereoType);
        exists.ShouldBeTrue();
        stereoType.ShouldNotBeNull();
        stereoType.ShouldBe("domain");
        node1.Key.ShouldBe(domainId);
        node1.Value.Id.ShouldBe(domainId);

        (match, noMatch) = noMatch.Partition(x => x.Key == Id.New("I1"));
        var node2 = match.First();
        exists = node2.Value.State.TryGetValue(nameof(ITypedObject<Any>.ObjectType), out var objectType);
        exists.ShouldBeTrue();

        objectType.ShouldNotBeNull();
        var objectTypeValue = "Invoice";
        objectType.ShouldBe(objectTypeValue);

        var ili1 = noMatch.First();
        exists = ili1.Value.State.TryGetValue(nameof(ITypedObject<Any>.ObjectType), out objectType);
        exists.ShouldBeTrue();
        objectType.ShouldNotBeNull();
        objectType.ShouldBe("InvoiceLineItem");
        ili1.Key.ShouldBe(invoiceLineItem.Id);
        ili1.Value.Id.ShouldBe(invoiceLineItem.Id);
    }

    [Fact]
    public void Repeat_InV_Should_ReturnPathWithAllNodes_When_QueryStateTryGetValueOfObjectTypeKey()
    {
        // Arrange
        var objectTypeKey = nameof(ITypedObject<Any>.ObjectType);
        var graph = GraphTestUtil.CreateGraph();
        GraphTestUtil.AddNodesAndEdges(graph);
        var invoiceObjectType = "Invoice";
        var invoiceLineItemObjectType = "InvoiceLineItem";
        var invoiceLineItem = graph.Nodes.First(x => x.State.TryGetValue(objectTypeKey, out var objType) && $"{objType}" == invoiceLineItemObjectType);

        var client = GraphQueryClient.New<Id, Node, Edge, G>(graph);

        // Act
        var nodes = client.NewQuery()
                          .V(x => x.Id == invoiceLineItem.Id)
                          .Repeat(x => x.InV((Node x) => true), x => x.State.TryGetValue(objectTypeKey, out var objType) && objType.EqualsNullable(invoiceObjectType))
                          .FindPath()
                          .Execute()
                          .ToArray();

        // Assert
        nodes.Length.ShouldBe(2);

        var node1 = nodes[0];
        var exists = node1.Value.State.TryGetValue(nameof(ITypedObject<Any>.ObjectType), out var objectType);
        exists.ShouldBeTrue();

        objectType.ShouldNotBeNull();
        objectType.ShouldBe("InvoiceLineItem");

        node1.Key.ShouldBe(invoiceLineItem.Id);

        var node2 = nodes[1];
        exists = node2.Value.State.TryGetValue(nameof(ITypedObject<Any>.ObjectType), out objectType);
        exists.ShouldBeTrue();

        objectType.ShouldNotBeNull();
        objectType.ShouldBe("Invoice");
    }
}