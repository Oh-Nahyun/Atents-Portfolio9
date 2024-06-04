using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.AI.Navigation;
using UnityEngine;

[RequireComponent(typeof(MazeVisualizer))]
[RequireComponent(typeof(NavMeshSurface))]
public class MazeGenerator : MonoBehaviour
{
    public int seed = -1;

    public enum MazeAlgorithm
    {
        RecursiveBackTracking = 0,
        Eller,
        Wilson
    }

    public MazeAlgorithm mazeAlgorithm = MazeAlgorithm.Wilson;

    MazeVisualizer visualizer;
    NavMeshSurface navMeshSurface;
    AsyncOperation navAsync;

    private void Awake()
    {
        visualizer = GetComponent<MazeVisualizer>();
        navMeshSurface = GetComponent<NavMeshSurface>();
    }

    private void Update()
    {
        Debug.Log("Update");
    }

    public void Generate(int width, int height)
    {
        Maze maze = null;
        switch (mazeAlgorithm)
        {
            case MazeAlgorithm.RecursiveBackTracking:
                maze = new BackTracking();
                break;
            case MazeAlgorithm.Eller:
                maze = new Eller();
                break;
            case MazeAlgorithm.Wilson:
                maze = new Wilson();
                break;
        }

        maze.MakeMaze(width, height, seed);
        
        visualizer.Clear();
        visualizer.Draw(maze);

        StartCoroutine(UpdateSurface());
    }

    IEnumerator UpdateSurface()
    {
        navAsync = navMeshSurface.UpdateNavMesh(navMeshSurface.navMeshData);
        while (!navAsync.isDone)
        {
            yield return null; // 매 프레임 기다리기
        }
        Debug.Log("Nav Surtace Updated!");
    }
}
