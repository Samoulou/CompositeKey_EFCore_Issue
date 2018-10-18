using System;
using System.Collections.Generic;
using System.Text;

namespace TestEFCoreApp
{
    public class Container
    {
        public Guid ContainerId { get; set; }

        public ICollection<Object> Objects { get; } = new HashSet<Object>();
        public ICollection<ObjectItem> ObjectItems { get; } = new HashSet<ObjectItem>();
    }
}
