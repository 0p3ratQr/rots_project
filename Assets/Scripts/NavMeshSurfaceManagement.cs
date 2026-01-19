using NavMeshPlus.Components;
using UnityEngine;

[RequireComponent(typeof(NavMeshSurface))]
public class NavMeshSurfaceManagement : MonoBehaviour
{

    public static NavMeshSurfaceManagement Instance { get; private set;}
    private NavMeshSurface _navmeshSurface;


    private void Awake()
    {
        Instance = this;
        _navmeshSurface = GetComponent<NavMeshSurface>();
        _navmeshSurface.hideEditorLogs = true;
    }
    public void RebakeNavMeshSurface()
    {
        _navmeshSurface.BuildNavMesh(); // Перезапекаем сетку
    }
}
