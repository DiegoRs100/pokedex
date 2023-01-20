using Pokedex.Business.Core;

namespace Pokedex.Business.Entities
{
    public class Category : Entity
    {
        public string Name { get; private set; } = default;
    }
}