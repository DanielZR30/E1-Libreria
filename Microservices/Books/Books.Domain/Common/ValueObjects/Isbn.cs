using Books.Domain.Exceptions;
using System.Text.RegularExpressions;

namespace Books.Domain.Common.ValueObjects
{
    public sealed class Isbn : IEquatable<Isbn>
    {
        public string Value { get; private set; } = null!;

        private Isbn() { }

        private Isbn(string value)
        {
            Value = value;
        }

        public static Isbn Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new BussinesRuleException("El ISBN es requerido.");
            }

            string cleanIsbn = value.Replace("-", "").Replace(" ", "").Trim();

            if (cleanIsbn.Length != 10 && cleanIsbn.Length != 13)
            {
                throw new BussinesRuleException("El ISBN debe tener 10 o 13 dígitos numéricos.");
            }

            if (cleanIsbn.Length == 13 && !Regex.IsMatch(cleanIsbn, @"^\d{13}$"))
            {
                throw new BussinesRuleException("El ISBN-13 debe contener únicamente dígitos numéricos.");
            }

            if (cleanIsbn.Length == 10 && !Regex.IsMatch(cleanIsbn, @"^\d{9}[\dX]$", RegexOptions.IgnoreCase))
            {
                throw new BussinesRuleException("El formato del ISBN-10 no es válido.");
            }

            return new Isbn(value.Trim());
        }

        public override string ToString() => Value;

        public override bool Equals(object? obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj is Isbn other && Equals(other);
        }

        public bool Equals(Isbn? other)
        {
            if (other is null) return false;
            string thisClean = Value.Replace("-", "").Replace(" ", "");
            string otherClean = other.Value.Replace("-", "").Replace(" ", "");
            return string.Equals(thisClean, otherClean, StringComparison.OrdinalIgnoreCase);
        }

        public override int GetHashCode()
        {
            return Value.Replace("-", "").Replace(" ", "").ToUpperInvariant().GetHashCode();
        }

        public static bool operator ==(Isbn? left, Isbn? right) => Equals(left, right);
        public static bool operator !=(Isbn? left, Isbn? right) => !Equals(left, right);
    }
}
