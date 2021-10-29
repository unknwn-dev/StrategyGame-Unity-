using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PathFinder {
    protected static int GetEstimatedPathCost(Vector3Int startPosition, Vector3Int targetPosition) {
        return Mathf.Max(Mathf.Max(Mathf.Abs(startPosition.x - targetPosition.x), Mathf.Abs(startPosition.y - targetPosition.y), Mathf.Abs(startPosition.z - targetPosition.z)));
    }

    public static List<Vector3Int> Find(Cell from, Cell to) {
        List<Cell> opened = new List<Cell>();
        List<Cell> closed = new List<Cell>();

        Cell currentCell = from;

        currentCell.g = 0;
        currentCell.h = GetEstimatedPathCost(from.CellPos, to.CellPos);

        opened.Add(currentCell);

        while (opened.Count != 0) {
            // Sorting the open list to get the tile with the lowest F.
            opened = opened.OrderBy(x => x.F).ThenByDescending(x => x.g).ToList();
            currentCell = opened[0];

            // Removing the current tile from the open list and adding it to the closed list.
            opened.Remove(currentCell);
            closed.Add(currentCell);

            int g = currentCell.g + 1;

            // If there is a target tile in the closed list, we have found a path.
            if (closed.Contains(to)) {
                break;
            }

            // Investigating each adjacent tile of the current tile.
            foreach (Cell adjacentTile in currentCell.GetNeighborCells()) {

                // Ignore not walkable adjacent tiles.
                if (!adjacentTile.IsGround) {
                    continue;
                }

                // Ignore the tile if it's already in the closed list.
                if (closed.Contains(adjacentTile)) {
                    continue;
                }

                // If it's not in the open list - add it and compute G and H.
                if (!(opened.Contains(adjacentTile))) {
                    adjacentTile.g = g;
                    adjacentTile.h = GetEstimatedPathCost(adjacentTile.CellPos, to.CellPos);
                    opened.Add(adjacentTile);
                }
                // Otherwise check if using current G we can get a lower value of F, if so update it's value.
                else if (adjacentTile.F > g + adjacentTile.h) {
                    adjacentTile.g = g;
                }
            }
        }

        List<Cell> finalPathTiles = new List<Cell>();
        // Backtracking - setting the final path.
        if (closed.Contains(to)) {
            currentCell = to;
            finalPathTiles.Add(currentCell);

            for (int i = to.g - 1; i >= 0; i--) {
                currentCell = closed.Find(x => x.g == i && currentCell.GetNeighborCells().Contains(x));
                finalPathTiles.Add(currentCell);
            }

            finalPathTiles.Reverse();
        }

        List<Vector3Int> pathPoses = new List<Vector3Int>();

        foreach(var c in finalPathTiles) {
            pathPoses.Add(c.CellPos);
        }

        pathPoses.Remove(pathPoses[0]);

        return pathPoses;
    }
}
