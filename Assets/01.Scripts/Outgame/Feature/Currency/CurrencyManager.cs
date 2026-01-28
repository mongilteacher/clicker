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
   
   
   private void Awake()
   {
      Instance = this;
   }


   private void Start()
   {
      Load();
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

      Save();
      
      OnDataChanged?.Invoke();
   }
   
   // 2. 재화 소모
   public bool TrySpend(ECurrencyType type, double amount)
   {
      if (_currencies[(int)type] >= amount)
      {
         _currencies[(int)type] -= amount;

         Save();
         
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
      // 1. PlayerPrefs + double->string
      // 2. PlayerPrefs + double->json
      
      for (int i = 0; i < (int)ECurrencyType.Count; i++)
      {
         var type = (ECurrencyType)i;
         PlayerPrefs.SetString(type.ToString(), _currencies[i].ToString("G17"));
      }
   }

   private void Load()
   {
      for (int i = 0; i < (int)ECurrencyType.Count; i++)
      {
         if (PlayerPrefs.HasKey(i.ToString()))
         {
            _currencies[i] = double.Parse(PlayerPrefs.GetString(i.ToString(), "0")); 
         }
      }
   }
}
