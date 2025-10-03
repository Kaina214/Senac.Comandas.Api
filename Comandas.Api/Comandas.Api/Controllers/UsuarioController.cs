using Comandas.Api.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Comandas.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        //Lista Usuarios 
      static List<Usuario> usuarios = new List<Usuario>()
{
         new Usuario
        {
          Id = 1,
          Nome = "Admin",
          Email = "admin@admin.com",
          Senha = "admin123"
         },
          new Usuario
            {
             Id = 2,
            Nome = "Admin",
            Email = "admin@admin.com",
            Senha = "admin123"
            }
        };


        // GET: api/<UsuarioController>
        [HttpGet]
        public IResult Get()
        {
            return Results.Ok(usuarios);
        }


        // GET api/<UsuarioController>/5
        [HttpGet("{id}")]
        public IResult Get(int id)
        {
            var usuario = usuarios.FirstOrDefault(u => u.Id == id);
            if (usuario is null)
            {
                return Results.NotFound("Usuário não encontrado!");
            }
            return Results.Ok(usuario);
        }

        // POST api/<UsuarioController>
        [HttpPost]
        public IResult Post([FromBody] Usuario usuario)
        {
            //adiciona o usuario na lista
            usuarios.Add(usuario);

           return Results.Created($"/api/usuario/{usuario.Id}", usuario);
            //CreatedAtAction(nameof(Get), new { id = usuario.Id }, usuario);
            //retorna o usuario criad
        }

        // PUT api/<UsuarioController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<UsuarioController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
