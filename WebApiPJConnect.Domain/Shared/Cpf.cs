namespace WebApiPJConnect.Domain.Shared
{
    public sealed class Cpf : IEquatable<Cpf>
    {
        public string Value { get; private set; }

        private Cpf() { } // EF Core

        public Cpf(string value)
        {
            var digits = OnlyDigits(value);
            if (!IsValid(digits))
                throw new ArgumentException("CPF inválido.", nameof(value));

            Value = digits;
        }

        public string Formatted => $"{Value[..3]}.{Value.Substring(3, 3)}.{Value.Substring(6, 3)}-{Value[^2..]}";

        public override string ToString() => Value;

        // Equality (Value Object)
        public bool Equals(Cpf? other) => other is not null && Value == other.Value;
        public override bool Equals(object? obj) => obj is Cpf v && Equals(v);
        public override int GetHashCode() => Value.GetHashCode(StringComparison.Ordinal);

        // Helpers
        private static string OnlyDigits(string s) => new(s.Where(char.IsDigit).ToArray());

        private static bool IsValid(string digits)
        {
            if (string.IsNullOrWhiteSpace(digits) || digits.Length != 11) return false;
            if (digits.Distinct().Count() == 1) return false;

            var nums = digits.Select(c => c - '0').ToArray();

            int Calc(int len)
            {
                var sum = 0;
                var weight = len + 1;
                for (int i = 0; i < len; i++) sum += nums[i] * (weight - i);
                var r = sum % 11;
                return r < 2 ? 0 : 11 - r;
            }

            var d1 = Calc(9);
            if (nums[9] != d1) return false;

            var d2 = Calc(10);
            return nums[10] == d2;
        }
    }
}
