﻿using KVSCommon.Database;
using KVSCommon.Entities;
using KVSCommon.Managers;
using KVSDataAccess.Managers.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KVSDataAccess.Managers
{
    public partial class ProductCategoryManager : EntityManager<ProductCategory, int>, IProductCategoryManager
    {
        public ProductCategoryManager(IKVSEntities context) : base(context) { }        
    }
}
