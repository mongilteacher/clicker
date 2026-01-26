using UnityEngine;

public class ClickTarget : MonoBehaviour
{
    // 목적: 타겟을 클릭하면 클릭하고 싶다.
    public LayerMask ClickLayer;
    
    private void Update()
    {
        // 1. 마우스 클릭을 감지한다.
        if (Input.GetMouseButtonDown(0))
        {        
            // 2. 마우스 좌표를 구한다.
            // 마우스 좌표계는 스크린 좌표계
            Vector2 mousePos = Input.mousePosition;
            
            // 마우스의 스크린 좌표계를 월드 좌표계로 바꿔줄 필요가 있다.
            Vector2 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
            
            // 3. 마우스 좌표로 가상의 레이저를 쏴서 그 레이저가 클릭타겟과 충돌했는지 체크
            RaycastHit2D hit = Physics2D.Raycast(worldPos, Vector2.zero, 0f, ClickLayer);
            if (hit == true)
            {
                Debug.Log("다음부터는 늦지 않겠습니다.");
            }
            
            // 4. 게임 오브젝트 설정
            // - ClickTarget이라는 Layer 추가
            // - Layer 설정 (인스펙터)
        }

        
        
    }
}
