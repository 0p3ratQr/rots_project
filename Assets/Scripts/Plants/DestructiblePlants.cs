using System;
using UnityEngine;

public class DestructiblePlants : MonoBehaviour
{
    public event EventHandler OnDestructibleTakeDamage;
    
    public void TakeDamage()
    {
        OnDestructibleTakeDamage?.Invoke(this, EventArgs.Empty);
        Destroy(gameObject);
        NavMeshSurfaceManagement.Instance.RebakeNavMeshSurface();
    }
}
