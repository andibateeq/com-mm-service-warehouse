﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Com.MM.Service.Warehouse.Lib.Interfaces
{
    public interface IDeleteable
    {
        Task<int> Delete(int id);
    }
}
