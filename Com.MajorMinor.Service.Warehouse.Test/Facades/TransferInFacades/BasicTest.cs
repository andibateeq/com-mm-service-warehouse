﻿using Com.MM.Service.Warehouse.Lib;
using Com.MM.Service.Warehouse.Lib.Facades;
using Com.MM.Service.Warehouse.Lib.Interfaces;
using Com.MM.Service.Warehouse.Lib.Models.InventoryModel;
using Com.MM.Service.Warehouse.Lib.Services;
using Com.MM.Service.Warehouse.Test.DataUtils.SPKDocDataUtils;
using Com.MM.Service.Warehouse.Test.DataUtils.TransferDataUtils;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Moq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Com.MM.Service.Warehouse.Test.Facades.TransferInFacades
{
    public class BasicTest
    {
        private const string ENTITY = "MMTransferIn";

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
            var HttpClientService = new Mock<IHttpClientService>();
            HttpClientService
                .Setup(x => x.GetAsync(It.IsAny<string>()))
                .ReturnsAsync(message);

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

        private TransferDataUtil dataUtil(TransferFacade facade, string testName)
        {
            var pkbbjfacade = new Com.MM.Service.Warehouse.Lib.Facades.PkpbjFacade(ServiceProvider, _dbContext(testName));
            var sPKDocDataUtil = new SPKDocDataUtil(pkbbjfacade);
            var transferFacade = new TransferFacade(ServiceProvider, _dbContext(testName));
            var transferDataUtil = new TransferDataUtil(transferFacade, sPKDocDataUtil);

            return new TransferDataUtil(facade, sPKDocDataUtil);
        }

        [Fact]
        public async Task Should_Success_Create_Data()
        {

            TransferFacade facade = new TransferFacade(GetServiceProvider().Object, _dbContext(GetCurrentMethod()));
            var model = await dataUtil(facade, GetCurrentMethod()).GetNewData();
            var Response = await facade.Create(model, USERNAME);
            Assert.NotEqual(0, Response);
        }

        [Fact]
        public async Task Should_Success_Create_Data_2()
        {
            DbSet<Inventory> dbSetInventory = _dbContext(GetCurrentMethod()).Set<Inventory>();
            Inventory inventory = new Inventory
            {
                ItemArticleRealizationOrder = "art1",
                ItemCode = "code",
                ItemDomesticCOGS = 0,
                ItemDomesticRetail = 0,
                ItemDomesticSale = 0,
                ItemDomesticWholeSale = 0,
                ItemId = 1,
                ItemInternationalCOGS = 0,
                ItemInternationalRetail = 0,
                ItemInternationalSale = 0,
                ItemInternationalWholeSale = 0,
                ItemName = "name",
                ItemSize = "size",
                Quantity = 1,
                ItemUom = "uom",
                StorageCode = "code",
                StorageId = 1,
                StorageIsCentral = false,
                StorageName = "name",

            };
            dbSetInventory.Add(inventory);
            var Created = _dbContext(GetCurrentMethod()).SaveChangesAsync();

            TransferFacade facade = new TransferFacade(GetServiceProvider().Object, _dbContext(GetCurrentMethod()));
            var model = await dataUtil(facade, GetCurrentMethod()).GetNewData();
            model.DestinationId = inventory.StorageId;
            model.DestinationName = inventory.StorageName;
            model.DestinationCode = inventory.StorageCode;
            foreach(var item in model.Items)
            {
                item.ItemId = inventory.ItemId;
                
            }
            var Response = await facade.Create(model, USERNAME);
            Assert.NotEqual(0, Response);
        }
        [Fact]
        public async Task Should_Success_Get_All_Data()
        {
            TransferFacade facade = new TransferFacade(GetServiceProvider().Object, _dbContext(GetCurrentMethod()));
            var model = await dataUtil(facade, GetCurrentMethod()).GetTestData();
            var Response = facade.Read();
            Assert.NotEmpty(Response.Item1);
        }
        [Fact]
        public async Task Should_Success_Get_Data_By_Id()
        {
            TransferFacade facade = new TransferFacade(GetServiceProvider().Object, _dbContext(GetCurrentMethod()));
            var model = await dataUtil(facade, GetCurrentMethod()).GetTestData();
            var Response = facade.ReadById((int)model.Id);
            Assert.NotNull(Response);
        }
    }
}
