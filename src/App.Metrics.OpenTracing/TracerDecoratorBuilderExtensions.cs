// <copyright file="TracerDecoratorBuilderExtensions.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>
using App.Metrics.Counter;
using OpenTracing.Contrib.Decorators;

namespace App.Metrics.OpenTracing
{
    public static class TracerDecoratorBuilderExtensions
    {
        public static TracerDecoratorBuilder  WithAppMetrics(this TracerDecoratorBuilder builder, IMetrics metrics)
        {
            builder.OnSpanStarted((span, operationName) =>
            {
                var counter = new CounterOptions { Name = operationName };
                metrics.Measure.Counter.Increment(counter);
            });

            builder.OnSpanFinished((span, operationName) =>
            {
                var counter = new CounterOptions { Name = operationName };
                metrics.Measure.Counter.Decrement(counter);
            });
            
            return builder; 
        }
    }
}
