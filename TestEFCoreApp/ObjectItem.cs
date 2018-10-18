using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace TestEFCoreApp
{
    public class ObjectItem
    {
        public Guid ContainerId { get; set; }
        public Guid ObjectItemId { get; set; }

        public Guid ParentObjectId { get; set; }
        public Guid ChildObjectId{ get; set; }

        public Object ChildObject { get; set; }
        public Object ParentObject { get; set; }
        public Container Container { get; set; }
    }
}
