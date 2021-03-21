using System;
using System.Diagnostics;
using KenBonny.CrossingTheGenericsDivide.Composition;
using KenBonny.CrossingTheGenericsDivide.CompositionWithFactory;
using KenBonny.CrossingTheGenericsDivide.Inheritance;

namespace KenBonny.CrossingTheGenericsDivide
{
	internal class Program
	{
		private static void Main(string[] args)
		{
			Console.WriteLine("Let's cross this generic divide");

			var policies = new IPolicy[]
			{
				new LifePolicy(),
				new HomePolicy(),
				new AutoPolicy()
			};

			var stopwatch = Stopwatch.StartNew();
			HardcodedPolicyProcessor.Process(policies);
			stopwatch.Stop();
			Console.WriteLine($"Elapsed: {stopwatch.Elapsed:c} ({stopwatch.ElapsedTicks:0 000})");

			Console.WriteLine();
			stopwatch.Restart();
			InheritanceGenericProcessor.Process(policies);
			stopwatch.Stop();
			Console.WriteLine($"Elapsed: {stopwatch.Elapsed:c} ({stopwatch.ElapsedTicks:0 000})");

			Console.WriteLine();
			stopwatch.Restart();
			CompositionGenericProcessor.Process(policies);
			stopwatch.Stop();
			Console.WriteLine($"Elapsed: {stopwatch.Elapsed:c} ({stopwatch.ElapsedTicks:0 000})");

			Console.WriteLine();
			stopwatch.Restart();
			CompositionWithFactoryGenericProcessor.Process(policies);
			stopwatch.Stop();
			Console.WriteLine($"Elapsed: {stopwatch.Elapsed:c} ({stopwatch.ElapsedTicks:0 000})");
		}
	}

	internal interface IPolicy { }

	internal class HomePolicy : IPolicy
	{
		public string Location => "No place like home.";
	}

	internal class AutoPolicy : IPolicy
	{
		public string ActivationText => "Auto mode activated";
	}

	internal class LifePolicy : IPolicy
	{
		public string Slogan => "I want to live!";
	}
}
