﻿/* -------------------------------------------------------------------------------------------------
   Restricted - Copyright (C) Siemens Healthcare GmbH/Siemens Medical Solutions USA, Inc., 2019. All rights reserved
   ------------------------------------------------------------------------------------------------- */
   
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MockPractice
{
    public interface INowProvider
    {
        DateTime GetNow();
    }

    internal class NowProvider : INowProvider
    {
        public DateTime GetNow()
            => DateTime.Now;
    }
}
