﻿// Description: EF Bulk Operations & Utilities | Bulk Insert, Update, Delete, Merge from database.
// Website & Documentation: https://github.com/zzzprojects/Entity-Framework-Plus
// Forum: https://github.com/zzzprojects/EntityFramework-Plus/issues
// License: http://www.zzzprojects.com/license-agreement/
// More projects: http://www.zzzprojects.com/
// Copyright (c) 2015 ZZZ Projects. All rights reserved.

using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Z.EntityFramework.Plus;

namespace Z.Test.EntityFramework.Plus
{
    public partial class QueryFilter_DbSet_AsNoFilter
    {
        [TestMethod]
        public void WithGlobalFilter_WithInstanceFilter_SingleFilter_GlobalFilterEnabled_InstanceFilterDisabled()
        {
            using (var ctx = new TestContext(false, enableFilter1: true))
            {
                ctx.Filter<Inheritance_Interface_Entity>(QueryFilterHelper.Filter.Filter5, entities => entities.Where(x => x.ColumnInt != 5));
                ctx.Filter(QueryFilterHelper.Filter.Filter5).Disable();

                Assert.AreEqual(45, ctx.Inheritance_Interface_Entities.AsNoFilter().Sum(x => x.ColumnInt));
            }
        }
    }
}