﻿using KVSCommon.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KVSCommon.Entities
{
    /// <summary>
    ///     Interface for ContainerStar context
    /// </summary>
    public partial interface IKVSEntities : IEntities
    {
        /// <summary>
        /// BICs
        /// </summary>
        System.Data.Linq.Table<BIC_DE> BIC_DE { get; }

    }
}
