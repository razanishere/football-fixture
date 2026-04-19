//* this class is for mapping levels to the probability and the range of attempts

public class StrengthRule
{
    
    //* dynamic attempt range based on level

    public static int getMinAttempts(int level)
    {
        return level + 1;
    }

    public static int getMaxAttempts(int level)
    {
        return level + 4;
    }

    //* fixed score chances based on level

    public static double getGoalProbability(int level)
    {
        return level switch
        {
            1 => 0.15,
            2 => 0.18,
            3 => 0.21,
            4 => 0.24,
            5 => 0.27,
            6 => 0.30,
            7 => 0.33,
            8 => 0.36,
            9 => 0.40,
            _ => 0.27
        };
    }


}