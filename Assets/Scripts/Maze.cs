using System.Collections.Generic;
using UnityEngine;

public class Maze
{
    public static Vector2 HiddenPosition = new Vector2(-10,-10);
    public static Vector2 PacManHome = new Vector2(14.2f, 9.5f);
    public static Vector2 LeftTunnel = new Vector2 (-1, 19);
    public static Vector2 RightTunnel = new Vector2 (30, 19);

    // Ghost locations
    public static Vector2 BlinkyStartLocation = new Vector2(14.0f, 21.5f);
    public static Vector2 BlinkyScatterTarget = new Vector2 (25.5f, 35.5f);
    public static Vector2 BlinkyStartDirection = Vector2.left;
    
    public static Vector2 InkyStartLocation = new Vector2(12.0f, 18.5f);
    public static Vector2 InkyScatterTarget = new Vector2(27.5f, 0.5f);
    public static Vector2 InkyStartDirection = Vector2.up;
    
    public static Vector2 PinkyStartLocation = new Vector2(14.0f, 18.5f);
    public static Vector2 PinkyScatterTarget = new Vector2(2.5f, 35.5f);
    public static Vector2 PinkyStartDirection = Vector2.down;
    
    public static Vector2 ClydeStartLocation = new Vector2(16.0f, 18.5f);
    public static Vector2 ClydeScatterTarget = new Vector2(0.5f, 0.5f);
    public static Vector2 ClydeStartDirection = Vector2.up;
    
    public static Vector2 Ghost1Home = new Vector2(12,19);
    public static Vector2 Ghost2Home = new Vector2(14,19);
    public static Vector2 Ghost3Home = new Vector2(16,19);
    
    /// <summary>
    ///  A list of all locations of the energizer pellets
    /// </summary>
    /// <returns>The locations.</returns>
    public static List<Vector2> EnergizerLocations()
    {
        var locations = new List<Vector2>
        {
            new Vector2(2, 10), new Vector2(27, 10), new Vector2(2, 30), new Vector2(27, 30)
        };

        return locations;
    }

    /// <summary>
    /// A list of all locations for the small dots
    /// </summary>
    /// <returns>The locations.</returns>
    public static List<Vector2> DotLocations()
    {
        var locations = ValidLocations ();
        // Remove the places where dots don't exist
        locations.Remove (new Vector2 (14, 10));
        locations.Remove (new Vector2 (15, 10));
        locations.Remove (new Vector2 (2, 10));
        locations.Remove (new Vector2 (27, 10));
        locations.Remove (new Vector2 (10, 14));
        locations.Remove (new Vector2 (19, 14));
        locations.Remove (new Vector2 (10, 15));
        locations.Remove (new Vector2 (19, 15));
        locations.Remove (new Vector2 (10, 16));
        locations.Remove (new Vector2 (11, 16));
        locations.Remove (new Vector2 (12, 16));
        locations.Remove (new Vector2 (13, 16));
        locations.Remove (new Vector2 (14, 16));
        locations.Remove (new Vector2 (15, 16));
        locations.Remove (new Vector2 (16, 16));
        locations.Remove (new Vector2 (17, 16));
        locations.Remove (new Vector2 (18, 16));
        locations.Remove (new Vector2 (19, 16));
        locations.Remove (new Vector2 (10, 17));
        locations.Remove (new Vector2 (19, 17));
        locations.Remove (new Vector2 (10, 18));
        locations.Remove (new Vector2 (19, 18));
        locations.Remove (new Vector2 (1, 19));
        locations.Remove (new Vector2 (2, 19));
        locations.Remove (new Vector2 (3, 19));
        locations.Remove (new Vector2 (4, 19));
        locations.Remove (new Vector2 (5, 19));
        locations.Remove (new Vector2 (6, 19));
        locations.Remove (new Vector2 (8, 19));
        locations.Remove (new Vector2 (9, 19));
        locations.Remove (new Vector2 (10, 19));
        locations.Remove (new Vector2 (19, 19));
        locations.Remove (new Vector2 (20, 19));
        locations.Remove (new Vector2 (21, 19));
        locations.Remove (new Vector2 (23, 19));
        locations.Remove (new Vector2 (24, 19));
        locations.Remove (new Vector2 (25, 19));
        locations.Remove (new Vector2 (26, 19));
        locations.Remove (new Vector2 (27, 19));
        locations.Remove (new Vector2 (28, 19));
        locations.Remove (new Vector2 (10, 20));
        locations.Remove (new Vector2 (19, 20));
        locations.Remove (new Vector2 (10, 21));
        locations.Remove (new Vector2 (19, 21));
        locations.Remove (new Vector2 (10, 22));
        locations.Remove (new Vector2 (11, 22));
        locations.Remove (new Vector2 (12, 22));
        locations.Remove (new Vector2 (13, 22));
        locations.Remove (new Vector2 (14, 22));
        locations.Remove (new Vector2 (15, 22));
        locations.Remove (new Vector2 (16, 22));
        locations.Remove (new Vector2 (17, 22));
        locations.Remove (new Vector2 (18, 22));
        locations.Remove (new Vector2 (19, 22));
        locations.Remove (new Vector2 (13, 23));
        locations.Remove (new Vector2 (16, 23));
        locations.Remove (new Vector2 (13, 24));
        locations.Remove (new Vector2 (16, 24));
        locations.Remove (new Vector2 (2, 30));
        locations.Remove (new Vector2 (27, 30));

        locations.Remove (new Vector2 (0, 19));
        locations.Remove (LeftTunnel);
        locations.Remove (new Vector2 (29, 19));
        locations.Remove (RightTunnel);

        return locations;
    }

    /// <summary>
    /// A list of all of the valid locations for PacMan and the Ghosts
    /// </summary>
    /// <returns>The locations.</returns>
    public static List<Vector2> ValidLocations()
    {
        var locations = new List<Vector2>
        {
            new Vector2(2, 4),
            new Vector2(3, 4),
            new Vector2(4, 4),
            new Vector2(5, 4),
            new Vector2(6, 4),
            new Vector2(7, 4),
            new Vector2(8, 4),
            new Vector2(9, 4),
            new Vector2(10, 4),
            new Vector2(11, 4),
            new Vector2(12, 4),
            new Vector2(13, 4),
            new Vector2(14, 4),
            new Vector2(15, 4),
            new Vector2(16, 4),
            new Vector2(17, 4),
            new Vector2(18, 4),
            new Vector2(19, 4),
            new Vector2(20, 4),
            new Vector2(21, 4),
            new Vector2(22, 4),
            new Vector2(23, 4),
            new Vector2(24, 4),
            new Vector2(25, 4),
            new Vector2(26, 4),
            new Vector2(27, 4),
            new Vector2(2, 5),
            new Vector2(13, 5),
            new Vector2(16, 5),
            new Vector2(27, 5),
            new Vector2(2, 6),
            new Vector2(13, 6),
            new Vector2(16, 6),
            new Vector2(27, 6),
            new Vector2(2, 7),
            new Vector2(3, 7),
            new Vector2(4, 7),
            new Vector2(5, 7),
            new Vector2(6, 7),
            new Vector2(7, 7),
            new Vector2(10, 7),
            new Vector2(11, 7),
            new Vector2(12, 7),
            new Vector2(13, 7),
            new Vector2(16, 7),
            new Vector2(17, 7),
            new Vector2(18, 7),
            new Vector2(19, 7),
            new Vector2(22, 7),
            new Vector2(23, 7),
            new Vector2(24, 7),
            new Vector2(25, 7),
            new Vector2(26, 7),
            new Vector2(27, 7),
            new Vector2(4, 8),
            new Vector2(7, 8),
            new Vector2(10, 8),
            new Vector2(19, 8),
            new Vector2(22, 8),
            new Vector2(25, 8),
            new Vector2(4, 9),
            new Vector2(7, 9),
            new Vector2(10, 9),
            new Vector2(19, 9),
            new Vector2(22, 9),
            new Vector2(25, 9),
            new Vector2(2, 10),
            new Vector2(3, 10),
            new Vector2(4, 10),
            new Vector2(7, 10),
            new Vector2(8, 10),
            new Vector2(9, 10),
            new Vector2(10, 10),
            new Vector2(11, 10),
            new Vector2(12, 10),
            new Vector2(13, 10),
            new Vector2(14, 10),
            new Vector2(15, 10),
            new Vector2(16, 10),
            new Vector2(17, 10),
            new Vector2(18, 10),
            new Vector2(19, 10),
            new Vector2(20, 10),
            new Vector2(21, 10),
            new Vector2(22, 10),
            new Vector2(25, 10),
            new Vector2(26, 10),
            new Vector2(27, 10),
            new Vector2(2, 11),
            new Vector2(7, 11),
            new Vector2(13, 11),
            new Vector2(16, 11),
            new Vector2(22, 11),
            new Vector2(27, 11),
            new Vector2(2, 12),
            new Vector2(7, 12),
            new Vector2(13, 12),
            new Vector2(16, 12),
            new Vector2(22, 12),
            new Vector2(27, 12),
            new Vector2(2, 13),
            new Vector2(3, 13),
            new Vector2(4, 13),
            new Vector2(5, 13),
            new Vector2(6, 13),
            new Vector2(7, 13),
            new Vector2(8, 13),
            new Vector2(9, 13),
            new Vector2(10, 13),
            new Vector2(11, 13),
            new Vector2(12, 13),
            new Vector2(13, 13),
            new Vector2(16, 13),
            new Vector2(17, 13),
            new Vector2(18, 13),
            new Vector2(19, 13),
            new Vector2(20, 13),
            new Vector2(21, 13),
            new Vector2(22, 13),
            new Vector2(23, 13),
            new Vector2(24, 13),
            new Vector2(25, 13),
            new Vector2(26, 13),
            new Vector2(27, 13),
            new Vector2(7, 14),
            new Vector2(10, 14),
            new Vector2(19, 14),
            new Vector2(22, 14),
            new Vector2(7, 15),
            new Vector2(10, 15),
            new Vector2(19, 15),
            new Vector2(22, 15),
            new Vector2(7, 16),
            new Vector2(10, 16),
            new Vector2(11, 16),
            new Vector2(12, 16),
            new Vector2(13, 16),
            new Vector2(14, 16),
            new Vector2(15, 16),
            new Vector2(16, 16),
            new Vector2(17, 16),
            new Vector2(18, 16),
            new Vector2(19, 16),
            new Vector2(22, 16),
            new Vector2(7, 17),
            new Vector2(10, 17),
            new Vector2(19, 17),
            new Vector2(22, 17),
            new Vector2(7, 18),
            new Vector2(10, 18),
            new Vector2(19, 18),
            new Vector2(22, 18),
            new Vector2(1, 19),
            new Vector2(2, 19),
            new Vector2(3, 19),
            new Vector2(4, 19),
            new Vector2(5, 19),
            new Vector2(6, 19),
            new Vector2(7, 19),
            new Vector2(8, 19),
            new Vector2(9, 19),
            new Vector2(10, 19),
            new Vector2(19, 19),
            new Vector2(20, 19),
            new Vector2(21, 19),
            new Vector2(22, 19),
            new Vector2(23, 19),
            new Vector2(24, 19),
            new Vector2(25, 19),
            new Vector2(26, 19),
            new Vector2(27, 19),
            new Vector2(28, 19),
            new Vector2(0, 19),
            LeftTunnel,
            new Vector2(29, 19),
            RightTunnel,
            new Vector2(7, 20),
            new Vector2(10, 20),
            new Vector2(19, 20),
            new Vector2(22, 20),
            new Vector2(7, 21),
            new Vector2(10, 21),
            new Vector2(19, 21),
            new Vector2(22, 21),
            new Vector2(7, 22),
            new Vector2(10, 22),
            new Vector2(11, 22),
            new Vector2(12, 22),
            new Vector2(13, 22),
            new Vector2(14, 22),
            new Vector2(15, 22),
            new Vector2(16, 22),
            new Vector2(17, 22),
            new Vector2(18, 22),
            new Vector2(19, 22),
            new Vector2(22, 22),
            new Vector2(7, 23),
            new Vector2(13, 23),
            new Vector2(16, 23),
            new Vector2(22, 23),
            new Vector2(7, 24),
            new Vector2(13, 24),
            new Vector2(16, 24),
            new Vector2(22, 24),
            new Vector2(2, 25),
            new Vector2(3, 25),
            new Vector2(4, 25),
            new Vector2(5, 25),
            new Vector2(6, 25),
            new Vector2(7, 25),
            new Vector2(10, 25),
            new Vector2(11, 25),
            new Vector2(12, 25),
            new Vector2(13, 25),
            new Vector2(16, 25),
            new Vector2(17, 25),
            new Vector2(18, 25),
            new Vector2(19, 25),
            new Vector2(22, 25),
            new Vector2(23, 25),
            new Vector2(24, 25),
            new Vector2(25, 25),
            new Vector2(26, 25),
            new Vector2(27, 25),
            new Vector2(2, 26),
            new Vector2(7, 26),
            new Vector2(10, 26),
            new Vector2(19, 26),
            new Vector2(22, 26),
            new Vector2(27, 26),
            new Vector2(2, 27),
            new Vector2(7, 27),
            new Vector2(10, 27),
            new Vector2(19, 27),
            new Vector2(22, 27),
            new Vector2(27, 27),
            new Vector2(2, 28),
            new Vector2(3, 28),
            new Vector2(4, 28),
            new Vector2(5, 28),
            new Vector2(6, 28),
            new Vector2(7, 28),
            new Vector2(8, 28),
            new Vector2(9, 28),
            new Vector2(10, 28),
            new Vector2(11, 28),
            new Vector2(12, 28),
            new Vector2(13, 28),
            new Vector2(14, 28),
            new Vector2(15, 28),
            new Vector2(16, 28),
            new Vector2(17, 28),
            new Vector2(18, 28),
            new Vector2(19, 28),
            new Vector2(20, 28),
            new Vector2(21, 28),
            new Vector2(22, 28),
            new Vector2(23, 28),
            new Vector2(24, 28),
            new Vector2(25, 28),
            new Vector2(26, 28),
            new Vector2(27, 28),
            new Vector2(2, 29),
            new Vector2(7, 29),
            new Vector2(13, 29),
            new Vector2(16, 29),
            new Vector2(22, 29),
            new Vector2(27, 29),
            new Vector2(2, 30),
            new Vector2(7, 30),
            new Vector2(13, 30),
            new Vector2(16, 30),
            new Vector2(22, 30),
            new Vector2(27, 30),
            new Vector2(2, 31),
            new Vector2(7, 31),
            new Vector2(13, 31),
            new Vector2(16, 31),
            new Vector2(22, 31),
            new Vector2(27, 31),
            new Vector2(2, 32),
            new Vector2(3, 32),
            new Vector2(4, 32),
            new Vector2(5, 32),
            new Vector2(6, 32),
            new Vector2(7, 32),
            new Vector2(8, 32),
            new Vector2(9, 32),
            new Vector2(10, 32),
            new Vector2(11, 32),
            new Vector2(12, 32),
            new Vector2(13, 32),
            new Vector2(16, 32),
            new Vector2(17, 32),
            new Vector2(18, 32),
            new Vector2(19, 32),
            new Vector2(20, 32),
            new Vector2(21, 32),
            new Vector2(22, 32),
            new Vector2(23, 32),
            new Vector2(24, 32),
            new Vector2(25, 32),
            new Vector2(26, 32),
            new Vector2(27, 32)
        };

        return locations;
    }

    /// <summary>
    /// A list of all of the special locations for the Ghosts
    /// </summary>
    /// <returns>The locations.</returns>
    public static List<Vector2> SpecialLocations()
    {
        var locations = new List<Vector2>
        {
            new Vector2(16, 10), new Vector2(13, 10), new Vector2(16, 22), new Vector2(13, 22)
        };

        return locations;
    }

    /// <summary>
    /// A list of all of the intersections on the maze.
    /// </summary>
    public static List<Vector2> Intersections()
    {
        var locations = new List<Vector2>
        {
            new Vector2(16, 4),
            new Vector2(13, 4),
            new Vector2(4, 7),
            new Vector2(25, 7),
            new Vector2(7, 10),
            new Vector2(10, 10),
            new Vector2(19, 10),
            new Vector2(22, 10),
            new Vector2(7, 13),
            new Vector2(10, 13),
            new Vector2(19, 13),
            new Vector2(22, 13),
            new Vector2(10, 16),
            new Vector2(19, 16),
            new Vector2(7, 19),
            new Vector2(10, 19),
            new Vector2(19, 19),
            new Vector2(22, 19),
            new Vector2(7, 25),
            new Vector2(22, 25),
            new Vector2(2, 28),
            new Vector2(7, 28),
            new Vector2(10, 28),
            new Vector2(13, 28),
            new Vector2(16, 28),
            new Vector2(19, 28),
            new Vector2(22, 28),
            new Vector2(27, 28),
            new Vector2(7, 32),
            new Vector2(22, 32)
        };

        return locations;
    }
}
