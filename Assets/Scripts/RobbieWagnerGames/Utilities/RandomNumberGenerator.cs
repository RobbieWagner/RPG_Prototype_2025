public class RandomNumberGenerator
{
    public int seed {get; private set;}
    public int rngPulls {get; private set;}
    private System.Random rng;

    public RandomNumberGenerator(int seed, int rngPulls)
    {
        this.seed = seed;
        this.rngPulls = rngPulls;

        RefreshRNG();
    }

    public void RefreshRNG()
    {
        rng = new System.Random(seed);
        
        for (int i = 0; i < rngPulls; i++)
        {
            rng.Next();
        }
    }

    public int Next(int min, int max)
    {
        return rng.Next(min, max);
    }

    public int Next(int max)
    {
        return rng.Next(max);
    }
}
