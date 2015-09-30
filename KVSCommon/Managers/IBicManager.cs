﻿using KVSCommon.Database;
using KVSCommon.Managers.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KVSCommon.Managers
{
    public interface IBicManager: IEntityManager<BIC_DE, int>
    {
        BIC_DE GetBicByCodeAndName(string code, string name);
    }
}