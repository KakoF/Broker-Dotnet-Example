using Domain.Interfaces.Cache;
using Domain.Interfaces.Repositories;
using Domain.Models;
using Moq;
using Service.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Service.Test.Services
{
    public class EntidadeServiceTests
    {

        private readonly MockRepository _mockRepository;
        private readonly Mock<IEntidadeMetodoRepository> _mockEntidadeMetodoRepository;
        private readonly Mock<ICache> _mockCache;

        private readonly EntidadeService _entidadeService;


        public EntidadeServiceTests()
        {
            _mockRepository = new MockRepository(MockBehavior.Loose);

            _mockEntidadeMetodoRepository = _mockRepository.Create<IEntidadeMetodoRepository>();
            _mockCache = _mockRepository.Create<ICache>();

            _entidadeService = new EntidadeService(_mockEntidadeMetodoRepository.Object, _mockCache.Object);
        }




        [Fact(DisplayName = "Quando buscar entidades configuradas para realizar contatagem, serviço deverá encontrar ao menos um registro")]
        public async Task GetEntidadesConfiguradas()
        {
            //Arr
            var entidadesConfiguradsa = new List<EntidadeConfiguradaModel>()
            {
                new EntidadeConfiguradaModel() { EntidadeId = 100515 }
            };
            _mockEntidadeMetodoRepository.Setup(c => c.ObterEntidadesConfiguradasTransbordoAsync()).ReturnsAsync(entidadesConfiguradsa);
            _mockCache.Setup(c => c.GetOrCreateAsync(It.IsAny<string>(), It.IsAny<Func<Task<IEnumerable<EntidadeConfiguradaModel>>>>(), null)).ReturnsAsync(entidadesConfiguradsa);

            //Act
            var result = await _entidadeService.GetEntidadesConfiguradasAsync();


            //Assert
            Assert.NotNull(result);
        }

        [Fact(DisplayName = "Quando verificar se entidade está configurada para realizar contatagem, serviço deverá retornar verdadeiro")]
        public async Task EntidadeConfiguradaParaContador()
        {
            //Arr
            var entidadesConfiguradsa = new List<EntidadeConfiguradaModel>()
            {
                new EntidadeConfiguradaModel() { EntidadeId = 100515 }
            };
            _mockEntidadeMetodoRepository.Setup(c => c.ObterEntidadesConfiguradasTransbordoAsync()).ReturnsAsync(entidadesConfiguradsa);
            _mockCache.Setup(c => c.GetOrCreateAsync(It.IsAny<string>(), It.IsAny<Func<Task<IEnumerable<EntidadeConfiguradaModel>>>>(), null)).ReturnsAsync(entidadesConfiguradsa);
            //Act
            var result = await _entidadeService.EntidadeConfiguradaParaContador(100515);

            //Assert
            Assert.True(result);
        }
    }
}
