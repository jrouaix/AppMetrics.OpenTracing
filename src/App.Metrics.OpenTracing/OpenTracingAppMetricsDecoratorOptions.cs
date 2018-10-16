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
                    SpansCountersEnabled = true,
                    DistinctOperationsCountersEnabled = true,
                    SpansMetersEnabled = true,
                    DistinctOperationsMetersEnabled = true,
                    SpansTimersEnabled = true,
                    DistinctOperationsTimersEnabled = true,
                };
            }
        }

        public static OpenTracingAppMetricsDecoratorOptions AllDisabled
        {
            get
            {
                return new OpenTracingAppMetricsDecoratorOptions
                {
                    SpansCountersEnabled = false,
                    DistinctOperationsCountersEnabled = false,
                    SpansMetersEnabled = false,
                    DistinctOperationsMetersEnabled = false,
                    SpansTimersEnabled = false,
                    DistinctOperationsTimersEnabled = false,
                };
            }
        }

        public bool SpansCountersEnabled { get; set; } = true;
        public string SpansCounterName { get; set; } = "OpenTracing current spans";
        public bool DistinctOperationsCountersEnabled { get; set; } = false;
        public string DistinctOperationCountersName { get; set; } = "OpenTracing current operation - ";


        public bool SpansMetersEnabled { get; set; } = true;
        public string SpansMeterName { get; set; } = "OpenTracing spans";
        public bool DistinctOperationsMetersEnabled { get; set; } = false;
        public string DistinctOperationMetersName { get; set; } = "OpenTracing operation - ";

        public bool SpansTimersEnabled { get; set; } = true;
        public string SpansTimerName { get; set; } = "OpenTracing spans";
        public bool DistinctOperationsTimersEnabled { get; set; } = false;
        public string DistinctOperationTimersName { get; set; } = "OpenTracing operation - ";

    }
}
