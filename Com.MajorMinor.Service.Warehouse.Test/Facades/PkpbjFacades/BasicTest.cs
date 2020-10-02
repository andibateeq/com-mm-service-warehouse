﻿using Com.MM.Service.Warehouse.Lib;
using Com.MM.Service.Warehouse.Lib.Interfaces;
using Com.MM.Service.Warehouse.Lib.Services;
using Com.MM.Service.Warehouse.Lib.ViewModels.PkbjByUserViewModel;
using Com.MM.Service.Warehouse.Test.DataUtils.SPKDocDataUtils;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Moq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xunit;

namespace Com.MM.Service.Warehouse.Test.Facades.PkpbjFacades
{
    public class BasicTest
    {
        private const string ENTITY = "MMPkpbjFacade";

        private const string USERNAME = "Unit Test";
        private IServiceProvider ServiceProvider { get; set; }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public string GetCurrentMethod()
        {
            StackTrace st = new StackTrace();
            StackFrame sf = st.GetFrame(1);

            return string.Concat(sf.GetMethod().Name, "_", ENTITY);
        }
        private Mock<IServiceProvider> GetServiceProvider()
        {
            HttpResponseMessage message = new HttpResponseMessage(System.Net.HttpStatusCode.OK);
            message.Content = new StringContent("{\"apiVersion\":\"1.0\",\"statusCode\":200,\"message\":\"Ok\",\"data\":[{\"Id\":7,\"code\":\"USD\",\"rate\":13700.0,\"date\":\"2018/10/20\"}],\"info\":{\"count\":1,\"page\":1,\"size\":1,\"total\":2,\"order\":{\"date\":\"desc\"},\"select\":[\"Id\",\"code\",\"rate\",\"date\"]}}");
            HttpResponseMessage messagePost = new HttpResponseMessage();
            var HttpClientService = new Mock<IHttpClientService>();
            HttpClientService
                .Setup(x => x.GetAsync(It.IsAny<string>()))
                .ReturnsAsync(message);
            HttpClientService
                .Setup(x => x.PostAsync(It.IsAny<string>(), It.IsAny<HttpContent>()))
                .ReturnsAsync(messagePost);
            var serviceProvider = new Mock<IServiceProvider>();
            serviceProvider
                .Setup(x => x.GetService(typeof(IdentityService)))
                .Returns(new IdentityService() { Token = "Token", Username = "Test" });

            serviceProvider
                .Setup(x => x.GetService(typeof(IHttpClientService)))
                .Returns(HttpClientService.Object);

            return serviceProvider;
        }

        private Mock<IServiceProvider> GetServiceProvidernulldataget()
        {
            HttpResponseMessage message = new HttpResponseMessage(System.Net.HttpStatusCode.OK);
            message.Content = new StringContent("{\"apiVersion\":\"1.0\",\"statusCode\":200,\"message\":\"Ok\",\"data\":[{ }}");
            HttpResponseMessage messagePost = new HttpResponseMessage();
            var HttpClientService = new Mock<IHttpClientService>();
            HttpClientService
                .Setup(x => x.GetAsync(It.IsAny<string>()))
                .ReturnsAsync(message);
            //HttpClientService
            //    .Setup(x => x.PostAsync(It.IsAny<string>(), It.IsAny<HttpContent>()))
            //    .ReturnsAsync(messagePost);
            var serviceProvider = new Mock<IServiceProvider>();
            serviceProvider
                .Setup(x => x.GetService(typeof(IdentityService)))
                .Returns(new IdentityService() { Token = "Token", Username = "Test" });

            serviceProvider
                .Setup(x => x.GetService(typeof(IHttpClientService)))
                .Returns(HttpClientService.Object);

            return serviceProvider;
        }
        private WarehouseDbContext _dbContext(string testName)
        {
            DbContextOptionsBuilder<WarehouseDbContext> optionsBuilder = new DbContextOptionsBuilder<WarehouseDbContext>();
            optionsBuilder
                .UseInMemoryDatabase(testName)
                .ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning));

            WarehouseDbContext dbContext = new WarehouseDbContext(optionsBuilder.Options);

            return dbContext;
        }

        private SPKDocDataUtil dataUtil(Com.MM.Service.Warehouse.Lib.Facades.PkpbjFacade facade, string testName)
        {
            var pkbbjfacade = new Com.MM.Service.Warehouse.Lib.Facades.PkpbjFacade(ServiceProvider, _dbContext(testName));
            //var sPKDocDataUtil = new SPKDocDataUtil(pkbbjfacade);
            //var transferFacade = new TransferFacade(ServiceProvider, _dbContext(testName));
            //var transferDataUtil = new TransferDataUtil(transferFacade, sPKDocDataUtil);

            return new SPKDocDataUtil(facade);
        }

        [Fact]
        public async Task Should_Success_Create_Data()
        {

            Com.MM.Service.Warehouse.Lib.Facades.PkpbjFacade facade = new Com.MM.Service.Warehouse.Lib.Facades.PkpbjFacade(GetServiceProvider().Object, _dbContext(GetCurrentMethod()));
            var model = dataUtil(facade, GetCurrentMethod()).GetNewData();
            var Response = await facade.Create(model, USERNAME);
            Assert.NotEqual(0, Response);
        }
        [Fact]
        public async Task Should_Success_Create_Data2()
        {

            Com.MM.Service.Warehouse.Lib.Facades.PkpbjFacade facade = new Com.MM.Service.Warehouse.Lib.Facades.PkpbjFacade(GetServiceProvidernulldataget().Object, _dbContext(GetCurrentMethod()));
            var model = dataUtil(facade, GetCurrentMethod()).GetNewData();
            var Response = await facade.Create(model, USERNAME);
            Assert.NotEqual(0, Response);
        }
        [Fact]
        public async Task Should_Success_Get_All_Data()
        {
            Com.MM.Service.Warehouse.Lib.Facades.PkpbjFacade facade = new Com.MM.Service.Warehouse.Lib.Facades.PkpbjFacade(GetServiceProvider().Object, _dbContext(GetCurrentMethod()));
            var model = await dataUtil(facade, GetCurrentMethod()).GetTestData();
            var Response = facade.Read();
            Assert.NotEmpty(Response.Item1);
        }
        [Fact]
        public async Task Should_Success_Get_All_Data_Expedition()
        {
            Com.MM.Service.Warehouse.Lib.Facades.PkpbjFacade facade = new Com.MM.Service.Warehouse.Lib.Facades.PkpbjFacade(GetServiceProvider().Object, _dbContext(GetCurrentMethod()));
            var model = await dataUtil(facade, GetCurrentMethod()).GetTestData();
            var Response = facade.ReadExpedition();
            Assert.NotEmpty(Response.Item1);
        }
        [Fact]
        public async Task Should_Success_Get_Data_By_Id()
        {
            Com.MM.Service.Warehouse.Lib.Facades.PkpbjFacade facade = new Com.MM.Service.Warehouse.Lib.Facades.PkpbjFacade(GetServiceProvider().Object, _dbContext(GetCurrentMethod()));
            var model = await dataUtil(facade, GetCurrentMethod()).GetTestData();
            var Response = facade.ReadById((int)model.Id);
            Assert.NotNull(Response);
        }
        [Fact]
        public async Task Should_Success_Get_Data_By_reference()
        {
            Com.MM.Service.Warehouse.Lib.Facades.PkpbjFacade facade = new Com.MM.Service.Warehouse.Lib.Facades.PkpbjFacade(GetServiceProvider().Object, _dbContext(GetCurrentMethod()));
            var model = await dataUtil(facade, GetCurrentMethod()).GetTestData();
            var Response = facade.ReadByReference(model.Reference);
            Assert.NotNull(Response);
        }
        [Fact]
        public void Should_Success_Validate_Data()
        {
            PkbjByUserViewModel ViewModel = new PkbjByUserViewModel
            {
                
                destination = new Lib.ViewModels.NewIntegrationViewModel.DestinationViewModel
                {
                    _id = 1,
                    code = "test",
                    name = "test"
                },
                reference = "test",
                items = new List<PkbjByUserItemViewModel> {
                    new PkbjByUserItemViewModel
                    {
                        item = new Lib.ViewModels.NewIntegrationViewModel.ItemViewModel
                        {
                            code = "test"
                        }
                    }
                }

            };
            Assert.True(ViewModel.Validate(null).Count() > 0);

            PkbjByUserViewModel destViewModel = new PkbjByUserViewModel
            {
                source = new Lib.ViewModels.NewIntegrationViewModel.SourceViewModel
                {
                    _id = 1,
                    code = "test",
                    name = "test"
                },
                
                reference = "test",
                items = new List<PkbjByUserItemViewModel> {
                    new PkbjByUserItemViewModel
                    {
                        item = new Lib.ViewModels.NewIntegrationViewModel.ItemViewModel
                        {
                            code = "test"
                        }
                    }
                }

            };
            Assert.True(destViewModel.Validate(null).Count() > 0);
            PkbjByUserViewModel refViewModel = new PkbjByUserViewModel
            {
                source = new Lib.ViewModels.NewIntegrationViewModel.SourceViewModel
                {
                    _id = 1,
                    code = "test",
                    name = "test"
                },
                destination = new Lib.ViewModels.NewIntegrationViewModel.DestinationViewModel
                {
                    _id = 1,
                    code = "test",
                    name = "test"
                },
                items = new List<PkbjByUserItemViewModel> {
                    new PkbjByUserItemViewModel
                    {
                        item = new Lib.ViewModels.NewIntegrationViewModel.ItemViewModel
                        {
                            code = "test"
                        }
                    }
                }

            };
            Assert.True(refViewModel.Validate(null).Count() > 0);
            PkbjByUserViewModel itemViewModel = new PkbjByUserViewModel
            {
                source = new Lib.ViewModels.NewIntegrationViewModel.SourceViewModel
                {
                    _id = 1,
                    code = "test",
                    name = "test"
                },
                destination = new Lib.ViewModels.NewIntegrationViewModel.DestinationViewModel
                {
                    _id = 1,
                    code = "test",
                    name = "test"
                },
                reference = "test"

            };
            Assert.True(itemViewModel.Validate(null).Count() > 0);
            PkbjByUserViewModel itemsViewModel = new PkbjByUserViewModel
            {
                source = new Lib.ViewModels.NewIntegrationViewModel.SourceViewModel
                {
                    _id = 1,
                    code = "test",
                    name = "test"
                },
                destination = new Lib.ViewModels.NewIntegrationViewModel.DestinationViewModel
                {
                    _id = 1,
                    code = "test",
                    name = "test"
                },
                reference = "test",
                items = new List<PkbjByUserItemViewModel> {
                    
                }

            };
            Assert.True(itemsViewModel.Validate(null).Count() > 0);
        }

    }
}
