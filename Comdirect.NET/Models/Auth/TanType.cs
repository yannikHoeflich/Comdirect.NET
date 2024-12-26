namespace Comdirect.NET.Models.Auth;

public readonly struct TanType : IEquatable<TanType> {
    public bool Equals(TanType other) {
        return Name == other.Name;
    }

    public override bool Equals(object? obj) {
        return obj is TanType other && Equals(other);
    }

    public override int GetHashCode() {
        return Name.GetHashCode();
    }

    public static bool operator ==(TanType left, TanType right) {
        return left.Equals(right);
    }

    public static bool operator !=(TanType left, TanType right) {
        return !left.Equals(right);
    }

    internal string Name { get; }

    internal TanType(string name) {
        Name = name;
    }

    public static TanType Push { get; } = new("P_TAN_PUSH");
    public static TanType Phone { get; } = new("MTAN");
    public static TanType Graphic { get; } = new("P_TAN");

    public static bool TryParse(string typeString, out TanType tanType) {
        switch (typeString) {
            case "P_TAN_PUSH":
                tanType = Push;
                return true;
            case "MTAN":
                tanType = Phone;
                return true;
            case "P_TAN":
                tanType = Graphic;
                return true;
            default:
                tanType = default;
                return false;
        }
    }
}