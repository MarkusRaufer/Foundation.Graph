using Foundation.Graph.Query.Json;
using Foundation.Text.Json.Serialization;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Foundation.Graph.Query.Tests.Json;

public class JsonQueryTests
{
    [Fact]
    public void Deserialize_Should_ReturnAJsonQueryObject_When_JsonStringWasDeserialized()
    {
        // Arrange
        var serializeOptions = new JsonSerializerOptions
        {
            Converters =
            {
                new ObjectJsonConverter()
            }
        };

        var json =
            """
            {
            	"Method": "Find",
            	"V": {
            		"Id": "I1",
            		"Out": true	
            	}
            }
            """;

        // Act
        var jsonQuery = JsonSerializer.Deserialize<JsonQuery>(json, serializeOptions);

        // Assert
        jsonQuery.ShouldNotBeNull();
        jsonQuery.Method.ShouldBe(QueryMethod.Find);
        jsonQuery.V.ShouldNotBeNull();
        jsonQuery.V["Id"].ShouldBe("I1");
        jsonQuery.V["Out"].ShouldBe(true);
    }
}
