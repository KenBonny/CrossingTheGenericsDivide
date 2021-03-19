using System;
using Microsoft.Extensions.DependencyInjection;

namespace KenBonny.CrossingTheGenericsDivide.Composition
{
	internal static class CompositionGenericProcessor
	{
		public static void Process(IPolicy[] policies)
		{
			Console.WriteLine("Composition generic processor:");
			var container = new ServiceCollection()
			                .AddTransient(typeof(PolicyProcessor<>))
			                .AddTransient<IPolicyProcessor<HomePolicy>>(_ => new HomePolicyProcessor())
			                .AddTransient<IPolicyProcessor<LifePolicy>>(_ => new LifePolicyProcessor())
			                .AddTransient<IPolicyProcessor<AutoPolicy>>(_ => new AutoPolicyProcessor())
			                .BuildServiceProvider();

			foreach (var policy in policies)
			{
				var policyType = policy.GetType();
				var processorType = typeof(PolicyProcessor<>).MakeGenericType(policyType);
				var policyProcessor = (IPolicyProcessor) container.GetService(processorType);
				var result = policyProcessor.Process(policy);
				Console.WriteLine(result);
			}
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
