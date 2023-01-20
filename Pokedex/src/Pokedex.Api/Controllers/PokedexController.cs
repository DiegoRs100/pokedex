using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pokedex.Api.Models;
using Pokedex.Business.Entities;
using Pokedex.Business.Queries;
using Pokedex.Business.Repositories;
using Pokedex.Business.Services;
using Swashbuckle.AspNetCore.Annotations;

namespace Pokedex.Api.Controllers
{
    //[Authorize]
    [Route("pokedex")]
    public class PokedexController : ControllerBase
    {
        private readonly IPokedexService _pokedexService;
        private readonly IPokemonRepository _pokemonRepository;
        private readonly IMapper _mapper;

        public PokedexController(IPokedexService pokedexService, 
                                 IMapper mapper,
                                 IPokemonRepository pokemonRepository)
        {
            _mapper = mapper;
            _pokedexService = pokedexService;
            _pokemonRepository = pokemonRepository;
        }

        [HttpPost]
        [SwaggerOperation("Cadastrar pokémon.")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> AddPokemon([FromBody] PokemonModel model)
        {
            var pokemon = _mapper.Map<Pokemon>(model);
            var pokemonId = await _pokedexService.AddPokemon(pokemon);

            return Created($"{HttpContext.Request.Path}/{pokemonId}", null);
        }

        [HttpPut]
        [SwaggerOperation("Atualizar cadastro de pokémon.")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> UpdatePokemon([FromBody] PokemonModel model) 
        {
            var pokemon = _mapper.Map<Pokemon>(model);
            await _pokedexService.UpdatePokemon(pokemon);

            return NoContent();
        }

        [HttpDelete("{pokemonId:guid}")]
        [SwaggerOperation("Remover pokémon.")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeletePokemon(Guid pokemonId)
        {
            await _pokedexService.DeletePokemon(pokemonId);
            return NoContent();
        }

        [HttpGet("{pokemonId:guid}")]
        [SwaggerOperation("Obter pokémon por Id.")]
        [ProducesResponseType(typeof(PokemonModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetPokemonById(Guid pokemonId)
        {
            var pokemon = await _pokemonRepository.GetById(pokemonId);

            if (pokemon == null)
                return NotFound();

            var result = _mapper.Map<PokemonModel>(pokemon);
            return Ok(result);
        }

        [HttpGet("find")]
        [SwaggerOperation("Listar pokémons.")]
        [ProducesResponseType(typeof(IEnumerable<PokemonModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> FindPokemons(FindPokemonQuery query)
        {
            var pokemons = await _pokemonRepository.Find(query);

            HttpContext.Response.Headers.Add("X-Total-Count", pokemons.Count().ToString());

            var result = _mapper.Map<IEnumerable<PokemonModel>>(pokemons);
            return Ok(result);
        }
    }
}