using Comandas.Api.DTOs;
using Comandas.Api.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Comandas.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        //variavel que represeenta o banco de dados
        public ComandasDbContext _context { get; set; }
        //construtor
        public UsuarioController(ComandasDbContext context)
        {
            _context = context;
        }
    
        //iresult que retorna  a lista de usuarios
        // GET: api/<UsuarioController>
        [HttpGet]
        public IResult Get()
        {   //conectar no banco 
            //executar a consulta SELECT * FROM usuarios
            var usuarios = _context.Usuarios.ToList();
            return Results.Ok(usuarios);
        }


        // GET api/<UsuarioController>/5
        [HttpGet("{id}")]
        public IResult Get(int id)
        {
            var usuario = _context.Usuarios.
                FirstOrDefault(u => u.Id == id);
            if (usuario is null)
            {
                return Results.NotFound("Usuário não encontrado!");
            }
            return Results.Ok(usuario);
        }

        // POST api/<UsuarioController>
        [HttpPost]
        public IResult Post([FromBody] UsuarioCreateRequest usuarioCreate)
        {
            if (usuarioCreate.Senha.Length <6  )
            {
                return Results.BadRequest("A senha deve ter no minínimo 6 caracteres ");
            }
            if (usuarioCreate.Nome.Length <3) 
            {
                return Results.Conflict("O nome deve ter no minínimo 3 caracteres ");
            }
            if (usuarioCreate.Email.Length < 5 || !usuarioCreate.Email.Contains("@"))
            {
                return Results.Conflict("O email deve ser válido ");
            }



            //cria um novo usuario
            var usuario = new Usuario
            {
            
                Nome = usuarioCreate.Nome,
                Email = usuarioCreate.Email,
                Senha = usuarioCreate.Senha
            };
            //adiciona o usuario na lista
            _context.Usuarios.Add(usuario);

            // EXECUTA o INSERT INTO Usuarios (Id , Nome, Email, Senha) VALUES (...)
            _context.SaveChanges();//serve para salvar as alterações no banco de dados

            return Results.Created($"/api/usuario/{usuario.Id}", usuario);
            //CreatedAtAction(nameof(Get), new { id = usuario.Id }, usuario);
            //retorna o usuario criad
        }
        /// <summary>
        /// Atualiza um usuário existente
        /// </summary>
        /// <param name="id"></param>
        /// <param name="usuarioUpdate"></param>
        /// <returns></returns>
        //ATUALIZA UM USUARIO
        // PUT api/<UsuarioController>/5
        [HttpPut("{id}")]
        public IResult Put(int id, [FromBody] UsuarioUpdateRequest usuarioUpdate)
        {
            var usuario = _context.Usuarios.
                FirstOrDefault(u => u.Id == id);
            if (usuario is null)
                return Results.NotFound($"Usuário do id {id} não encontrado");//404
            //Atualiza os dados do usuario
            usuario.Nome = usuarioUpdate.Nome;
            usuario.Email = usuarioUpdate.Email;
            usuario.Senha = usuarioUpdate.Senha;

            //update no banco de dados
            _context.SaveChanges();
            //Retorna sem content
            return Results.NoContent();//204
        }

        // DELETE api/<UsuarioController>/5
        [HttpDelete("{id}")]
        public IResult Delete(int id)
        {
            var usuario = _context.Usuarios.
                FirstOrDefault(u => u.Id == id);
            if (usuario is null)

                return Results.NotFound($"usuario{id}não encontrado!");
            _context.Usuarios.Remove(usuario);
            var removido = _context.SaveChanges();
            if (removido > 0)
            {
                return Results.NoContent();
            }
           return Results.StatusCode(500);


        }
    }
}
