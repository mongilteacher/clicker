using System;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager Instance { get; private set; }
    public static event Action OnDataChanged ;

    [SerializeField] private UpgradeSpecTableSO _specTable;
    
    private Dictionary<EUpgradeType, Upgrade> _upgrades = new ();
    
    private void Awake()
    {
        Instance = this;

        // 스펙 데이터에 따라 도메인 생성
        foreach (var specData in _specTable.Datas)
        {
            if (_upgrades.ContainsKey(specData.Type))
            {
                throw new Exception($"There is already an upgrade with type {specData.Type}");
            }
            
            _upgrades.Add(specData.Type, new Upgrade(specData));
        }
        
        OnDataChanged?.Invoke();
    }
}