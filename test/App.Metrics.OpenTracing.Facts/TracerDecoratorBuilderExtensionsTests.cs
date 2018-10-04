using FluentAssertions;
using OpenTracing.Contrib.Decorators;
using OpenTracing.Mock;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace App.Metrics.OpenTracing.Facts
{
    public class SomeTests
    {
        private readonly IMetricsRoot _metrics;

        public SomeTests()
        {
            var builder = new MetricsBuilder()
                .Configuration.Configure(
                    options =>
                    {
                        options.DefaultContextLabel = "Test";
                        options.Enabled = true;
                    })
                .OutputEnvInfo.AsPlainText()
                .OutputMetrics.AsPlainText()
                .SampleWith.AlgorithmR(1028)
                .TimeWith.Clock<TestClock>();

            _metrics = builder.Build();
        }

        [Fact]
        public void ShouldCounterSpan()
        {
            var tracer = new TracerDecoratorBuilder(new MockTracer())
                .WithAppMetrics(_metrics)
                .Build();

            using (tracer.BuildSpan("Operation").StartActive())
            {
                var snap = _metrics.Snapshot.Get();
                var counter = snap.Contexts.Single().Counters.Single();
                counter.Name.Should().Be("Operation");
                counter.Value.Count.Should().Be(1);
            }
            {
                var snap = _metrics.Snapshot.Get();
                var counter = snap.Contexts.Single().Counters.Single();
                counter.Name.Should().Be("Operation");
                counter.Value.Count.Should().Be(0);
            }
        }
    }
}
