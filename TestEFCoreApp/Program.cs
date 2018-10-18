using System;

namespace TestEFCoreApp
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var db = new CoreContext())
            {
                db.Database.EnsureDeleted();
                db.Database.EnsureCreated();

                /*First case - work : We created everything separately */
                var rest = new Container();
                db.Container.Add(rest);
                db.SaveChanges();

                var objParent = new Object
                {
                    ObjectId = Guid.NewGuid(),
                    Name = "Parent Object"
                };
                rest.Objects.Add(objParent);
                db.SaveChanges();

                var objItems = new ObjectItem
                {
                    ChildObject = new Object
                    {
                        ContainerId= rest.ContainerId,
                        ObjectId = Guid.NewGuid(),
                        Name = "Child Object"
                    }
                };
                objParent.ChildrenObjectItems.Add(objItems);
                db.SaveChanges();

                /*Second case - work : We provide the ContainerId and it works*/
                var rest1 = new Container();
                db.Container.Add(rest1);
                db.SaveChanges();

                var objParent1 = new Object
                {
                    ContainerId = rest1.ContainerId,
                    ObjectId = Guid.NewGuid(),
                    Name = "Parent Object 1",
                    ChildrenObjectItems = 
                    {
                        new ObjectItem
                        {
                            ContainerId = rest1.ContainerId,
                            ChildObject = new Object
                            {
                                ContainerId = rest1.ContainerId,
                                ObjectId = Guid.NewGuid(),
                                Name = "Child Object 1"
                            }
                        }
                    }
                };
                rest1.Objects.Add(objParent1);
                db.SaveChanges();

                /*Third case don't work : We don't provide the containerId (which should be provided by EF)*/
                var rest2 = new Container();
                db.Container.Add(rest2);
                db.SaveChanges();

                var objParent2 = new Object
                {
                    ContainerId = rest2.ContainerId,
                    ObjectId = Guid.NewGuid(),
                    Name = "Parent Object 2",
                    ChildrenObjectItems =
                    {
                        new ObjectItem
                        {
                            ChildObject = new Object
                            {
                                ObjectId = Guid.NewGuid(),
                                Name = "Child Object 2"
                            }
                        }
                    }
                };
                rest2.Objects.Add(objParent2);
                db.SaveChanges();

                /*Fourth case - Don't work : We create everything in a row*/
                var rest3 = new Container
                {
                    Objects =
                    {
                        new Object
                        {
                            ObjectId = Guid.NewGuid(),
                            Name = "Parent Object 3",
                            ChildrenObjectItems = 
                            {
                                new ObjectItem
                                {
                                    ChildObject = new Object
                                    {
                                        ObjectId = Guid.NewGuid(),
                                        Name = "Child Object 3"
                                    }
                                }
                            }
                        }
                    }

                }; 
 
                db.Add(rest3);
                db.SaveChanges();
            }
        }
    }
}
