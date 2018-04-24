﻿using System.Linq;
using System.Reflection;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Elastic.Xunit.Sdk
{
	public class ElasticTestDiscoverer : XunitTestFrameworkDiscoverer
	{
		public ElasticTestDiscoverer(IAssemblyInfo assemblyInfo, ISourceInformationProvider sourceProvider, IMessageSink diagnosticMessageSink, IXunitTestCollectionFactory collectionFactory = null) : base(assemblyInfo, sourceProvider, diagnosticMessageSink, collectionFactory)
		{
			var a = Assembly.Load(new AssemblyName(assemblyInfo.Name));
			var options = a.GetCustomAttributes<ElasticXunitConfigurationAttribute>().FirstOrDefault()?.Options ?? new ElasticXunitRunOptions();
			this.Options = options;
		}

		public ElasticXunitRunOptions Options { get; }

		protected override bool FindTestsForType(ITestClass testClass, bool includeSourceInformation, IMessageBus messageBus, ITestFrameworkDiscoveryOptions discoveryOptions)
		{
			discoveryOptions.SetValue(nameof(ElasticXunitRunOptions.RunIntegrationTests), this.Options.RunIntegrationTests);
			discoveryOptions.SetValue(nameof(ElasticXunitRunOptions.RunUnitTests), this.Options.RunUnitTests);
			discoveryOptions.SetValue(nameof(ElasticXunitRunOptions.TestFilter), this.Options.TestFilter);
			discoveryOptions.SetValue(nameof(ElasticXunitRunOptions.ClusterFilter), this.Options.ClusterFilter);
			return base.FindTestsForType(testClass, includeSourceInformation, messageBus, discoveryOptions);
		}
	}
}