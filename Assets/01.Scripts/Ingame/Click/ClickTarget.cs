using UnityEngine;

public class ClickTarget : MonoBehaviour, Clickable
{
   [SerializeField] private string _name;


   public bool OnClick(ClickInfo clickInfo)
   {
      Debug.Log($"{_name}: 다음부터는 늦지 않겠습니다.");

      return true;
   }
}
