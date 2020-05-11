using GraphicsTemplate.Api.Mapping;
using GraphicsTemplate.ApplicationServices;
using Microsoft.AspNetCore.Mvc;
using System;

namespace GraphicsTemplate.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GraphicsController
    {
        private readonly TransformMapper _mapper;
        private readonly IGraphicsService _service;

        public GraphicsController(TransformMapper mapper, IGraphicsService service)
        {
            _mapper = mapper;
            _service = service;
        }

        [HttpPost]
        [Route("transform/{id}")]
        public void Transform([FromRoute] Guid id, [FromBody] Dto.Transform transform)
        {
            // _validator.Validate(transform)
            _service.SetTransform(id, _mapper.Map(transform));
        }
    }
}
