﻿// Description: EF Bulk Operations & Utilities | Bulk Insert, Update, Delete, Merge from database.
// Website & Documentation: https://github.com/zzzprojects/Entity-Framework-Plus
// Forum: https://github.com/zzzprojects/EntityFramework-Plus/issues
// License: http://www.zzzprojects.com/license-agreement/
// More projects: http://www.zzzprojects.com/
// Copyright (c) 2015 ZZZ Projects. All rights reserved.

using System;

namespace Z.EntityFramework.Plus
{
    public static partial class QueryDelayedExtensions
    {
        public static class Error
        {
            internal static Exception ArgumentNull(string paramName)
            {
                return new ArgumentNullException(paramName);
            }

            internal static Exception ArgumentOutOfRange(string paramName)
            {
                return new ArgumentOutOfRangeException(paramName);
            }
        }
    }
}