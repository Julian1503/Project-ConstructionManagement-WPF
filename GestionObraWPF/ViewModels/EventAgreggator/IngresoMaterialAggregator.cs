﻿using GestionObraWPF.DTOs;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionObraWPF.ViewModels.EventAgreggator
{
    public class IngresoMaterialAggregator : PubSubEvent<IngresoMaterialDto>
    {
    }
}
