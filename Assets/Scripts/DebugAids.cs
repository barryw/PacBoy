using UnityEngine;

public class DebugAids
{
    /// <summary>
    /// Draw the famous PacMan debug grid. PacMan is played on a 36x28 grid.
    /// </summary>
    public static void ShowGrid()
    {
        for (var x = 0; x < 28; x++)
        {
            for (var y = 0; y < 36; y++)
            {
                Debug.DrawLine (new Vector2 (x, y), new Vector2 (x + 1f, y), Color.white, 0, false);
                Debug.DrawLine (new Vector2 (x, y), new Vector2 (x, y + 1), Color.white, 0, false);
                Debug.DrawLine (new Vector2 (x, y + 1), new Vector2 (x + 1, y + 1), Color.white, 0, false);
                Debug.DrawLine (new Vector2 (x + 1, y + 1), new Vector2 (x + 1, y), Color.white, 0, false);
            }   
        }    
    }
    
    /// <summary>
    /// Draw colored boxes - one for each ghost - showing what that ghost is targeting
    /// </summary>
    public static void ShowTarget(BaseActor actor, Vector2 target, Color color)
    {
        Debug.DrawLine(actor.TileCenter, target, color);
        Debug.DrawLine (new Vector3 (target.x - .5f, target.y - .5f), new Vector3 (target.x + .5f, target.y - .5f), color, 0, false);
        Debug.DrawLine (new Vector3 (target.x - .5f, target.y - .5f), new Vector3 (target.x - .5f, target.y + .5f), color, 0, false);
        Debug.DrawLine (new Vector3 (target.x - .5f, target.y + .5f), new Vector3 (target.x + .5f, target.y + .5f), color, 0, false);
        Debug.DrawLine (new Vector3 (target.x + .5f, target.y - .5f), new Vector3 (target.x + .5f, target.y + .5f), color, 0, false);
    }
}