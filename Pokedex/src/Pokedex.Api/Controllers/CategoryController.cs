using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Pokedex.Api.Models;
using Pokedex.Business.Repositories;
using Swashbuckle.AspNetCore.Annotations;

namespace Pokedex.Api.Controllers
{
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public CategoryController(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _mapper = mapper;
            _categoryRepository = categoryRepository;
        }

        [HttpGet("all")]
        [SwaggerOperation("Listar todas as categorias de pokémons.")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            var categories = await _categoryRepository.GetAll();

            HttpContext.Response.Headers.Add("X-Total-Count", categories.Count().ToString());

            var result = _mapper.Map<IEnumerable<CategoryModel>>(categories);
            return Ok(result);
        }
    }
}
