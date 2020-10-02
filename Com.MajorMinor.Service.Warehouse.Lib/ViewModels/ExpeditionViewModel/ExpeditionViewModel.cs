﻿using Com.MM.Service.Warehouse.Lib.Utilities;
using Com.MM.Service.Warehouse.Lib.ViewModels.NewIntegrationViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Linq;

namespace Com.MM.Service.Warehouse.Lib.ViewModels.ExpeditionViewModel
{
    public class ExpeditionViewModel : BaseViewModel
    {
        public string code { get; set; }
        public DateTimeOffset date { get; set; }
        public ExpeditionServiceViewModel expeditionService { get; set; }
        public string remark { get; set; }
        public int weight { get; set; }
        public List<ExpeditionItemViewModel> items { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (this.expeditionService == null)
            {
                yield return new ValidationResult("expedition is required", new List<string> { "source" });
            }
            //if (this.destination == null)
            //{
            //    yield return new ValidationResult("Destination is required", new List<string> { "destination" });
            //}
            //if (this.reference == null || this.reference == "")
            //{
            //    yield return new ValidationResult("Reference is required", new List<string> { "reference" });
            //}

            int itemErrorCount = 0;
            int detailErrorCount = 0;
            if (this.items == null || this.items.Count == 0)
            {
                yield return new ValidationResult("items is required", new List<string> { "itemscount" });
            }
            else
            {
                string itemError = "[";
                foreach (var item in items)
                {
                    itemError += "{";

                    if (item.spkDocsViewModel.Weight == 0)
                    {
                        itemErrorCount++;
                        itemError += "Weight: 'wight must be > 0', ";
                    }
                    if (item.details == null || item.details.Count.Equals(0))
                    {
                        itemErrorCount++;
                        itemError += "detailscount: 'Details is required', ";
                    }
                    else
                    {
                        string detailError = "[";

                        foreach (var detail in item.details)
                        {
                            detailError += "{";

                            if (detail.sendQuantity == 0)
                            {
                                detailErrorCount++;
                                detailError += "sendQuantity: 'sendQuantity can not 0', ";
                            }
                            WarehouseDbContext warehouseDbContext = (WarehouseDbContext)validationContext.GetService(typeof(WarehouseDbContext));
                            if (detail.sendQuantity > warehouseDbContext.Inventories.Where(x => x.ItemArticleRealizationOrder == detail.item.articleRealizationOrder && x.ItemCode == detail.item.code && x.ItemName == detail.item.name && x.StorageId == item.spkDocsViewModel.source._id).FirstOrDefault().Quantity)
                            {
                                detailErrorCount++;
                                detailError += "sendQuantity: 'sendQuantity can not more than inventory quantity', ";
                            }

                            if (detail.sendQuantity > detail.quantity)
                            {
                                detailErrorCount++;
                                detailError += "sendQuantity: 'sendQuantity can not more than packingList quantity', ";
                            }
                            
                            if (detail.remark == null || detail.remark == "")
                            {
                                detailErrorCount++;
                                detailError += "remark: 'remark is required', ";
                            }
                            detailError += "}, ";
                        }

                        detailError += "]";

                        if (detailErrorCount > 0)
                        {
                            itemErrorCount++;
                            itemError += $"details: {detailError}, ";
                        }
                    }


                    itemError += "}, ";
                }

                itemError += "]";

                if (itemErrorCount > 0)
                    yield return new ValidationResult(itemError, new List<string> { "items" });
            }
        }
    }
}
