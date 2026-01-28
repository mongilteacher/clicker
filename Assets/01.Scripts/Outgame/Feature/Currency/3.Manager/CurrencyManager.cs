using System;
using UnityEngine;

// 오직 데이터(재화)를 "관리"하는 클래스입니다.
// 클린 아키텍처에서는 "서비스"라는 이름을 쓴다. (그러나 게임에서는 보통 "매니저"라고 표현한다.)
public class CurrencyManager : MonoBehaviour
{
   public static CurrencyManager Instance;
   // CRUD
   //  관리란: "데이터에 대한 생성 / 조회 / 사용 / 소모 / 이벤트 등" 로직
   //         ㄴ 비즈니스 로직(게임 로직): 데이터 사용에 대한 핵심 규칙

   // 이벤트
   public static event Action OnDataChanged;

   
   // 재화 데이터들 (배열로 관리)
   private double[] _currencies = new double[(int)ECurrencyType.Count];
   
   // 저장소
   // 의존이란 한 객체가 동작하기 위해서 다른 객체를 참조하는것을
   // DIP: 구현체에 의존하지 말고 약속에 의존해라 
   private ICurrencyRepository _repository;
   
   private void Awake()
   {
      Instance = this;

      _repository = new LocalCurrencyRepository();
   }

   private void Start()
   {
      _currencies = _repository.Load().Currencies;
   }
   
   
   // 0. 재화 조회
   public double Get(ECurrencyType currencyType)
   {
      return _currencies[(int)currencyType];
   }
   
   // - 어쩔수 없는 재화 조회 편의 기능... ㅠㅠ
   
   public double Gold => Get(ECurrencyType.Gold);
   public double Ruby => Get(ECurrencyType.Ruby);
   public double Jelly => Get(ECurrencyType.Jelly);
   
   
   // 1. 재화 추가
   public void Add(ECurrencyType type, double amount)
   {
      _currencies[(int)type] += amount;

      _repository.Save(new CurrencySaveData()
      {
         Currencies = _currencies
      });
      
      OnDataChanged?.Invoke();
   }
   
   // 2. 재화 소모
   public bool TrySpend(ECurrencyType type, double amount)
   {
      if (_currencies[(int)type] >= amount)
      {
         _currencies[(int)type] -= amount;

         _repository.Save(new CurrencySaveData()
         {
            Currencies = _currencies
         });
         
         OnDataChanged?.Invoke();

         return true;
      }

      return false;
   }
   
   // 3. 돈 있으세요? 
   public bool CanAfford(ECurrencyType type, double amount)
   {
      return _currencies[(int)type] >= amount;
   }
   
   private void Save()
   {
      // 0. 도대체 관리라는 책임이 어디까지야? 
      // 저장하는 방식
      // 1. PlayerPrefs + double->string
      // 2. PlayerPrefs + double->json
      // 3. CSV /Json으로 저장해주세요.
      // 4. 서버에 저장합신다. // DB에저장합시다.
      // 5. 플랫폼에 따라 다르게 저장: 유니티에서는 3번,, 빌드하고나면 4번으로 저장되게 해주세요..
      // 6. Save 호출하면 Save가 더이상 호출되지 않은지 0.6초가 지나면 세이브되게...
   }

  
}
