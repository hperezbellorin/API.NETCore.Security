using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Seguridad.Datos.NLog;
using Seguridad.Datos.Repositorio.Interfaces;
using Seguridad.Modelo;

namespace ApplicacionSeguridad.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DetalleVisitaController : ControllerBase
    {
       private readonly IDetalleVisitaRepo _detvisita;
        private ILog logger;
        public DetalleVisitaController(IDetalleVisitaRepo detalleVisista, ILog logger)
        {
            this._detvisita = detalleVisista;
            this.logger = logger;
        }

        // GET: api/Student
        [HttpGet]
        public IActionResult Get()

        {
            return Ok(_detvisita.GetAll());
        }

        // POST: api/Student
       [HttpPost]
       // [HttpPost("Post")]
        public async Task<IActionResult> Post([FromBody] DetalleVisita detallevisita)
        {
            try
            {
                if (detallevisita != null)
                {
                    await _detvisita.Add(detallevisita);
                    return Ok();
                }
               
            }
            catch (Exception ex)
            {
                logger.Information(ex.Message);

                // throw;
            }
            return Created(HttpContext.Request.Host + Request.Path + " /" + detallevisita.IdVisita, detallevisita);

        }


    }
}