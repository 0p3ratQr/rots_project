using UnityEngine;

public class DestructiblePlantVisual : MonoBehaviour
{
    [SerializeField] private DestructiblePlants _destructiblePlant;
    [SerializeField] private GameObject _bushDeathVFX;


    private void Start()
    {
            _destructiblePlant.OnDestructibleTakeDamage += DestructiblePlants_OnDestructibleTakeDamage;    
    }

    
    private void DestructiblePlants_OnDestructibleTakeDamage(object sender, System.EventArgs e)
    {
        ShowDeathVFX();
    }


    private void ShowDeathVFX()
    {
        Instantiate(_bushDeathVFX, _destructiblePlant.transform.position, Quaternion.identity);
    }

    private void OnDestroy()
    {
        _destructiblePlant.OnDestructibleTakeDamage -= DestructiblePlants_OnDestructibleTakeDamage;
    }
}
