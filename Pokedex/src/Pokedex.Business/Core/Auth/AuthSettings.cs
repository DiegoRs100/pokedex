namespace Pokedex.Business.Core.Auth
{
    public class AuthSettings
    {
        public string Secret { get; set; } = default!;
        public string Issuer { get; set; } = default!;
        public string ValidIn { get; set; } = default!;
    }
}