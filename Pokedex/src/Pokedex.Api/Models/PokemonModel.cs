using Pokedex.Business.Enums;

namespace Pokedex.Api.Models
{
    public class PokemonModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;
        public string Category { get; set; } = default!;
        public Gender Gender { get; set; }

        public int Hp { get; private set; }
        public int Attack { get; private set; }
        public int Defense { get; private set; }
        public int Speed { get; private set; }
    }
}