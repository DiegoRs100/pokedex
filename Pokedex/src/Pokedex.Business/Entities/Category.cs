using Pokedex.Business.Core;

namespace Pokedex.Business.Entities
{
    public class Category : Entity
    {
        public Guid Name { get; private set; }
    }
}