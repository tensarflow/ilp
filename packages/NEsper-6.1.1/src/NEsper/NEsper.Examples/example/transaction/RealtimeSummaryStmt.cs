///////////////////////////////////////////////////////////////////////////////////////
// Copyright (C) 2006-2017 Esper Team. All rights reserved.                           /
// http://esper.codehaus.org                                                          /
// ---------------------------------------------------------------------------------- /
// The software in this package is published under the terms of the GPL license       /
// a copy of which has been included with this distribution in the license.txt file.  /
///////////////////////////////////////////////////////////////////////////////////////


using System;
using com.espertech.esper.client;

namespace NEsper.Examples.Transaction
{
    public class RealtimeSummaryStmt
    {
        public EPStatement TotalsStatement { get; private set; }
        public EPStatement CustomerStatement { get; private set; }
        public EPStatement SupplierStatement { get; private set; }

        public RealtimeSummaryStmt(EPAdministrator admin)
        {
            //
            // Min,Max,Average total latency from the events (difference in time between A and C) over the past 30 minutes.
            // Min,Max,Average latency between events A/B (time stamp of B minus A) and B/C (time stamp of C minus B).
            //
            String stmtTotal = "select min(latencyAC) as minLatencyAC, " +
                                      "max(latencyAC) as maxLatencyAC, " +
                                      "avg(latencyAC) as avgLatencyAC, " +
                                      "min(latencyAB) as minLatencyAB, " +
                                      "max(latencyAB) as maxLatencyAB, " +
                                      "avg(latencyAB) as avgLatencyAB, " +
                                      "min(latencyBC) as minLatencyBC, " +
                                      "max(latencyBC) as maxLatencyBC, " +
                                      "avg(latencyBC) as avgLatencyBC " +
                               "from CombinedEvent.win:time(30 min)";

            TotalsStatement = admin.CreateEPL(stmtTotal);

            //
            // Min,Max,Average latency grouped by (a) customer ID and (b) supplier ID.
            // In other words, metrics on the the latency of the orders coming from each customer and going to each supplier.
            //
            String stmtCustomer = "select customerId," +
                                         "min(latencyAC) as minLatency," +
                                         "max(latencyAC) as maxLatency," +
                                         "avg(latencyAC) as avgLatency " +
                                  "from CombinedEvent.win:time(30 min) " +
                                  "group by customerId";

            CustomerStatement = admin.CreateEPL(stmtCustomer);

            String stmtSupplier = "select supplierId," +
                                         "min(latencyAC) as minLatency," +
                                         "max(latencyAC) as maxLatency," +
                                         "avg(latencyAC) as avgLatency " +
                                  "from CombinedEvent.win:time(30 min) " +
                                  "group by supplierId";

            SupplierStatement = admin.CreateEPL(stmtSupplier);
        }
    }
}
