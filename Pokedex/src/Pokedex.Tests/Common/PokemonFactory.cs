using Bogus;
using Pokedex.Business.Entities;
using Pokedex.Business.Enums;

namespace Pokedex.Tests.Common
{
    public static class PokemonFactory
    {
        public static readonly Faker _faker = new(); 

        public static Pokemon CreateValid()
        { 
            return new Pokemon(Guid.NewGuid().ToString(), Guid.NewGuid(), _faker.Random.Enum<Gender>(), 1, 1, 1, 1);
        }

        public static Pokemon CreateInvalid()
        {
            return new Pokemon(string.Empty, Guid.Empty, Gender.All, 1, 1, 1, 1);
        }
    }
}