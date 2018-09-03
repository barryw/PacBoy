using System.Collections.Generic;
using UnityEngine;

public class Maze
{
    private static Maze _instance;

    private Maze ()
    {
    }

    // We are a singleton
    public static Maze Instance()
    {
        return _instance ?? (_instance = new Maze());
    }

    public Vector2 LeftTunnel = new Vector2 (-1, 19);
    public Vector2 RightTunnel = new Vector2 (30, 19);

    /// <summary>
    ///  A list of all locations of the energizer pellets
    /// </summary>
    /// <returns>The locations.</returns>
    public List<Vector2> EnergizerLocations()
    {
        var locations = new List<Vector2> ();
        locations.Add (new Vector2 (2, 10));
        locations.Add (new Vector2 (27, 10));
        locations.Add (new Vector2 (2, 30));
        locations.Add (new Vector2 (27, 30));

        return locations;
    }

    /// <summary>
    /// A list of all locations for the small dots
    /// </summary>
    /// <returns>The locations.</returns>
    public List<Vector2> DotLocations()
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
    public List<Vector2> ValidLocations()
    {
        var locations = new List<Vector2> ();

        // Row 4
        locations.Add (new Vector2 (2, 4));
        locations.Add (new Vector2 (3, 4));
        locations.Add (new Vector2 (4, 4));
        locations.Add (new Vector2 (5, 4));
        locations.Add (new Vector2 (6, 4));
        locations.Add (new Vector2 (7, 4));
        locations.Add (new Vector2 (8, 4));
        locations.Add (new Vector2 (9, 4));
        locations.Add (new Vector2 (10, 4));
        locations.Add (new Vector2 (11, 4));
        locations.Add (new Vector2 (12, 4));
        locations.Add (new Vector2 (13, 4));
        locations.Add (new Vector2 (14, 4));
        locations.Add (new Vector2 (15, 4));
        locations.Add (new Vector2 (16, 4));
        locations.Add (new Vector2 (17, 4));
        locations.Add (new Vector2 (18, 4));
        locations.Add (new Vector2 (19, 4));
        locations.Add (new Vector2 (20, 4));
        locations.Add (new Vector2 (21, 4));
        locations.Add (new Vector2 (22, 4));
        locations.Add (new Vector2 (23, 4));
        locations.Add (new Vector2 (24, 4));
        locations.Add (new Vector2 (25, 4));
        locations.Add (new Vector2 (26, 4));
        locations.Add (new Vector2 (27, 4));

        // Row 5
        locations.Add (new Vector2 (2, 5));
        locations.Add (new Vector2 (13, 5));
        locations.Add (new Vector2 (16, 5));
        locations.Add (new Vector2 (27, 5));

        // Row 6
        locations.Add (new Vector2 (2, 6));
        locations.Add (new Vector2 (13, 6));
        locations.Add (new Vector2 (16, 6));
        locations.Add (new Vector2 (27, 6));

        // Row 7
        locations.Add (new Vector2 (2, 7));
        locations.Add (new Vector2 (3, 7));
        locations.Add (new Vector2 (4, 7));
        locations.Add (new Vector2 (5, 7));
        locations.Add (new Vector2 (6, 7));
        locations.Add (new Vector2 (7, 7));
        locations.Add (new Vector2 (10, 7));
        locations.Add (new Vector2 (11, 7));
        locations.Add (new Vector2 (12, 7));
        locations.Add (new Vector2 (13, 7));
        locations.Add (new Vector2 (16, 7));
        locations.Add (new Vector2 (17, 7));
        locations.Add (new Vector2 (18, 7));
        locations.Add (new Vector2 (19, 7));
        locations.Add (new Vector2 (22, 7));
        locations.Add (new Vector2 (23, 7));
        locations.Add (new Vector2 (24, 7));
        locations.Add (new Vector2 (25, 7));
        locations.Add (new Vector2 (26, 7));
        locations.Add (new Vector2 (27, 7));

        // Row 8
        locations.Add (new Vector2 (4, 8));
        locations.Add (new Vector2 (7, 8));
        locations.Add (new Vector2 (10, 8));
        locations.Add (new Vector2 (19, 8));
        locations.Add (new Vector2 (22, 8));
        locations.Add (new Vector2 (25, 8));

        // Row 9
        locations.Add (new Vector2 (4, 9));
        locations.Add (new Vector2 (7, 9));
        locations.Add (new Vector2 (10, 9));
        locations.Add (new Vector2 (19, 9));
        locations.Add (new Vector2 (22, 9));
        locations.Add (new Vector2 (25, 9));

        // Row 10
        locations.Add (new Vector2 (2, 10));
        locations.Add (new Vector2 (3, 10));
        locations.Add (new Vector2 (4, 10));
        locations.Add (new Vector2 (7, 10));
        locations.Add (new Vector2 (8, 10));
        locations.Add (new Vector2 (9, 10));
        locations.Add (new Vector2 (10, 10));
        locations.Add (new Vector2 (11, 10));
        locations.Add (new Vector2 (12, 10));
        locations.Add (new Vector2 (13, 10));
        locations.Add (new Vector2 (14, 10));
        locations.Add (new Vector2 (15, 10));
        locations.Add (new Vector2 (16, 10));
        locations.Add (new Vector2 (17, 10));
        locations.Add (new Vector2 (18, 10));
        locations.Add (new Vector2 (19, 10));
        locations.Add (new Vector2 (20, 10));
        locations.Add (new Vector2 (21, 10));
        locations.Add (new Vector2 (22, 10));
        locations.Add (new Vector2 (25, 10));
        locations.Add (new Vector2 (26, 10));
        locations.Add (new Vector2 (27, 10));

        // Row 11
        locations.Add (new Vector2 (2, 11));
        locations.Add (new Vector2 (7, 11));
        locations.Add (new Vector2 (13, 11));
        locations.Add (new Vector2 (16, 11));
        locations.Add (new Vector2 (22, 11));
        locations.Add (new Vector2 (27, 11));

        // Row 12
        locations.Add (new Vector2 (2, 12));
        locations.Add (new Vector2 (7, 12));
        locations.Add (new Vector2 (13, 12));
        locations.Add (new Vector2 (16, 12));
        locations.Add (new Vector2 (22, 12));
        locations.Add (new Vector2 (27, 12));

        // Row 13
        locations.Add (new Vector2(2, 13));
        locations.Add (new Vector2(3, 13));
        locations.Add (new Vector2(4, 13));
        locations.Add (new Vector2(5, 13));
        locations.Add (new Vector2(6, 13));
        locations.Add (new Vector2(7, 13));
        locations.Add (new Vector2(8, 13));
        locations.Add (new Vector2(9, 13));
        locations.Add (new Vector2(10, 13));
        locations.Add (new Vector2(11, 13));
        locations.Add (new Vector2(12, 13));
        locations.Add (new Vector2(13, 13));
        locations.Add (new Vector2(16, 13));
        locations.Add (new Vector2(17, 13));
        locations.Add (new Vector2(18, 13));
        locations.Add (new Vector2(19, 13));
        locations.Add (new Vector2(20, 13));
        locations.Add (new Vector2(21, 13));
        locations.Add (new Vector2(22, 13));
        locations.Add (new Vector2(23, 13));
        locations.Add (new Vector2(24, 13));
        locations.Add (new Vector2(25, 13));
        locations.Add (new Vector2(26, 13));
        locations.Add (new Vector2(27, 13));

        // Row 14
        locations.Add (new Vector2 (7, 14));
        locations.Add (new Vector2 (10, 14));
        locations.Add (new Vector2 (19, 14));
        locations.Add (new Vector2 (22, 14));

        // Row 15
        locations.Add (new Vector2 (7, 15));
        locations.Add (new Vector2 (10, 15));
        locations.Add (new Vector2 (19, 15));
        locations.Add (new Vector2 (22, 15));

        // Row 16
        locations.Add (new Vector2 (7, 16));
        locations.Add (new Vector2 (10, 16));
        locations.Add (new Vector2 (11, 16));
        locations.Add (new Vector2 (12, 16));
        locations.Add (new Vector2 (13, 16));
        locations.Add (new Vector2 (14, 16));
        locations.Add (new Vector2 (15, 16));
        locations.Add (new Vector2 (16, 16));
        locations.Add (new Vector2 (17, 16));
        locations.Add (new Vector2 (18, 16));
        locations.Add (new Vector2 (19, 16));
        locations.Add (new Vector2 (22, 16));

        // Row 17
        locations.Add (new Vector2 (7, 17));
        locations.Add (new Vector2 (10, 17));
        locations.Add (new Vector2 (19, 17));
        locations.Add (new Vector2 (22, 17));

        // Row 18
        locations.Add (new Vector2 (7, 18));
        locations.Add (new Vector2 (10, 18));
        locations.Add (new Vector2 (19, 18));
        locations.Add (new Vector2 (22, 18));

        // Row 19
        locations.Add (new Vector2 (1, 19));
        locations.Add (new Vector2 (2, 19));
        locations.Add (new Vector2 (3, 19));
        locations.Add (new Vector2 (4, 19));
        locations.Add (new Vector2 (5, 19));
        locations.Add (new Vector2 (6, 19));
        locations.Add (new Vector2 (7, 19));
        locations.Add (new Vector2 (8, 19));
        locations.Add (new Vector2 (9, 19));
        locations.Add (new Vector2 (10, 19));
        locations.Add (new Vector2 (19, 19));
        locations.Add (new Vector2 (20, 19));
        locations.Add (new Vector2 (21, 19));
        locations.Add (new Vector2 (22, 19));
        locations.Add (new Vector2 (23, 19));
        locations.Add (new Vector2 (24, 19));
        locations.Add (new Vector2 (25, 19));
        locations.Add (new Vector2 (26, 19));
        locations.Add (new Vector2 (27, 19));
        locations.Add (new Vector2 (28, 19));

        // Special locations for tunnel
        locations.Add (new Vector2 (0, 19));
        locations.Add (LeftTunnel);
        locations.Add (new Vector2 (29, 19));
        locations.Add (RightTunnel);

        // Row 20
        locations.Add (new Vector2 (7, 20));
        locations.Add (new Vector2 (10, 20));
        locations.Add (new Vector2 (19, 20));
        locations.Add (new Vector2 (22, 20));

        // Row 21
        locations.Add (new Vector2 (7, 21));
        locations.Add (new Vector2 (10, 21));
        locations.Add (new Vector2 (19, 21));
        locations.Add (new Vector2 (22, 21));

        // Row 22
        locations.Add (new Vector2 (7, 22));
        locations.Add (new Vector2 (10, 22));
        locations.Add (new Vector2 (11, 22));
        locations.Add (new Vector2 (12, 22));
        locations.Add (new Vector2 (13, 22));
        locations.Add (new Vector2 (14, 22));
        locations.Add (new Vector2 (15, 22));
        locations.Add (new Vector2 (16, 22));
        locations.Add (new Vector2 (17, 22));
        locations.Add (new Vector2 (18, 22));
        locations.Add (new Vector2 (19, 22));
        locations.Add (new Vector2 (22, 22));

        // Row 23
        locations.Add (new Vector2 (7, 23));
        locations.Add (new Vector2 (13, 23));
        locations.Add (new Vector2 (16, 23));
        locations.Add (new Vector2 (22, 23));

        // Row 24
        locations.Add (new Vector2 (7, 24));
        locations.Add (new Vector2 (13, 24));
        locations.Add (new Vector2 (16, 24));
        locations.Add (new Vector2 (22, 24));

        // Row 25
        locations.Add (new Vector2 (2, 25));
        locations.Add (new Vector2 (3, 25));
        locations.Add (new Vector2 (4, 25));
        locations.Add (new Vector2 (5, 25));
        locations.Add (new Vector2 (6, 25));
        locations.Add (new Vector2 (7, 25));
        locations.Add (new Vector2 (10, 25));
        locations.Add (new Vector2 (11, 25));
        locations.Add (new Vector2 (12, 25));
        locations.Add (new Vector2 (13, 25));
        locations.Add (new Vector2 (16, 25));
        locations.Add (new Vector2 (17, 25));
        locations.Add (new Vector2 (18, 25));
        locations.Add (new Vector2 (19, 25));
        locations.Add (new Vector2 (22, 25));
        locations.Add (new Vector2 (23, 25));
        locations.Add (new Vector2 (24, 25));
        locations.Add (new Vector2 (25, 25));
        locations.Add (new Vector2 (26, 25));
        locations.Add (new Vector2 (27, 25));

        // Row 26
        locations.Add (new Vector2 (2, 26));
        locations.Add (new Vector2 (7, 26));
        locations.Add (new Vector2 (10, 26));
        locations.Add (new Vector2 (19, 26));
        locations.Add (new Vector2 (22, 26));
        locations.Add (new Vector2 (27, 26));

        // Row 27
        locations.Add (new Vector2 (2, 27));
        locations.Add (new Vector2 (7, 27));
        locations.Add (new Vector2 (10, 27));
        locations.Add (new Vector2 (19, 27));
        locations.Add (new Vector2 (22, 27));
        locations.Add (new Vector2 (27, 27));

        // Row 28
        locations.Add (new Vector2 (2, 28));
        locations.Add (new Vector2 (3, 28));
        locations.Add (new Vector2 (4, 28));
        locations.Add (new Vector2 (5, 28));
        locations.Add (new Vector2 (6, 28));
        locations.Add (new Vector2 (7, 28));
        locations.Add (new Vector2 (8, 28));
        locations.Add (new Vector2 (9, 28));
        locations.Add (new Vector2 (10, 28));
        locations.Add (new Vector2 (11, 28));
        locations.Add (new Vector2 (12, 28));
        locations.Add (new Vector2 (13, 28));
        locations.Add (new Vector2 (14, 28));
        locations.Add (new Vector2 (15, 28));
        locations.Add (new Vector2 (16, 28));
        locations.Add (new Vector2 (17, 28));
        locations.Add (new Vector2 (18, 28));
        locations.Add (new Vector2 (19, 28));
        locations.Add (new Vector2 (20, 28));
        locations.Add (new Vector2 (21, 28));
        locations.Add (new Vector2 (22, 28));
        locations.Add (new Vector2 (23, 28));
        locations.Add (new Vector2 (24, 28));
        locations.Add (new Vector2 (25, 28));
        locations.Add (new Vector2 (26, 28));
        locations.Add (new Vector2 (27, 28));

        // Row 29
        locations.Add (new Vector2 (2, 29));
        locations.Add (new Vector2 (7, 29));
        locations.Add (new Vector2 (13, 29));
        locations.Add (new Vector2 (16, 29));
        locations.Add (new Vector2 (22, 29));
        locations.Add (new Vector2 (27, 29));

        // Row 30
        locations.Add (new Vector2 (2, 30));
        locations.Add (new Vector2 (7, 30));
        locations.Add (new Vector2 (13, 30));
        locations.Add (new Vector2 (16, 30));
        locations.Add (new Vector2 (22, 30));
        locations.Add (new Vector2 (27, 30));

        // Row 31
        locations.Add (new Vector2 (2, 31));
        locations.Add (new Vector2 (7, 31));
        locations.Add (new Vector2 (13, 31));
        locations.Add (new Vector2 (16, 31));
        locations.Add (new Vector2 (22, 31));
        locations.Add (new Vector2 (27, 31));

        // Row 32
        locations.Add (new Vector2 (2, 32));
        locations.Add (new Vector2 (3, 32));
        locations.Add (new Vector2 (4, 32));
        locations.Add (new Vector2 (5, 32));
        locations.Add (new Vector2 (6, 32));
        locations.Add (new Vector2 (7, 32));
        locations.Add (new Vector2 (8, 32));
        locations.Add (new Vector2 (9, 32));
        locations.Add (new Vector2 (10, 32));
        locations.Add (new Vector2 (11, 32));
        locations.Add (new Vector2 (12, 32));
        locations.Add (new Vector2 (13, 32));
        locations.Add (new Vector2 (16, 32));
        locations.Add (new Vector2 (17, 32));
        locations.Add (new Vector2 (18, 32));
        locations.Add (new Vector2 (19, 32));
        locations.Add (new Vector2 (20, 32));
        locations.Add (new Vector2 (21, 32));
        locations.Add (new Vector2 (22, 32));
        locations.Add (new Vector2 (23, 32));
        locations.Add (new Vector2 (24, 32));
        locations.Add (new Vector2 (25, 32));
        locations.Add (new Vector2 (26, 32));
        locations.Add (new Vector2 (27, 32));

        return locations;
    }

    /// <summary>
    /// A list of all of the special locations for the Ghosts
    /// </summary>
    /// <returns>The locations.</returns>
    public List<Vector2> SpecialLocations()
    {
        var locations = new List<Vector2> ();
        locations.Add (new Vector2 (16, 10));
        locations.Add (new Vector2 (13, 10));
        locations.Add (new Vector2 (16, 22));
        locations.Add (new Vector2 (13, 22));

        return locations;
    }

    /// <summary>
    /// A list of all of the intersections on the maze.
    /// </summary>
    public List<Vector2> Intersections()
    {
        var locations = new List<Vector2> ();

        locations.Add (new Vector2 (16, 4));
        locations.Add (new Vector2 (13, 4));
        locations.Add (new Vector2 (4, 7));
        locations.Add (new Vector2 (25, 7));
        locations.Add (new Vector2 (7, 10));
        locations.Add (new Vector2 (10, 10));
        locations.Add (new Vector2 (19, 10));
        locations.Add (new Vector2 (22, 10));
        locations.Add (new Vector2 (7, 13));
        locations.Add (new Vector2 (10, 13));
        locations.Add (new Vector2 (19, 13));
        locations.Add (new Vector2 (22, 13));
        locations.Add (new Vector2 (10, 16));
        locations.Add (new Vector2 (19, 16));
        locations.Add (new Vector2 (7, 19));
        locations.Add (new Vector2 (10, 19));
        locations.Add (new Vector2 (19, 19));
        locations.Add (new Vector2 (22, 19));
        locations.Add (new Vector2 (7, 25));
        locations.Add (new Vector2 (22, 25));
        locations.Add (new Vector2 (2, 28));
        locations.Add (new Vector2 (7, 28));
        locations.Add (new Vector2 (10, 28));
        locations.Add (new Vector2 (13, 28));
        locations.Add (new Vector2 (16, 28));
        locations.Add (new Vector2 (19, 28));
        locations.Add (new Vector2 (22, 28));
        locations.Add (new Vector2 (27, 28));
        locations.Add (new Vector2 (7, 32));
        locations.Add (new Vector2 (22, 32));

        return locations;
    }
}
