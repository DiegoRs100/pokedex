using Pokedex.Business.Core;

namespace Pokedex.Business.Entities
{
    public class PokemonAbility : Entity
    {
        public string Name { get; set; } = default!;
        public Guid PokemonId { get; set; }
    }
}