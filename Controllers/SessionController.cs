using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Seguridad.Datos.Repositorio.Interfaces;
using Seguridad.Modelo;

namespace ApplicacionSeguridad.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SessionController : ControllerBase
    {
        private readonly ISesionRepo _session;

        public SessionController(ISesionRepo session)
        {
            this._session = session;
        }




        //public string Get()
        //{
        //    var claimsIdentity = this.User.Identity as ClaimsIdentity;
        //    var claim = claimsIdentity.Claims;
        //    // or
        //    var data = claimsIdentity.FindFirst("userId").Value;
        //    return data;
        //}





        //// GET: api/Student
        [Authorize]
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_session.GetAll());
        }
        // GET: api/session/5
        [HttpGet("{id}", Name = "Get")]
        public IActionResult Get(int id)
        {
            var session = _session.Find(id);
            if (session != null)
            {
                return Ok(session);
            }
            else
            {
                return NotFound();
            }

        }

        // POST: api/session
        [HttpPost]
        public IActionResult Post([FromBody] Sesion session)
        {
            if (session != null)
            {
                _session.Add(session);
            }
            return Created(HttpContext.Request.Host + Request.Path + " /" + session.IdSesion, session);

        }




    }
}