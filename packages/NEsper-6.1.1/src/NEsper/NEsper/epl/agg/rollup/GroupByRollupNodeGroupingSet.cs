///////////////////////////////////////////////////////////////////////////////////////
// Copyright (C) 2006-2017 Esper Team. All rights reserved.                           /
// http://esper.codehaus.org                                                          /
// ---------------------------------------------------------------------------------- /
// The software in this package is published under the terms of the GPL license       /
// a copy of which has been included with this distribution in the license.txt file.  /
///////////////////////////////////////////////////////////////////////////////////////

using System.Collections.Generic;

using com.espertech.esper.compat.collections;

namespace com.espertech.esper.epl.agg.rollup
{
    public class GroupByRollupNodeGroupingSet : GroupByRollupNodeBase
    {
        public override IList<int[]> Evaluate(GroupByRollupEvalContext context)
        {
            IList<int[]> rollup = new List<int[]>();
            foreach (GroupByRollupNodeBase node in this.ChildNodes)
            {
                IList<int[]> result = node.Evaluate(context);

                // find dups
                foreach (var row in result)
                {
                    foreach (var existing in rollup)
                    {
                        if (Collections.AreEqual(row, existing))
                        {
                            throw new GroupByRollupDuplicateException(row);
                        }
                    }
                }

                rollup.AddAll(result);
            }
            return rollup;
        }
    }
}