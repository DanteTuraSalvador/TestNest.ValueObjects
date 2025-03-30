# 🏷️ ValueObject Library

A .NET implementation of the Value Object pattern that enforces domain constraints and eliminates primitive obsession for domain values.

## ✨ Features

- 🧱 **Immutable** - All properties are read-only after creation  
- ⚖️ **Value-based equality** - Compares by values, not reference  
- 🛡️ **Self-validating** - Encapsulates validation rules  
- 💰 **Currency support** - Built-in monetary value handling  
- 🧵 **Thread-safe** - Lazy initialization for empty instances  
- 🔄 **Fluid API** - Chainable update methods  
- 📦 **Self-contained** - No external dependencies  

---

## 📌 Core Implementation

### 🔹 ValueObject Base Class

```csharp
public abstract class ValueObject : IEquatable<ValueObject>
{
    public static bool operator ==(ValueObject? a, ValueObject? b) 
        => a is null ? b is null : a.Equals(b);

    public static bool operator !=(ValueObject? a, ValueObject? b) 
        => !(a == b);

    public virtual bool Equals(ValueObject? other) =>
        ReferenceEquals(this, other) || 
        (other is not null && 
         GetType() == other.GetType() && 
         ValuesAreEqual(other));

    public override bool Equals(object? obj) 
        => obj is ValueObject valueObject && Equals(valueObject);

    public override int GetHashCode() =>
        GetAtomicValues().Aggregate(17, (hash, value) => 
            HashCode.Combine(hash, value?.GetHashCode() ?? 0));

    protected abstract IEnumerable<object?> GetAtomicValues();

    private bool ValuesAreEqual(ValueObject other) 
        => GetAtomicValues().SequenceEqual(other.GetAtomicValues());
}
