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
    public partial class QueryFilter_DbSet_Filter
    {
        [TestMethod]
        public void WithInstanceFilter_ManyFilter_Enabled()
        {
            using (var ctx = new TestContext())
            {
                ctx.Filter<Inheritance_Interface_Entity>(QueryFilterHelper.Filter.Filter1, entities => entities.Where(x => x.ColumnInt != 1), false);
                ctx.Filter<Inheritance_Interface_IEntity>(QueryFilterHelper.Filter.Filter2, entities => entities.Where(x => x.ColumnInt != 2), false);
                ctx.Filter<Inheritance_Interface_Base>(QueryFilterHelper.Filter.Filter3, entities => entities.Where(x => x.ColumnInt != 3), false);
                ctx.Filter<Inheritance_Interface_IBase>(QueryFilterHelper.Filter.Filter4, entities => entities.Where(x => x.ColumnInt != 4), false);

                ctx.Filter(QueryFilterHelper.Filter.Filter1).Enable();
                ctx.Filter(QueryFilterHelper.Filter.Filter2).Enable();
                ctx.Filter(QueryFilterHelper.Filter.Filter3).Enable();
                ctx.Filter(QueryFilterHelper.Filter.Filter4).Enable();

                Assert.AreEqual(35, ctx.Inheritance_Interface_Entities.Filter(
                    QueryFilterHelper.Filter.Filter1,
                    QueryFilterHelper.Filter.Filter2,
                    QueryFilterHelper.Filter.Filter3,
                    QueryFilterHelper.Filter.Filter4).Sum(x => x.ColumnInt));
            }
        }
    }
}