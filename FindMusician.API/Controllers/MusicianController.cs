using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FindMusician.API.Services;
using FindMusician.API.Models;
using Microsoft.AspNetCore.Authorization;

namespace FindMusician.API.Controllers
{
    [Produces("application/json")]
    [Route("api/musician")]
    public class MusicianController : BaseController
    {
        private MusicianService _service;

        public MusicianController(MusicianService service)
        {
            _service = service;
        }

        [Authorize("Bearer")]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var result = _service.Get();
                return Response(result);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return Response();
            }
        }

        [Authorize("Bearer")]
        [HttpGet("{id}", Name = "Get")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return Response();
            }

            try
            {
                var result = _service.Get(id);
                if (result == null)
                {
                    ModelState.AddModelError(string.Empty, "Músico não encontrado.");
                    return Response();
                }

                return Response(result);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return Response();
            }
        }

        [Authorize("Bearer")]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Musician musician)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Response();
                }

                _service.Add(musician);
                return Response(musician);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return Response();
            }
        }

        [Authorize("Bearer")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromRoute] int id, [FromBody] Musician musician)
        {
            if (!ModelState.IsValid)
            {
                return Response();
            }

            if (id != musician.Id)
            {
                ModelState.AddModelError(string.Empty, "O ID informado não é valido");
                return Response();
            }

            try
            {
                _service.Update(musician);
                return Response();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return Response();
            }
        }

        [Authorize("Bearer")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return Response();
            }

            if (id == 0)
            {
                ModelState.AddModelError(string.Empty, "O ID informado não é valido");
                return Response();
            }

            try
            {
                _service.Delete(id);
                return Response(id);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return Response();
            }
        }
    }
}
