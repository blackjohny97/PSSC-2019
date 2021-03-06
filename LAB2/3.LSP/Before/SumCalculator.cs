using System.Linq;

namespace Before 
{
    public class SumCalculator 
    {
        protected readonly int[] _numbers;

        public SumCalculator (int[] numbers) 
        {
            _numbers = numbers;
        }

        public int Calculate () => _numbers.Sum ();
    }
}