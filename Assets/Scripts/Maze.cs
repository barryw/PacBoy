using System;
using System.Collections.Generic;
using UnityEngine;

public class Maze
{
    static Maze _instance;

    private Maze ()
    {
    }

    public static Maze Instance()
    {
        if (_instance == null)
            _instance = new Maze ();

        return _instance;
    }

    /// <summary>
    ///  A list of all locations of the energizer pellets
    /// </summary>
    /// <returns>The locations.</returns>
    public List<Vector2> EnergizerLocations()
    {
        List<Vector2> Locations = new List<Vector2> ();
        Locations.Add (new Vector2 (2, 10));
        Locations.Add (new Vector2 (27, 10));
        Locations.Add (new Vector2 (2, 30));
        Locations.Add (new Vector2 (27, 30));

        return Locations;
    }

    /// <summary>
    /// A list of all locations for the small dots
    /// </summary>
    /// <returns>The locations.</returns>
    public List<Vector2> DotLocations()
    {
        List<Vector2> Locations = ValidLocations ();
        // Remove the places where dots don't exist
        Locations.Remove (new Vector2 (14, 10));
        Locations.Remove (new Vector2 (15, 10));
        Locations.Remove (new Vector2 (10, 14));
        Locations.Remove (new Vector2 (19, 14));
        Locations.Remove (new Vector2 (10, 15));
        Locations.Remove (new Vector2 (19, 15));
        Locations.Remove (new Vector2 (10, 16));
        Locations.Remove (new Vector2 (11, 16));
        Locations.Remove (new Vector2 (12, 16));
        Locations.Remove (new Vector2 (13, 16));
        Locations.Remove (new Vector2 (14, 16));
        Locations.Remove (new Vector2 (15, 16));
        Locations.Remove (new Vector2 (16, 16));
        Locations.Remove (new Vector2 (17, 16));
        Locations.Remove (new Vector2 (18, 16));
        Locations.Remove (new Vector2 (19, 16));
        Locations.Remove (new Vector2 (10, 17));
        Locations.Remove (new Vector2 (19, 17));
        Locations.Remove (new Vector2 (10, 18));
        Locations.Remove (new Vector2 (19, 18));
        Locations.Remove (new Vector2 (1, 19));
        Locations.Remove (new Vector2 (2, 19));
        Locations.Remove (new Vector2 (3, 19));
        Locations.Remove (new Vector2 (4, 19));
        Locations.Remove (new Vector2 (5, 19));
        Locations.Remove (new Vector2 (6, 19));
        Locations.Remove (new Vector2 (8, 19));
        Locations.Remove (new Vector2 (9, 19));
        Locations.Remove (new Vector2 (10, 19));
        Locations.Remove (new Vector2 (19, 19));
        Locations.Remove (new Vector2 (20, 19));
        Locations.Remove (new Vector2 (21, 19));
        Locations.Remove (new Vector2 (23, 19));
        Locations.Remove (new Vector2 (24, 19));
        Locations.Remove (new Vector2 (25, 19));
        Locations.Remove (new Vector2 (26, 19));
        Locations.Remove (new Vector2 (27, 19));
        Locations.Remove (new Vector2 (28, 19));
        Locations.Remove (new Vector2 (10, 20));
        Locations.Remove (new Vector2 (19, 20));
        Locations.Remove (new Vector2 (10, 21));
        Locations.Remove (new Vector2 (19, 21));
        Locations.Remove (new Vector2 (10, 22));
        Locations.Remove (new Vector2 (11, 22));
        Locations.Remove (new Vector2 (12, 22));
        Locations.Remove (new Vector2 (13, 22));
        Locations.Remove (new Vector2 (14, 22));
        Locations.Remove (new Vector2 (15, 22));
        Locations.Remove (new Vector2 (16, 22));
        Locations.Remove (new Vector2 (17, 22));
        Locations.Remove (new Vector2 (18, 22));
        Locations.Remove (new Vector2 (19, 22));
        Locations.Remove (new Vector2 (13, 23));
        Locations.Remove (new Vector2 (16, 23));
        Locations.Remove (new Vector2 (13, 24));
        Locations.Remove (new Vector2 (16, 24));

        return Locations;
    }

    /// <summary>
    /// A list of all of the valid locations for PacMan and the Ghosts
    /// </summary>
    /// <returns>The locations.</returns>
    public List<Vector2> ValidLocations()
    {
        List<Vector2> Locations = new List<Vector2> ();

        // Row 4
        Locations.Add (new Vector2 (2, 4));
        Locations.Add (new Vector2 (3, 4));
        Locations.Add (new Vector2 (4, 4));
        Locations.Add (new Vector2 (5, 4));
        Locations.Add (new Vector2 (6, 4));
        Locations.Add (new Vector2 (7, 4));
        Locations.Add (new Vector2 (8, 4));
        Locations.Add (new Vector2 (9, 4));
        Locations.Add (new Vector2 (10, 4));
        Locations.Add (new Vector2 (11, 4));
        Locations.Add (new Vector2 (12, 4));
        Locations.Add (new Vector2 (13, 4));
        Locations.Add (new Vector2 (14, 4));
        Locations.Add (new Vector2 (15, 4));
        Locations.Add (new Vector2 (16, 4));
        Locations.Add (new Vector2 (17, 4));
        Locations.Add (new Vector2 (18, 4));
        Locations.Add (new Vector2 (19, 4));
        Locations.Add (new Vector2 (20, 4));
        Locations.Add (new Vector2 (21, 4));
        Locations.Add (new Vector2 (22, 4));
        Locations.Add (new Vector2 (23, 4));
        Locations.Add (new Vector2 (24, 4));
        Locations.Add (new Vector2 (25, 4));
        Locations.Add (new Vector2 (26, 4));
        Locations.Add (new Vector2 (27, 4));

        // Row 5
        Locations.Add (new Vector2 (2, 5));
        Locations.Add (new Vector2 (13, 5));
        Locations.Add (new Vector2 (16, 5));
        Locations.Add (new Vector2 (27, 5));

        // Row 6
        Locations.Add (new Vector2 (2, 6));
        Locations.Add (new Vector2 (13, 6));
        Locations.Add (new Vector2 (16, 6));
        Locations.Add (new Vector2 (27, 6));

        // Row 7
        Locations.Add (new Vector2 (2, 7));
        Locations.Add (new Vector2 (3, 7));
        Locations.Add (new Vector2 (4, 7));
        Locations.Add (new Vector2 (5, 7));
        Locations.Add (new Vector2 (6, 7));
        Locations.Add (new Vector2 (7, 7));
        Locations.Add (new Vector2 (10, 7));
        Locations.Add (new Vector2 (11, 7));
        Locations.Add (new Vector2 (12, 7));
        Locations.Add (new Vector2 (13, 7));
        Locations.Add (new Vector2 (16, 7));
        Locations.Add (new Vector2 (17, 7));
        Locations.Add (new Vector2 (18, 7));
        Locations.Add (new Vector2 (19, 7));
        Locations.Add (new Vector2 (22, 7));
        Locations.Add (new Vector2 (23, 7));
        Locations.Add (new Vector2 (24, 7));
        Locations.Add (new Vector2 (25, 7));
        Locations.Add (new Vector2 (26, 7));
        Locations.Add (new Vector2 (27, 7));

        // Row 8
        Locations.Add (new Vector2 (4, 8));
        Locations.Add (new Vector2 (7, 8));
        Locations.Add (new Vector2 (10, 8));
        Locations.Add (new Vector2 (19, 8));
        Locations.Add (new Vector2 (22, 8));
        Locations.Add (new Vector2 (25, 8));

        // Row 9
        Locations.Add (new Vector2 (4, 9));
        Locations.Add (new Vector2 (7, 9));
        Locations.Add (new Vector2 (10, 9));
        Locations.Add (new Vector2 (19, 9));
        Locations.Add (new Vector2 (22, 9));
        Locations.Add (new Vector2 (25, 9));

        // Row 10
        Locations.Add (new Vector2 (2,10));
        Locations.Add (new Vector2 (3, 10));
        Locations.Add (new Vector2 (4, 10));
        Locations.Add (new Vector2 (7, 10));
        Locations.Add (new Vector2 (8, 10));
        Locations.Add (new Vector2 (9, 10));
        Locations.Add (new Vector2 (10, 10));
        Locations.Add (new Vector2 (11, 10));
        Locations.Add (new Vector2 (12, 10));
        Locations.Add (new Vector2 (13, 10));
        Locations.Add (new Vector2 (14, 10));
        Locations.Add (new Vector2 (15, 10));
        Locations.Add (new Vector2 (16, 10));
        Locations.Add (new Vector2 (17, 10));
        Locations.Add (new Vector2 (18, 10));
        Locations.Add (new Vector2 (19, 10));
        Locations.Add (new Vector2 (20, 10));
        Locations.Add (new Vector2 (21, 10));
        Locations.Add (new Vector2 (22, 10));
        Locations.Add (new Vector2 (25, 10));
        Locations.Add (new Vector2 (26, 10));
        Locations.Add (new Vector2 (27, 10));

        // Row 11
        Locations.Add (new Vector2 (2, 11));
        Locations.Add (new Vector2 (7, 11));
        Locations.Add (new Vector2 (13, 11));
        Locations.Add (new Vector2 (16, 11));
        Locations.Add (new Vector2 (22, 11));
        Locations.Add (new Vector2 (27, 11));

        // Row 12
        Locations.Add (new Vector2 (2, 12));
        Locations.Add (new Vector2 (7, 12));
        Locations.Add (new Vector2 (13, 12));
        Locations.Add (new Vector2 (16, 12));
        Locations.Add (new Vector2 (22, 12));
        Locations.Add (new Vector2 (27, 12));

        // Row 13
        Locations.Add (new Vector2(2, 13));
        Locations.Add (new Vector2(3, 13));
        Locations.Add (new Vector2(4, 13));
        Locations.Add (new Vector2(5, 13));
        Locations.Add (new Vector2(6, 13));
        Locations.Add (new Vector2(7, 13));
        Locations.Add (new Vector2(8, 13));
        Locations.Add (new Vector2(9, 13));
        Locations.Add (new Vector2(10, 13));
        Locations.Add (new Vector2(11, 13));
        Locations.Add (new Vector2(12, 13));
        Locations.Add (new Vector2(13, 13));
        Locations.Add (new Vector2(16, 13));
        Locations.Add (new Vector2(17, 13));
        Locations.Add (new Vector2(18, 13));
        Locations.Add (new Vector2(19, 13));
        Locations.Add (new Vector2(20, 13));
        Locations.Add (new Vector2(21, 13));
        Locations.Add (new Vector2(22, 13));
        Locations.Add (new Vector2(23, 13));
        Locations.Add (new Vector2(24, 13));
        Locations.Add (new Vector2(25, 13));
        Locations.Add (new Vector2(26, 13));
        Locations.Add (new Vector2(27, 13));

        // Row 14
        Locations.Add (new Vector2 (7, 14));
        Locations.Add (new Vector2 (10, 14));
        Locations.Add (new Vector2 (19, 14));
        Locations.Add (new Vector2 (22, 14));

        // Row 15
        Locations.Add (new Vector2 (7, 15));
        Locations.Add (new Vector2 (10, 15));
        Locations.Add (new Vector2 (19, 15));
        Locations.Add (new Vector2 (22, 15));

        // Row 16
        Locations.Add (new Vector2 (7, 16));
        Locations.Add (new Vector2 (10, 16));
        Locations.Add (new Vector2 (11, 16));
        Locations.Add (new Vector2 (12, 16));
        Locations.Add (new Vector2 (13, 16));
        Locations.Add (new Vector2 (14, 16));
        Locations.Add (new Vector2 (15, 16));
        Locations.Add (new Vector2 (16, 16));
        Locations.Add (new Vector2 (17, 16));
        Locations.Add (new Vector2 (18, 16));
        Locations.Add (new Vector2 (19, 16));
        Locations.Add (new Vector2 (22, 16));

        // Row 17
        Locations.Add (new Vector2 (7, 17));
        Locations.Add (new Vector2 (10, 17));
        Locations.Add (new Vector2 (19, 17));
        Locations.Add (new Vector2 (22, 17));

        // Row 18
        Locations.Add (new Vector2 (7, 18));
        Locations.Add (new Vector2 (10, 18));
        Locations.Add (new Vector2 (19, 18));
        Locations.Add (new Vector2 (22, 18));

        // Row 19
        Locations.Add (new Vector2 (1, 19));
        Locations.Add (new Vector2 (2, 19));
        Locations.Add (new Vector2 (3, 19));
        Locations.Add (new Vector2 (4, 19));
        Locations.Add (new Vector2 (5, 19));
        Locations.Add (new Vector2 (6, 19));
        Locations.Add (new Vector2 (7, 19));
        Locations.Add (new Vector2 (8, 19));
        Locations.Add (new Vector2 (9, 19));
        Locations.Add (new Vector2 (10, 19));
        Locations.Add (new Vector2 (19, 19));
        Locations.Add (new Vector2 (20, 19));
        Locations.Add (new Vector2 (21, 19));
        Locations.Add (new Vector2 (22, 19));
        Locations.Add (new Vector2 (23, 19));
        Locations.Add (new Vector2 (24, 19));
        Locations.Add (new Vector2 (25, 19));
        Locations.Add (new Vector2 (26, 19));
        Locations.Add (new Vector2 (27, 19));
        Locations.Add (new Vector2 (28, 19));

        return Locations;
    }

    /// <summary>
    /// A list of all of the special locations for the Ghosts
    /// </summary>
    /// <returns>The locations.</returns>
    public List<Vector2> SpecialLocations()
    {
        List<Vector2> Locations = new List<Vector2> ();
        Locations.Add (new Vector2 (16, 10));
        Locations.Add (new Vector2 (13, 10));
        Locations.Add (new Vector2 (16, 22));
        Locations.Add (new Vector2 (13, 22));

        return Locations;
    }

    /// <summary>
    /// A list of all of the intersections on the maze.
    /// </summary>
    public List<Vector2> Intersections()
    {
        List<Vector2> Locations = new List<Vector2> ();

        Locations.Add (new Vector2 (16, 4));
        Locations.Add (new Vector2 (13, 4));
        Locations.Add (new Vector2 (4, 7));
        Locations.Add (new Vector2 (25, 7));
        Locations.Add (new Vector2 (7, 10));
        Locations.Add (new Vector2 (10, 10));
        Locations.Add (new Vector2 (19, 10));
        Locations.Add (new Vector2 (22, 10));
        Locations.Add (new Vector2 (7, 13));
        Locations.Add (new Vector2 (10, 13));
        Locations.Add (new Vector2 (19, 13));
        Locations.Add (new Vector2 (22, 13));
        Locations.Add (new Vector2 (10, 16));
        Locations.Add (new Vector2 (19, 16));
        Locations.Add (new Vector2 (7, 19));
        Locations.Add (new Vector2 (10, 19));
        Locations.Add (new Vector2 (19, 19));
        Locations.Add (new Vector2 (22, 19));
        Locations.Add (new Vector2 (7, 25));
        Locations.Add (new Vector2 (22, 25));
        Locations.Add (new Vector2 (2, 28));
        Locations.Add (new Vector2 (7, 28));
        Locations.Add (new Vector2 (10, 28));
        Locations.Add (new Vector2 (13, 28));
        Locations.Add (new Vector2 (16, 28));
        Locations.Add (new Vector2 (19, 28));
        Locations.Add (new Vector2 (22, 28));
        Locations.Add (new Vector2 (27, 28));
        Locations.Add (new Vector2 (7, 32));
        Locations.Add (new Vector2 (22, 32));

        return Locations;
    }
}
