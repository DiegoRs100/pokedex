using FluentValidation.Results;
using Pokedex.Business.Core;
using Pokedex.Business.Enums;
using Pokedex.Business.Validations;

namespace Pokedex.Business.Entities
{
    public class Pokemon : Entity
    {
        public string Name { get; private set; }
        public Guid CategoryId { get; private set; }
        public Gender Gender { get; private set; }
        public List<PokemonAbility> Abilities { get; private set; } = new();

        public Pokemon(string name, Guid categoryId, Gender gender)
        {
            Name = name;
            CategoryId = categoryId;
            Gender = gender;
        }

        public override ValidationResult Validate()
        {
            return new PokemonValidation().Validate(this);
        }
    }
}