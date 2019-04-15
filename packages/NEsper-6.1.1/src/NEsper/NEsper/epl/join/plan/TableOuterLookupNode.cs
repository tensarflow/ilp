///////////////////////////////////////////////////////////////////////////////////////
// Copyright (C) 2006-2017 Esper Team. All rights reserved.                           /
// http://esper.codehaus.org                                                          /
// ---------------------------------------------------------------------------------- /
// The software in this package is published under the terms of the GPL license       /
// a copy of which has been included with this distribution in the license.txt file.  /
///////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;

using com.espertech.esper.client;
using com.espertech.esper.compat.collections;
using com.espertech.esper.compat.threading;
using com.espertech.esper.epl.join.exec.@base;
using com.espertech.esper.epl.join.table;
using com.espertech.esper.epl.virtualdw;
using com.espertech.esper.util;
using com.espertech.esper.view;

namespace com.espertech.esper.epl.join.plan
{
    /// <summary>
    /// Specifies exection of a table lookup with outer join using the a specified lookup plan.
    /// </summary>
    public class TableOuterLookupNode : QueryPlanNode
    {
        private readonly TableLookupPlan _tableLookupPlan;

        /// <summary>Ctor. </summary>
        /// <param name="tableLookupPlan">plan for performing lookup</param>
        public TableOuterLookupNode(TableLookupPlan tableLookupPlan)
        {
            _tableLookupPlan = tableLookupPlan;
        }

        /// <summary>Returns lookup plan. </summary>
        /// <value>lookup plan</value>
        public TableLookupPlan LookupStrategySpec
        {
            get { return _tableLookupPlan; }
        }

        public override void Print(IndentWriter writer)
        {
            writer.WriteLine("TableOuterLookupNode " +
                   " tableLookupPlan=" + _tableLookupPlan);
        }

        public override ExecNode MakeExec(string statementName, int statementId, Attribute[] annotations, IDictionary<TableLookupIndexReqKey, EventTable>[] indexesPerStream, EventType[] streamTypes, Viewable[] streamViews, HistoricalStreamIndexList[] historicalStreamIndexLists, VirtualDWView[] viewExternal, ILockable[] tableSecondaryIndexLocks)
        {
            JoinExecTableLookupStrategy lookupStrategy = _tableLookupPlan.MakeStrategy(statementName, statementId, annotations, indexesPerStream, streamTypes, viewExternal);
            int indexedStream = _tableLookupPlan.IndexedStream;
            if (tableSecondaryIndexLocks[indexedStream] != null)
            {
                return new TableOuterLookupExecNodeTableLocking(indexedStream, lookupStrategy, tableSecondaryIndexLocks[indexedStream]);
            }

            return new TableOuterLookupExecNode(_tableLookupPlan.IndexedStream, lookupStrategy);
        }

        public override void AddIndexes(ISet<TableLookupIndexReqKey> usedIndexes)
        {
            usedIndexes.AddAll(_tableLookupPlan.IndexNum);
        }
    }
}
