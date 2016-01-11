﻿// Description: EF Bulk Operations & Utilities | Bulk Insert, Update, Delete, Merge from database.
// Website & Documentation: https://github.com/zzzprojects/Entity-Framework-Plus
// Forum: https://github.com/zzzprojects/EntityFramework-Plus/issues
// License: http://www.zzzprojects.com/license-agreement/
// More projects: http://www.zzzprojects.com/
// Copyright (c) 2015 ZZZ Projects. All rights reserved.


#if EF5 || EF6
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Z.EntityFramework.Plus;
#if EF5 || EF6
using System.Data.Entity;

#elif EF7
using Microsoft.Data.Entity;

#endif

namespace Z.Test.EntityFramework.Plus
{
    public partial class Audit_EntityModified
    {
        [TestMethod]
        public void Inheritance_TPH()
        {
            TestContext.DeleteAll(x => x.AuditEntries);
            TestContext.DeleteAll(x => x.Inheritance_TPH_Animals);

            TestContext.Insert(x => x.Inheritance_TPH_Animals, Inheritance_TPH_Cat.Create, 1);
            TestContext.Insert(x => x.Inheritance_TPH_Animals, Inheritance_TPH_Dog.Create, 2);

            var audit = AuditHelper.AutoSaveAudit();

            using (var ctx = new TestContext())
            {
                ctx.Inheritance_TPH_Animals.ToList().ForEach(x =>
                {
                    x.ColumnInt++;

                    var cat = x as Inheritance_TPH_Cat;
                    var dog = x as Inheritance_TPH_Dog;

                    if (cat != null)
                    {
                        cat.ColumnCat++;
                    }
                    if (dog != null)
                    {
                        dog.ColumnDog++;
                    }
                });

                ctx.SaveChanges(audit);
            }

            // UnitTest - Audit
            {
                var entries = audit.Entries;

                // Entries
                {
                    // Entries Count
                    Assert.AreEqual(3, entries.Count);

                    // Entries State
                    Assert.AreEqual(AuditEntryState.EntityModified, entries[0].State);
                    Assert.AreEqual(AuditEntryState.EntityModified, entries[1].State);
                    Assert.AreEqual(AuditEntryState.EntityModified, entries[2].State);

                    // Entries EntitySetName
                    Assert.AreEqual(TestContext.TypeName(x => x.Inheritance_TPH_Animals), entries[0].EntitySetName);
                    Assert.AreEqual(TestContext.TypeName(x => x.Inheritance_TPH_Animals), entries[1].EntitySetName);
                    Assert.AreEqual(TestContext.TypeName(x => x.Inheritance_TPH_Animals), entries[2].EntitySetName);

                    // Entries TypeName
                    Assert.AreEqual(typeof (Inheritance_TPH_Cat).Name, entries[0].TypeName);
                    Assert.AreEqual(typeof (Inheritance_TPH_Dog).Name, entries[1].TypeName);
                    Assert.AreEqual(typeof (Inheritance_TPH_Dog).Name, entries[2].TypeName);
                }

                // Properties
                {
                    var propertyIndex = -1;

                    // Properties Count
                    Assert.AreEqual(2, entries[0].Properties.Count);
                    Assert.AreEqual(2, entries[1].Properties.Count);
                    Assert.AreEqual(2, entries[2].Properties.Count);

                    // ColumnInt
                    propertyIndex = 0;
                    Assert.AreEqual("ColumnInt", entries[0].Properties[propertyIndex].PropertyName);
                    Assert.AreEqual("ColumnInt", entries[1].Properties[propertyIndex].PropertyName);
                    Assert.AreEqual("ColumnInt", entries[2].Properties[propertyIndex].PropertyName);
                    Assert.AreEqual(0, entries[0].Properties[propertyIndex].OldValue);
                    Assert.AreEqual(0, entries[1].Properties[propertyIndex].OldValue);
                    Assert.AreEqual(1, entries[2].Properties[propertyIndex].OldValue);
                    Assert.AreEqual(1, entries[0].Properties[propertyIndex].NewValue);
                    Assert.AreEqual(1, entries[1].Properties[propertyIndex].NewValue);
                    Assert.AreEqual(2, entries[2].Properties[propertyIndex].NewValue);

                    // ColumnCat | ColumnDog
                    propertyIndex = 1;
                    Assert.AreEqual("ColumnCat", entries[0].Properties[propertyIndex].PropertyName);
                    Assert.AreEqual("ColumnDog", entries[1].Properties[propertyIndex].PropertyName);
                    Assert.AreEqual("ColumnDog", entries[2].Properties[propertyIndex].PropertyName);
                    Assert.AreEqual(0, entries[0].Properties[propertyIndex].OldValue);
                    Assert.AreEqual(0, entries[1].Properties[propertyIndex].OldValue);
                    Assert.AreEqual(1, entries[2].Properties[propertyIndex].OldValue);
                    Assert.AreEqual(1, entries[0].Properties[propertyIndex].NewValue);
                    Assert.AreEqual(1, entries[1].Properties[propertyIndex].NewValue);
                    Assert.AreEqual(2, entries[2].Properties[propertyIndex].NewValue);
                }
            }

            // UnitTest - Audit (Database)
            {
                using (var ctx = new TestContext())
                {
                    // ENSURE order
                    var entries = ctx.AuditEntries.OrderBy(x => x.AuditEntryID).Include(x => x.Properties).ToList();
                    entries.ForEach(x => x.Properties = x.Properties.OrderBy(y => y.AuditEntryPropertyID).ToList());

                    // Entries
                    {
                        // Entries Count
                        Assert.AreEqual(3, entries.Count);

                        // Entries State
                        Assert.AreEqual(AuditEntryState.EntityModified, entries[0].State);
                        Assert.AreEqual(AuditEntryState.EntityModified, entries[1].State);
                        Assert.AreEqual(AuditEntryState.EntityModified, entries[2].State);

                        // Entries EntitySetName
                        Assert.AreEqual(TestContext.TypeName(x => x.Inheritance_TPH_Animals), entries[0].EntitySetName);
                        Assert.AreEqual(TestContext.TypeName(x => x.Inheritance_TPH_Animals), entries[1].EntitySetName);
                        Assert.AreEqual(TestContext.TypeName(x => x.Inheritance_TPH_Animals), entries[2].EntitySetName);

                        // Entries TypeName
                        Assert.AreEqual(typeof(Inheritance_TPH_Cat).Name, entries[0].TypeName);
                        Assert.AreEqual(typeof(Inheritance_TPH_Dog).Name, entries[1].TypeName);
                        Assert.AreEqual(typeof(Inheritance_TPH_Dog).Name, entries[2].TypeName);
                    }

                    // Properties
                    {
                        var propertyIndex = -1;

                        // Properties Count
                        Assert.AreEqual(2, entries[0].Properties.Count);
                        Assert.AreEqual(2, entries[1].Properties.Count);
                        Assert.AreEqual(2, entries[2].Properties.Count);

                        // ColumnInt
                        propertyIndex = 0;
                        Assert.AreEqual("ColumnInt", entries[0].Properties[propertyIndex].PropertyName);
                        Assert.AreEqual("ColumnInt", entries[1].Properties[propertyIndex].PropertyName);
                        Assert.AreEqual("ColumnInt", entries[2].Properties[propertyIndex].PropertyName);
                        Assert.AreEqual("0", entries[0].Properties[propertyIndex].OldValue);
                        Assert.AreEqual("0", entries[1].Properties[propertyIndex].OldValue);
                        Assert.AreEqual("1", entries[2].Properties[propertyIndex].OldValue);
                        Assert.AreEqual("1", entries[0].Properties[propertyIndex].NewValue);
                        Assert.AreEqual("1", entries[1].Properties[propertyIndex].NewValue);
                        Assert.AreEqual("2", entries[2].Properties[propertyIndex].NewValue);

                        // ColumnCat | ColumnDog
                        propertyIndex = 1;
                        Assert.AreEqual("ColumnCat", entries[0].Properties[propertyIndex].PropertyName);
                        Assert.AreEqual("ColumnDog", entries[1].Properties[propertyIndex].PropertyName);
                        Assert.AreEqual("ColumnDog", entries[2].Properties[propertyIndex].PropertyName);
                        Assert.AreEqual("0", entries[0].Properties[propertyIndex].OldValue);
                        Assert.AreEqual("0", entries[1].Properties[propertyIndex].OldValue);
                        Assert.AreEqual("1", entries[2].Properties[propertyIndex].OldValue);
                        Assert.AreEqual("1", entries[0].Properties[propertyIndex].NewValue);
                        Assert.AreEqual("1", entries[1].Properties[propertyIndex].NewValue);
                        Assert.AreEqual("2", entries[2].Properties[propertyIndex].NewValue);
                    }
                }
            }
        }
    }
}
#endif