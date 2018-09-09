using OnlineStore.Core.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineStore.Core.Abstractions
{
    public class EntityBaseWithId: EntityBase
    {
        public long Id { get; set; }
    }
}
