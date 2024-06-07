using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_07_DynamicNavSurface : TestBase
{
    [Header("셀")]
    public Direction pathData;
    public CellVisualizer cellVisualizer;

    [Header("미로")]
    public int width = 5;
    public int height = 5;
    public MazeVisualizer backtracking;
    public MazeVisualizer eller;
    public MazeVisualizer wilson;

    public NavMeshSurface surface;
    AsyncOperation navAsync;

    public MazeGenerator generator;

    protected override void OnTest1(InputAction.CallbackContext context)
    {
        backtracking.Clear();
        BackTracking mazeBackTracking = new BackTracking();
        mazeBackTracking.MakeMaze(width, height, seed);
        backtracking.Draw(mazeBackTracking);

        eller.Clear();
        Eller mazeEller = new Eller();
        mazeEller.MakeMaze(width, height, seed);
        eller.Draw(mazeEller);

        wilson.Clear();
        Wilson mazeWilson = new Wilson();
        mazeWilson.MakeMaze(width, height, seed);
        wilson.Draw(mazeWilson);
    }

    protected override void OnTest2(InputAction.CallbackContext context)
    {
        StartCoroutine(UpdateSurface());
    }

    protected override void OnTest3(InputAction.CallbackContext context)
    {
        generator.Generate(width, height);
    }

    IEnumerator UpdateSurface()
    {
        navAsync = surface.UpdateNavMesh(surface.navMeshData);
        while (!navAsync.isDone)
        {
            yield return null; // 매 프레임 기다리기
        }
        Debug.Log("Nav Surtace Updated!");
    }
}
