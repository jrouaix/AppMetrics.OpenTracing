// <copyright file="TracerDecoratorBuilderExtensions.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>
using App.Metrics.Counter;
using OpenTracing.Contrib.Decorators;
using System;

namespace App.Metrics.OpenTracing
{
    public static class TracerDecoratorBuilderExtensions
    {
        public static TracerDecoratorBuilder WithAppMetrics(this TracerDecoratorBuilder builder, IMetrics metrics, bool allEnabled = true, Action<OpenTracingAppMetricsDecoratorOptions> setupOptions = null)
            => WithAppMetrics(builder, metrics, allEnabled ? OpenTracingAppMetricsDecoratorOptions.AllEnabled : OpenTracingAppMetricsDecoratorOptions.AllDisabled, setupOptions ?? (opts => { }));

        public static TracerDecoratorBuilder WithAppMetrics(this TracerDecoratorBuilder builder, IMetrics metrics, OpenTracingAppMetricsDecoratorOptions options)
            => WithAppMetrics(builder, metrics, options, opts => { });

        private static TracerDecoratorBuilder WithAppMetrics(this TracerDecoratorBuilder builder, IMetrics metrics, OpenTracingAppMetricsDecoratorOptions options, Action<OpenTracingAppMetricsDecoratorOptions> setupOptions)
        {
            options = options ?? throw new ArgumentNullException(nameof(options));
            setupOptions?.Invoke(options);

            if (options.SpansCounters)
            {
                var spansCounter = new CounterOptions
                {
                    Name = options.SpansCounterName
                };


                builder.OnSpanStarted((span, operationName) =>
                {
                    metrics.Measure.Counter.Increment(spansCounter, operationName);

                    if (options.DistinctOperationsCounters)
                    {
                        var distinctCounter = new CounterOptions { Name = options.DistinctOperationCounterName + operationName };
                        metrics.Measure.Counter.Increment(distinctCounter);
                    }
                });

                builder.OnSpanFinished((span, operationName) =>
                {
                    metrics.Measure.Counter.Decrement(spansCounter, operationName);

                    if (options.DistinctOperationsCounters)
                    {
                        var distinctCounter = new CounterOptions { Name = options.DistinctOperationCounterName + operationName };
                        metrics.Measure.Counter.Decrement(distinctCounter);
                    }
                });
            }

            return builder;
        }
    }
}
