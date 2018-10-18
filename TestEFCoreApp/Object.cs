using System;
using System.Collections.Generic;
using System.Text;

namespace TestEFCoreApp
{
    public class Object
    {
        public Guid ContainerId { get; set; }
        public Guid ObjectId { get; set; }

        public string Name { get; set; }
        
        public Container Container { get; set; }
        public ICollection<ObjectItem> ParentObjectItems { get; } = new HashSet<ObjectItem>();
        public ICollection<ObjectItem> ChildrenObjectItems { get; set; } = new HashSet<ObjectItem>();

    }
}
