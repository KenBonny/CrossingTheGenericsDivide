using System;
using System.ComponentModel.Design;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace KenBonny.CrossingTheGenericsDivide.CompositionWithFactory
{
	internal static class CompositionWithFactoryGenericProcessor
	{
		public static void Process(IPolicy[] policies)
		{
			Console.WriteLine("Composition with factory generic processor:");
			var host = Host.CreateDefaultBuilder()
			               .ConfigureServices((_, services) => services.AddTransient<PolicyProcessorFactory>()
			                                                           .AddTransient(typeof(PolicyProcessor<>))
			                                                           .AddTransient<IPolicyProcessor<HomePolicy>>(_ => new HomePolicyProcessor())
			                                                           .AddTransient<IPolicyProcessor<LifePolicy>>(_ => new LifePolicyProcessor())
			                                                           .AddTransient<IPolicyProcessor<AutoPolicy>>(_ => new AutoPolicyProcessor()))
			               .Build();

			var factory = host.Services.GetService<PolicyProcessorFactory>();
			foreach (var policy in policies)
			{
				var policyProcessor = factory.Create(policy);
				var result = policyProcessor.Process(policy);
				Console.WriteLine(result);
			}
		}
	}

	internal class PolicyProcessorFactory
	{
		private readonly IServiceProvider _container;

		public PolicyProcessorFactory(IServiceProvider container) => _container = container;

		public IPolicyProcessor Create(IPolicy policy)
		{
			var policyType = policy.GetType();
			var processorType = typeof(PolicyProcessor<>).MakeGenericType(policyType);
			return (IPolicyProcessor) _container.GetService(processorType);
		}
	}

	internal interface IPolicyProcessor
	{
		string Process(IPolicy policy);
	}

	internal interface IPolicyProcessor<in TPolicy>
		where TPolicy : IPolicy
	{
		string Process(TPolicy policy);
	}

	internal class PolicyProcessor<TPolicy> : IPolicyProcessor
		where TPolicy : IPolicy
	{
		private readonly IPolicyProcessor<TPolicy> _policyProcessor;

		public PolicyProcessor(IPolicyProcessor<TPolicy> policyProcessor)
		{
			_policyProcessor = policyProcessor;
		}

		public string Process(IPolicy policy) => $"{_policyProcessor.GetType().FullName}: {_policyProcessor.Process((TPolicy) policy)}";
	}

	internal class LifePolicyProcessor : IPolicyProcessor<LifePolicy>
	{
		public string Process(LifePolicy policy) => policy.Slogan;
	}

	internal class AutoPolicyProcessor : IPolicyProcessor<AutoPolicy>
	{
		public string Process(AutoPolicy policy) => policy.ActivationText;
	}

	internal class HomePolicyProcessor : IPolicyProcessor<HomePolicy>
	{
		public string Process(HomePolicy policy) => policy.Location;
	}
}
