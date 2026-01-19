using UnityEngine;

public class ActiveWeapon : MonoBehaviour
{

     public static ActiveWeapon Instance { get; private set; }
     [SerializeField] private Sword sword;

    private void Update()
    {
        if (Player.Instance.IsAlive())
            FollowMousePosition();
    }

    private void Start() //Здесь чел писал Awake, но я поменял на Start чтобы гарантировать что Player уже инициализирован,  так как вылетала ошибка null reference exception
     {
         Instance = this;
     }
     public Sword GetActiveWeapon()
     {
         return sword;
     }


         private void FollowMousePosition()
    {
        Vector3 mousePos = GameInput.Instance.GetMousePosition();
        Vector3 playerScreenPosition = Player.Instance.GetPlayerPosition();

        if (mousePos.x < playerScreenPosition.x)
        {
         transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else
        {
         transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }
}
