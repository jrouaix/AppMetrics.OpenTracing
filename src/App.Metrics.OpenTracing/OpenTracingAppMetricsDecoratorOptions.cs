using System;
using System.Collections.Generic;
using System.Text;

namespace App.Metrics.OpenTracing
{
    public class OpenTracingAppMetricsDecoratorOptions
    {
        public static OpenTracingAppMetricsDecoratorOptions Default { get => new OpenTracingAppMetricsDecoratorOptions(); }

        public static OpenTracingAppMetricsDecoratorOptions AllEnabled
        {
            get
            {
                return new OpenTracingAppMetricsDecoratorOptions
                {

                };
            }
        }

        public static OpenTracingAppMetricsDecoratorOptions AllDisabled
        {
            get
            {
                return new OpenTracingAppMetricsDecoratorOptions
                {

                };
            }
        }

        public bool SpansCounters { get; set; } = true;
        public string SpansCounterName { get; set; } = "OpenTracing current spans";
        public bool DistinctOperationsCounters { get; set; } = false;
        public string DistinctOperationCounterName { get; set; } = "OpenTracing current operation - ";



    }
}
