namespace TimeLinerOptimze.Core.Extensions
{
    public static class DoubleExtension
    {
        public static double Round(this double number , int digits)=>
            Math.Round(number, digits);
    }
}
