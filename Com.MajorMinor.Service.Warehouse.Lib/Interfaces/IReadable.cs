﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Com.MM.Service.Warehouse.Lib.Interfaces
{
    public interface IReadable
    {
        /* List of Data, Total Data, Order Dictionary */
        Tuple<List<object>, int, Dictionary<string, string>> Read(int Page = 1, int Size = 25, string Order = "{}", string Keyword = null, string Filter = "{}");
    }
}
