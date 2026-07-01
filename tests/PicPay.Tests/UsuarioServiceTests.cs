using Microsoft.AspNetCore.Http;
using Moq;
using PicPay.Domains;
using PicPay.Factory;
using PicPay.Repository.UsuarioRepositories;
using PicPay.Services.UsuarioServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PicPay.Tests
{
    public class UsuarioServiceTests
    {
        private readonly UsuarioService _service;
        private readonly Mock<IUsuarioRepository> usuarioRepository;
        private readonly Usuario usuarioTest;
        //private readonly IFormFile file;

        public UsuarioServiceTests()
        {
           usuarioRepository = new Mock<IUsuarioRepository>();

           _service = new UsuarioService(usuarioRepository.Object);

            var usuario = UsuarioFactory.UsuarioLojista();

            //using var stream = new MemoryStream();
            //file = new FormFile(stream, 0,stream.Length,"formtest","filetest");
        }

        /*[Fact]
        public async Task SaveUsuarioAsyncTest()
        {
            var dto = new UsuarioDTO(); 

            await _service.SaveUsuarioAsync(dto);

            usuarioRepository.Setup(x => x.SaveAsync(usuarioTest))
                .Returns(Task.FromResult(usuarioTest));
        }*/
    }
}
