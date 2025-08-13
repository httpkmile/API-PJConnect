using System.Text;

namespace WebApiPJConnect.Domain.Shared
{
    public sealed class Address : IEquatable<Address>
    {
        public string Street { get; private set; } = default!;
        public string Number { get; private set; } = default!;
        public string City { get; private set; } = default!;
        public string State { get; private set; } = default!; // UF (2 letras)
        public string ZipCode { get; private set; } = default!; // CEP (apenas dígitos)

        private Address() { } // EF Core

        public Address(string street, string number, string city, string state, string zipCode)
        {
            Street = NormalizeNonEmpty(street, nameof(street), 120);
            Number = NormalizeNonEmpty(number, nameof(number), 20);
            City = NormalizeNonEmpty(city, nameof(city), 60);
            State = NormalizeUF(state);
            ZipCode = NormalizeZip(zipCode);
        }

        public string ZipFormatted => $"{ZipCode[..5]}-{ZipCode[^3..]}";

        public override string ToString()
            => new StringBuilder()
                .Append(Street).Append(", ").Append(Number)
                .Append(" - ").Append(City).Append("/").Append(State)
                .Append(" - CEP ").Append(ZipFormatted)
                .ToString();

        // Equality (Value Object)
        public bool Equals(Address? other)
            => other is not null
               && string.Equals(Street, other.Street, StringComparison.OrdinalIgnoreCase)
               && string.Equals(Number, other.Number, StringComparison.OrdinalIgnoreCase)
               && string.Equals(City, other.City, StringComparison.OrdinalIgnoreCase)
               && string.Equals(State, other.State, StringComparison.Ordinal)
               && string.Equals(ZipCode, other.ZipCode, StringComparison.Ordinal);

        public override bool Equals(object? obj) => obj is Address a && Equals(a);

        public override int GetHashCode()
            => HashCode.Combine(
                Street.ToUpperInvariant(),
                Number.ToUpperInvariant(),
                City.ToUpperInvariant(),
                State,
                ZipCode
            );

        // Helpers
        private static string NormalizeNonEmpty(string? s, string param, int maxLen)
        {
            var v = (s ?? string.Empty).Trim();
            if (v.Length == 0) throw new ArgumentException($"{param} é obrigatório.", param);
            if (v.Length > maxLen) v = v.Substring(0, maxLen);
            return v;
        }

        private static string NormalizeUF(string? s)
        {
            var v = (s ?? string.Empty).Trim().ToUpperInvariant();
            if (v.Length != 2) throw new ArgumentException("UF inválido. Use 2 letras (ex.: SP).", nameof(s));
            return v;
        }

        private static string NormalizeZip(string? s)
        {
            var digits = new string((s ?? string.Empty).Where(char.IsDigit).ToArray());
            if (digits.Length != 8) throw new ArgumentException("CEP inválido. Use 8 dígitos.", nameof(s));
            return digits;
        }
    }
}