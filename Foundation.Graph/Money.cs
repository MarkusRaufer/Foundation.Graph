namespace Foundation.Graph;

public record Money(string Currency, decimal Amount) : Money<string>(Currency, Amount);

public record Money<TCurrency>(TCurrency Currency, decimal Amount);
