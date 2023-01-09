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

            RuleFor(p => p.Hp)
                .GreaterThan(0)
                .WithMessage("O HP não pode ser menor ou igual a zero.");

            RuleFor(p => p.Attack)
                .GreaterThan(0)
                .WithMessage("O ataque não pode ser menor ou igual a zero.");

            RuleFor(p => p.Defense)
                .GreaterThan(0)
                .WithMessage("A defesa não pode ser menor ou igual a zero.");

            RuleFor(p => p.Speed)
                .GreaterThan(0)
                .WithMessage("A velocidade não pode ser menor ou igual a zero.");
        }
    }
}