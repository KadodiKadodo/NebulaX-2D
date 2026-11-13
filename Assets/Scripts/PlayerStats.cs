public static class PlayerStats
{
    // 0 = Basic, 1 = Level 1, 2 = Level 2
    public static int engineLevel = 0; 
    public static int oxygenLevel = 0; 
    
    public static float GetOxygenDrainRate()
    {
        if (engineLevel == 0) return 1f;  // 100 oxygen / 5 = 20 seconds
        if (engineLevel == 1) return 2f;  // Heavier engine! 150 oxygen / 8 = 18 seconds
        return 3f;                       // Super heavy engine! 200 oxygen / 10 = 20 seconds
    }

    public static float GetMaxOxygen()
    {
        if (oxygenLevel == 0) return 100f;
        if (oxygenLevel == 1) return 150f;
        return 200f;
    }
}