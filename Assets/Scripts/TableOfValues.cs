/// <summary>
/// This class contains a bunch of values as they relate to PacMan, the ghosts, fruits, etc.
/// Most of what I know about the original PacMan game came from this excellent page: http://gameinternals.com/post/2072558330/understanding-pac-man-ghost-behavior
/// </summary>
public class TableOfValues
{
    /// <summary>
    /// Constant speed for all actors
    /// </summary>
    public static float Speed()
    {
        return 9.0f;
    }

    /// <summary>
    /// The speed of PacMan while he's eating dots for every level
    /// </summary>
    /// <returns>The speed as a percentage</returns>
    /// <param name="level">Level.</param>
    public static float PacManDotSpeed(int level)
    {
        switch (level) {
        case 1:
            return .71f;
        case 2:
        case 3:
        case 4:
            return .79f;
        case 5:
        case 6:
        case 7:
        case 8:
        case 9:
        case 10:
        case 11:
        case 12:
        case 13:
        case 14:
        case 15:
        case 16:
        case 17:
        case 18:
        case 19:
        case 20:
            return .87f;
        case 21:
            return .79f;
        default:
            return 0f;
        }
    }

    /// <summary>
    /// PacMan's speed while he's high on power pellets
    /// </summary>
    /// <param name="level"></param>
    /// <returns></returns>
    public static float PacManPowerPelletSpeed(int level)
    {
        switch (level) {
        case 1:
            return .79f;
        case 2:
        case 3:
        case 4:
            return .83f;
        case 5:
        case 6:
        case 7:
        case 8:
        case 9:
        case 10:
        case 11:
        case 12:
        case 13:
        case 14:
        case 15:
        case 16:
        case 17:
        case 18:
        case 19:
        case 20:
            return .87f;
        default:
            return 0f;
        }
    }

    /// <summary>
    /// The speed of PacMan for every level
    /// </summary>
    /// <returns>The speed as a percentage</returns>
    /// <param name="level">Level.</param>
    public static float PacManSpeed(int level)
    {
        switch(level) {
        case 1:
            return .80f;
        case 2:
        case 3:
        case 4:
            return .90f;
        case 5:
        case 6:
        case 7:
        case 8:
        case 9:
        case 10:
        case 11:
        case 12:
        case 13:
        case 14:
        case 15:
        case 16:
        case 17:
        case 18:
        case 19:
        case 20:
            return 1.0f;
        case 21:
            return .90f;
        default:
            return 0f;
        }
    }

    /// <summary>
    /// PacMan's speed when the ghosts are in frightened mode
    /// </summary>
    /// <param name="level"></param>
    /// <returns></returns>
    public static float PacManFrightenedSpeed(int level)
    {
        switch (level) {
        case 1:
            return .90f;
        case 2:
        case 3:
        case 4:
            return .95f;
        case 5:
        case 6:
        case 7:
        case 8:
        case 9:
        case 10:
        case 11:
        case 12:
        case 13:
        case 14:
        case 15:
        case 16:
        case 18:
            return 1.0f;
        default:
            return 0f;
        }
    }

    /// <summary>
    /// PacMan's speed while the ghosts are in frightened mode and he's eating dots
    /// </summary>
    /// <param name="level"></param>
    /// <returns></returns>
    public static float PacManFrightenedDotSpeed(int level)
    {
        switch (level) {
        case 1:
            return .79f;
        case 2:
        case 3:
        case 4:
            return .83f;
        case 5:
        case 6:
        case 7:
        case 8:
        case 9:
        case 10:
        case 11:
        case 12:
        case 13:
        case 14:
        case 15:
        case 16:
        case 18:
            return .87f;
        default:
            return 0f;
        }
    }

    /// <summary>
    /// The amount of time ghosts will remained frightened
    /// </summary>
    /// <param name="level"></param>
    /// <returns></returns>
    public static int GhostFrightenedTime(int level)
    {
        switch (level) {
        case 1:
            return 6;
        case 2:
        case 6:
        case 10:
            return 5;
        case 3:
            return 4;
        case 4:
            return 3;
        case 5:
        case 7:
        case 8:
        case 11:
            return 2;
        case 9:
        case 12:
        case 13:
        case 15:
        case 16:
        case 18:
            return 1;
        default:
            return 0;
        }
    }

    /// <summary>
    /// The number of times ghosts flash
    /// </summary>
    /// <param name="level"></param>
    /// <returns></returns>
    public static int GhostFrightenedFlashes(int level)
    {
        switch (level) {
        case 1:
        case 2:
        case 3:
        case 4:
        case 5:
        case 6:
        case 7:
        case 8:
        case 10:
        case 11:
        case 14:
            return 5;
        case 9:
        case 12:
        case 13:
        case 15:
        case 16:
        case 18:
            return 3;
        default:
            return 0;
        }
    }

    /// <summary>
    /// The ghosts' speed for each level
    /// </summary>
    /// <returns>The speed as a percentage</returns>
    /// <param name="level">Level.</param>
    public static float GhostSpeed(int level)
    {
        switch (level) {
        case 1:
            return .75f;
        case 2:
        case 3:
        case 4:
            return .85f;
        case 5:
        case 6:
        case 7:
        case 8:
        case 9:
        case 10:
        case 11:
        case 12:
        case 13:
        case 14:
        case 15:
        case 16:
        case 17:
        case 18:
        case 19:
        case 20:
            return .95f;
        default:
            return 0f;
        }
    }

    /// <summary>
    /// Speed of the ghosts while they're frightened
    /// </summary>
    /// <param name="level"></param>
    /// <returns></returns>
    public static float GhostFrightenedSpeed(int level)
    {
        switch (level) {
        case 1:
            return .50f;
        case 2:
        case 3:
        case 4:
            return .55f;
        case 5:
        case 6:
        case 7:
        case 8:
        case 9:
        case 10:
        case 11:
        case 12:
        case 13:
        case 14:
        case 15:
        case 16:
        case 17:
        case 18:
        case 19:
        case 20:
            return .60f;
        default:
            return 0f;
        }
    }

    /// <summary>
    /// The speed of the ghosts while traveling through the tunnel
    /// </summary>
    /// <param name="level"></param>
    /// <returns></returns>
    public static float GhostTunnelSpeed(int level)
    {
        switch (level) {
        case 1:
            return .40f;
        case 2:
        case 3:
        case 4:
            return .45f;
        case 5:
        case 6:
        case 7:
        case 8:
        case 9:
        case 10:
        case 11:
        case 12:
        case 13:
        case 14:
        case 15:
        case 16:
        case 17:
        case 18:
        case 19:
        case 20:
        case 21:
            return .50f;
        default:
            return 0f;
        }
    }

    /// <summary>
    /// The number of dots remaining before Blinky turns into Cruise Elroy (1st time)
    /// </summary>
    /// <returns>The number of dots remaining</returns>
    public static int CruiseElroy1DotsLeft(int level)
    {
        switch (level) {
        case 1:
            return 20;
        case 2:
            return 30;
        case 3:
        case 4:
        case 5:
            return 40;
        case 6:
        case 7:
        case 8:
            return 50;
        case 9:
        case 10:
        case 11:
            return 60;
        case 12:
        case 13:
        case 14:
            return 80;
        case 15:
        case 16:
        case 17:
        case 18:
            return 100;
        case 19:
        case 20:
        case 21:
            return 120;
        default:
            return 0;
        }
    }

    /// <summary>
    /// The number of dots remaining before Blinky turns into Cruise Elroy (2nd time)
    /// </summary>
    /// <returns>The number of dots remaining</returns>
    public static int CruiseElroy2DotsLeft(int level)
    {
        switch (level) {
        case 1:
            return 10;
        case 2:
            return 15;
        case 3:
        case 4:
        case 5:
            return 20;
        case 6:
        case 7:
        case 8:
            return 25;
        case 9:
        case 10:
        case 11:
            return 30;
        case 12:
        case 13:
        case 14:
            return 40;
        case 15:
        case 16:
        case 17:
        case 18:
            return 50;
        case 19:
        case 20:
        case 21:
            return 60;
        default:
            return 0;
        }
    }

    /// <summary>
    /// Blinky's speed the 1st time he goes into Cruise Elroy mode.
    /// </summary>
    /// <param name="level"></param>
    /// <returns></returns>
    public static float CruiseElroy1Speed(int level)
    {
        switch (level) {
        case 1:
            return .80f;
        case 2:
        case 3:
        case 4:
            return .90f;
        case 5:
        case 6:
        case 7:
        case 8:
        case 9:
        case 10:
        case 11:
        case 12:
        case 13:
        case 14:
        case 15:
        case 16:
        case 17:
        case 18:
        case 19:
        case 20:
        case 21:
            return 1.0f;
        default:
            return 0f;
        }
    }

    /// <summary>
    /// Blinky's speed the 2nd time he goes into Cruise Elroy mode.
    /// </summary>
    /// <param name="level"></param>
    /// <returns></returns>
    public static float CruiseElroy2Speed(int level)
    {
        switch (level) {
        case 1:
            return .85f;
        case 2:
        case 3:
        case 4:
            return .95f;
        case 5:
        case 6:
        case 7:
        case 8:
        case 9:
        case 10:
        case 11:
        case 12:
        case 13:
        case 14:
        case 15:
        case 16:
        case 17:
        case 18:
        case 19:
        case 20:
        case 21:
            return 1.5f;
        default:
            return 0f;
        }
    }

    /// <summary>
    /// Bonus points for each level
    /// </summary>
    /// <returns>TThe number of points for the fruit</returns>
    /// <param name="level"></param>
    public static int BonusPoints(int level)
    {
        switch (level) {
        case 1:
            return 100;
        case 2:
            return 300;
        case 3:
        case 4:
            return 500;
        case 5:
        case 6:
            return 700;
        case 7:
        case 8:
            return 1000;
        case 9:
        case 10:
            return 2000;
        case 11:
        case 12:
            return 3000;
        case 13:
        case 14:
        case 15:
        case 16:
        case 17:
        case 18:
        case 19:
        case 20:
        case 21:
            return 5000;
        default:
            return 0;
        }
    }
}

