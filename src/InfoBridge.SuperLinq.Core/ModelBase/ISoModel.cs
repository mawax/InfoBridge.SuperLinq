﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfoBridge.SuperLinq.Core.ModelBase
{
    public interface ISoModel : IModel
    {
        int PK { get; set; }
    }
}
