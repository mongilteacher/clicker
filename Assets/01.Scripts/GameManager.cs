using UnityEngine;

// 게임 매니저라는 갓 클래스 (모든 데이터가 있다.)
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    // 클리커게임의 숫자 단위는 억,조,경,해 ~~~~ 넘어가요.
    //                영어로  K, M, G, A, B, AA, TT ~~~
    public double ManualDamage = 238473234233434223239874d;          // 21억까지
       
    // int(21억), long(경), biginteger(숫자를 쪼개 문자로관리하면서 연산하니까 매우 느립니다.)
    // float , double , decimal (부동소수점) 같은 자료형 크기 대비 범위가 엄청나게큽니다.
    
    // float   10^38     (여러분들 컵정도의 크기) 정밀도: 6자리
    // double  10^900    (지구정도의 크기)       정밀도가 15자리인가되요.
    // decimal 10^283904 (은하계정도의 크기)      
    
  

    public double AutoDamage = 2233438473234233434223239874d;
    public double Gold; 
    
    private void Awake()
    {
        Instance = this;
    }
    

}
