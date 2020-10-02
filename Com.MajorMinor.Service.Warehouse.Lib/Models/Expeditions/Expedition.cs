﻿using Com.MM.Service.Warehouse.Lib.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Com.MM.Service.Warehouse.Lib.Models.Expeditions
{
    public class Expedition : BaseModel
    {
        [MaxLength(255)]
        public string Code { get; set; }
        public DateTimeOffset Date { get; set; }
        public int ExpeditionServiceId { get; set; }
        [MaxLength(255)]
        public string ExpeditionServiceCode { get; set; }
        [MaxLength(255)]
        public string ExpeditionServiceName { get; set; }
        [MaxLength(1000)]
        public string Remark { get; set; }
        public int Weight { get; set; }
        public virtual ICollection<ExpeditionItem> Items { get; set; }

    }
}
