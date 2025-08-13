namespace WebApiPJConnect.Domain.Shared
{
    public sealed class Cnpj : IEquatable<Cnpj>
    {
        public string Value { get; private set; }

        private Cnpj() { } // EF Core

        public Cnpj(string value)
        {
            var digits = OnlyDigits(value);
            if (!IsValid(digits))
                throw new ArgumentException("CNPJ inválido.", nameof(value));

            Value = digits;
        }

        public string Formatted => $"{Value[..2]}.{Value.Substring(2, 3)}.{Value.Substring(5, 3)}/{Value.Substring(8, 4)}-{Value[^2..]}";

        public override string ToString() => Value;

        // Equality (Value Object)
        public bool Equals(Cnpj? other) => other is not null && Value == other.Value;
        public override bool Equals(object? obj) => obj is Cnpj v && Equals(v);
        public override int GetHashCode() => Value.GetHashCode(StringComparison.Ordinal);

        // Helpers
        private static string OnlyDigits(string s) => new(s.Where(char.IsDigit).ToArray());

        private static bool IsValid(string digits)
        {
            if (string.IsNullOrWhiteSpace(digits) || digits.Length != 14) return false;
            if (digits.Distinct().Count() == 1) return false; // evita 000.../111...

            var nums = digits.Select(c => c - '0').ToArray();

            int Calc(int[] w, int len)
            {
                var sum = 0;
                for (int i = 0; i < len; i++) sum += nums[i] * w[i];
                var r = sum % 11;
                return r < 2 ? 0 : 11 - r;
            }

            // pesos
            var w1 = new[] { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            var w2 = new[] { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };

            var d1 = Calc(w1, 12);
            if (nums[12] != d1) return false;

            var d2 = Calc(w2, 13);
            return nums[13] == d2;
        }
    }
}
