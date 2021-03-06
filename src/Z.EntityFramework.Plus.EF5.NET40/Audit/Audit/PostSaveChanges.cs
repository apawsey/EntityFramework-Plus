﻿// Description: Entity Framework Bulk Operations & Utilities (EF Bulk SaveChanges, Insert, Update, Delete, Merge | LINQ Query Cache, Deferred, Filter, IncludeFilter, IncludeOptimize | Audit)
// Website & Documentation: https://github.com/zzzprojects/Entity-Framework-Plus
// Forum & Issues: https://github.com/zzzprojects/EntityFramework-Plus/issues
// License: https://github.com/zzzprojects/EntityFramework-Plus/blob/master/LICENSE
// More projects: http://www.zzzprojects.com/
// Copyright © ZZZ Projects Inc. 2014 - 2016. All rights reserved.

using System.Linq;
#if EF5
using System.Data;
using System.Data.Objects;

#elif EF6
using System.Data.Entity.Core;
using System.Data.Entity.Core.Objects;

#elif EF7
using Microsoft.Data.Entity.ChangeTracking;

#endif

namespace Z.EntityFramework.Plus
{
    public partial class Audit
    {
        /// <summary>Updates audit entries after the save changes has been executed.</summary>
        /// <param name="audit">The audit to use to add changes made to the context.</param>
        public static void PostSaveChanges(Audit audit)
        {
            foreach (var entry in audit.Entries)
            {
                if (entry.DelayedKey != null)
                {
#if EF5 || EF6
                    var objectStateEntry = entry.DelayedKey as ObjectStateEntry;
#elif EF7
                    var objectStateEntry = entry.DelayedKey as EntityEntry;
#endif
                    if (objectStateEntry != null)
                    {
#if EF5 || EF6
                        if (objectStateEntry.IsRelationship)
                        {
                            var values = objectStateEntry.CurrentValues;
                            var leftKeys = (EntityKey) values.GetValue(0);
                            var rightKeys = (EntityKey) values.GetValue(1);
                            var leftRelationName = values.GetName(0);
                            var rightRelationName = values.GetName(1);

                            foreach (var keyValue in leftKeys.EntityKeyValues)
                            {
                                entry.Properties.Add(new AuditEntryProperty(entry, leftRelationName, keyValue.Key, null, keyValue.Value));
                            }

                            foreach (var keyValue in rightKeys.EntityKeyValues)
                            {
                                entry.Properties.Add(new AuditEntryProperty(entry, rightRelationName, keyValue.Key, null, keyValue.Value));
                            }
                        }
                        else
                        {
                            foreach (var keyValue in objectStateEntry.EntityKey.EntityKeyValues)
                            {
                                var property = entry.Properties.FirstOrDefault(x => x.PropertyName == keyValue.Key);

                                // ENSURE the property is audited
                                if (property != null)
                                {
                                    property.NewValue = keyValue.Value;
                                }
                            }
                        }
                    }
#elif EF7
                        foreach (var keyValue in objectStateEntry.Metadata.GetKeys())
                        {
                            var key = objectStateEntry.Property(keyValue.Properties[0].Name);
                            var property = entry.Properties.FirstOrDefault(x => x.PropertyName == keyValue.Properties[0].Name);
                           
                            // ENSURE the property is audited
                            if (property != null)
                            {
                                property.NewValue = key.CurrentValue;
                            }
                        }
                    }
#endif
                }
            }
        }
    }
}