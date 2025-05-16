namespace Common.Helpers
{
    public static class RandomGenerator
    {
        public static string GenerateRandomName(int length = 8)
        {
            var chars = "abcdefghijklmnopqrstuvwxyz";
            var rand = new Random();
            return new string(Enumerable.Range(0, length)
                .Select(_ => chars[rand.Next(chars.Length)]).ToArray());
        }
    }
}
