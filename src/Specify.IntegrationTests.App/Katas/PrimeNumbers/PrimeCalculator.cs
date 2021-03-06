﻿using System.Linq;
using Shouldly;

namespace Specify.IntegrationTests.App.Katas.PrimeNumbers
{
    public class PrimeCalculator
    {
        public bool IsPrime(int candidate)
        {
            return candidate != 1 &&
                !Enumerable.Range(2, candidate)
                .Any(i => candidate != i && candidate % i == 0);
        }
    }

    public abstract class PrimeCalculatorKata : NScenarioFor<PrimeCalculator>
    {
        protected bool Result;
        
    }

    public class calculating_prime_for_one : PrimeCalculatorKata
    {
        public void when_calculating_prime_for_one()
        {
            Result = SUT.IsPrime(1);
        }

        public void Then_should_return_false()
        {
            Result.ShouldBe(false);
        }
    }
}
