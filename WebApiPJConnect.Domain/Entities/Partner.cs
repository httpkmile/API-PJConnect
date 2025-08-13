using WebApiPJConnect.Domain.Shared;

namespace WebApiPJConnect.Domain.Entities
{
    /// <summary>
    /// Entidade de Sócio vinculada a uma Company.
    /// </summary>
    public class Partner
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        public string Name { get; private set; } = default!;
        public Cpf Cpf { get; private set; } = default!;

        // Construtor sem parâmetros para o EF Core
        private Partner() { }

        public Partner(string name, Cpf cpf)
        {
            SetName(name);
            Cpf = cpf ?? throw new ArgumentNullException(nameof(cpf));
        }

        public void Update(string name, Cpf cpf)
        {
            SetName(name);
            Cpf = cpf ?? throw new ArgumentNullException(nameof(cpf));
        }

        private void SetName(string name)
        {
            var v = (name ?? string.Empty).Trim();
            if (v.Length == 0) throw new ArgumentException("Nome do sócio é obrigatório.", nameof(name));
            if (v.Length > 120) v = v[..120];
            Name = v;
        }
    }
}
