using FluentValidation;
using Pokedex.Business.Entities;

namespace Pokedex.Business.Validations
{
    public class PokemonValidation : AbstractValidator<Pokemon>
    {
        public PokemonValidation()
        {
            RuleFor(p => p.Name)
                .Length(3, 50)
                .WithMessage("O nome do pokémon deve ter entre {MinLength} e {MaxLength} caracteres.");

            RuleFor(p => p.CategoryId)
                .NotEmpty()
                .WithMessage("É necessário informar uma categoria válida.");

            //RuleForEach(p => p.Abilities)
            //    .Length(3, 50)
            //    .WithMessage("Todas as habilidades informadas devem ter entre {MinLength} e {MaxLength} caracteres.");
        }
    }
}