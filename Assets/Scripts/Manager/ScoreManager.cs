using System.Collections;
using Unity.VisualScripting;
using UnityEngine;


public static class ScoreManager
{
    public static int CalcLevel(int score)
    {
        return (score / SettingsManager.Instance.LEVEL_SCORE_DIVIDER) + 1;
    }

    public static int CalcOceanSpeedMultiplier(int score)
    {
        var level = CalcLevel(score);
        // Level = 1 => oceanSpeedMultiplier = 1
        // Level = 2 => oceanSpeedMultiplier = 2
        // Level = 3 => oceanSpeedMultiplier = 2
        // Level = 4 => oceanSpeedMultiplier = 3
        // Level = 5 => oceanSpeedMultiplier = 3
        // Level = 6 => oceanSpeedMultiplier = 4
        // Level = 7 => oceanSpeedMultiplier = 4
        // etc

        return 1 + (level / 2);
    }

    public static int CalcSpawnProbability(int score)
    {
        var level = CalcLevel(score);
        // Level = 1 => spawnProbability = 1
        // Level = 2 => spawnProbability = 1
        // Level = 3 => spawnProbability = 2
        // Level = 4 => spawnProbability = 2
        // Level = 5 => spawnProbability = 3
        // Level = 6 => spawnProbability = 3
        // etc
        return 1 + ((level - 1) / 2);
    }

}
