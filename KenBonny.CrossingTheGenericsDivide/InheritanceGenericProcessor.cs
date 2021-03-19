using System;
using Microsoft.Extensions.DependencyInjection;

namespace KenBonny.CrossingTheGenericsDivide.Inheritance
{
	internal static class InheritanceGenericProcessor
	{
		public static void Process(IPolicy[] policies)
		{
			Console.WriteLine("Inheritance generic processing");
			var container = new ServiceCollection()
			                .AddTransient<IPolicyProcessor<HomePolicy>>(_ => new HomePolicyProcessor())
			                .AddTransient<IPolicyProcessor<LifePolicy>>(_ => new LifePolicyProcessor())
			                .AddTransient<IPolicyProcessor<AutoPolicy>>(_ => new AutoPolicyProcessor())
			                .BuildServiceProvider();

			foreach (var policy in policies)
			{
				var policyType = policy.GetType();
				var processorType = typeof(IPolicyProcessor<>).MakeGenericType(policyType);
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

	internal abstract class PolicyProcessorBase<TPolicy> : IPolicyProcessor, IPolicyProcessor<TPolicy>
		where TPolicy : IPolicy
	{
		public string Process(IPolicy policy) => Process((TPolicy) policy);

		public abstract string Process(TPolicy policy);
	}

	internal class LifePolicyProcessor : PolicyProcessorBase<LifePolicy>
	{
		public override string Process(LifePolicy policy) => $"{GetType().FullName}: {policy.Slogan}";
	}

	internal class AutoPolicyProcessor : PolicyProcessorBase<AutoPolicy>
	{
		public override string Process(AutoPolicy policy) => $"{GetType().FullName}: {policy.ActivationText}";
	}

	internal class HomePolicyProcessor : PolicyProcessorBase<HomePolicy>
	{
		public override string Process(HomePolicy policy) => $"{GetType().FullName}: {policy.Location}";
	}
}
