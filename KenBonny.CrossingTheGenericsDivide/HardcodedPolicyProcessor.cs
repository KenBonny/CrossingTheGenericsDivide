using System;

namespace KenBonny.CrossingTheGenericsDivide
{
	internal static class HardcodedPolicyProcessor
	{
		public static void Process(IPolicy[] policies)
		{
			Console.WriteLine("Hardcoded processing");
			foreach (var policy in policies)
			{
				switch (policy)
				{
					case HomePolicy home:
						Console.WriteLine($"Hardcoded: {home.Location}");
						break;
					case LifePolicy life:
						Console.WriteLine($"Hardcoded: {life.Slogan}");
						break;
					case AutoPolicy auto:
						Console.WriteLine($"Hardcoded: {auto.ActivationText}");
						break;
					default:
						Console.WriteLine("Undefined policy");
						break;
				}
			}
		}
	}
}
