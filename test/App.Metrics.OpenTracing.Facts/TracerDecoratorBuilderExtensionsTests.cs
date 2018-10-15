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
            var options = OpenTracingAppMetricsDecoratorOptions.Default;

            var tracer = new TracerDecoratorBuilder(new MockTracer())
                .WithAppMetrics(_metrics, options)
                .Build();

            using (tracer.BuildSpan("Operation").StartActive())
            {
                var snap = _metrics.Snapshot.Get();
                var counter = snap.Contexts.Single().Counters.Single();
                counter.Name.Should().Be(options.SpansCounterName);
                counter.Value.Count.Should().Be(1);
                counter.Value.Items.Single().Item.Should().Equals("Operation");
                counter.Value.Items.Single().Count.Should().Equals(1);
                counter.Value.Items.Single().Percent.Should().Equals(100D);
            }

            using (tracer.BuildSpan("ParentSpan").StartActive())
            using (tracer.BuildSpan("Childspan").StartActive())
            {
                var snap = _metrics.Snapshot.Get();
                var counter = snap.Contexts.Single().Counters.Single();
                counter.Name.Should().Be(options.SpansCounterName);
                counter.Value.Count.Should().Be(2);
                counter.Value.Items.First().Item.Should().Equals("ParentSpan");
                counter.Value.Items.First().Count.Should().Equals(1);
                counter.Value.Items.First().Percent.Should().Equals(50D);
                counter.Value.Items.Last().Item.Should().Equals("Childspan");
                counter.Value.Items.Last().Count.Should().Equals(1);
                counter.Value.Items.Last().Percent.Should().Equals(50D);
            }

            {
                var snap = _metrics.Snapshot.Get();
                var counter = snap.Contexts.Single().Counters.Single();
                counter.Name.Should().Be(options.SpansCounterName);
                counter.Value.Count.Should().Be(0);
            }
        }
    }
}
