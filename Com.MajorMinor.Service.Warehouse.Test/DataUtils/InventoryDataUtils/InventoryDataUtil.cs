﻿using Com.MM.Service.Warehouse.Lib;
using Com.MM.Service.Warehouse.Lib.Facades;
using Com.MM.Service.Warehouse.Lib.Models.InventoryModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Com.MM.Service.Warehouse.Test.DataUtils.InventoryDataUtils
{
    public class InventoryDataUtil
    {
        private readonly WarehouseDbContext dbContext;
        private readonly InventoryFacade inventoryFacade;
        private readonly DbSet<Inventory> dbSetInventory;
        public InventoryDataUtil(InventoryFacade facade, WarehouseDbContext dbContext)
        {
            this.inventoryFacade = facade;
            this.dbContext = dbContext;
            this.dbSetInventory = dbContext.Set<Inventory>();
            //this.garmentPurchaseOrderDataUtil = garmentPurchaseOrderDataUtil;
        }
        public Inventory GetNewData()
        {
            //var datas = await Task.Run(() => garmentPurchaseOrderDataUtil.GetTestDataByTags());
            return new Inventory
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
        }
        public Inventory GetNewData_Transfer()
        {
            //var datas = await Task.Run(() => garmentPurchaseOrderDataUtil.GetTestDataByTags());
            return new Inventory
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
                StorageCode = "GTM.01",
                StorageId = 8,
                StorageIsCentral = true,
                StorageName = "GUDANG TRANSFER STOCK MAJOR MINOR",

            };
        }
        public async Task<Inventory> GetTestData()
        {
            var data = GetNewData();
            dbSetInventory.Add(data);
            await dbContext.SaveChangesAsync();
            return data;
        }

        public async Task<Inventory> GetTestDataForTransfer()
        {
            var data = GetNewData_Transfer();
            dbSetInventory.Add(data);
            await dbContext.SaveChangesAsync();
            return data;
        }
    }
}
