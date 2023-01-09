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

        public int Hp { get; private set; }
        public int Attack { get; private set; }
        public int Defense { get; private set; }
        public int Speed { get; private set; }

        public Category Category { get; private set; } = default!;

        public Pokemon(string name, Guid categoryId, Gender gender, int hp, int attack, int defense, int speed)
        {
            Name = name;
            CategoryId = categoryId;
            Gender = gender;
            Hp = hp;
            Attack = attack;
            Defense = defense;
            Speed = speed;
        }

        public override ValidationResult Validate()
        {
            return new PokemonValidation().Validate(this);
        }
    }
}