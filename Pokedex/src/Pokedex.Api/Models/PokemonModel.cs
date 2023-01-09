using Pokedex.Business.Enums;

namespace Pokedex.Api.Models
{
    public class PokemonModel
    {
        public string Name { get; set; } = default!;
        public string Category { get; set; } = default!;
        public Gender Gender { get; set; }
        public List<PokemonAbilityModel> Abilities { get; set; } = new();
    }
}