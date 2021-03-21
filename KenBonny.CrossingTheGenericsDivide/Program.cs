using System;
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

			HardcodedPolicyProcessor.Process(policies);
			Console.WriteLine();
			InheritanceGenericProcessor.Process(policies);
			Console.WriteLine();
			CompositionGenericProcessor.Process(policies);
			Console.WriteLine();
			CompositionWithFactoryGenericProcessor.Process(policies);
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
