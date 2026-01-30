using System;
using UnityEngine;


// 매니저의 역할:
// 1. 도메인 관리 : 생성/조회/수정/삭제와 같은 비즈니스 로직 
// 2. 외부와의 소통 창구
public class AccountManager : MonoBehaviour
{
    public AccountManager Instance { get; private set; }

    private Account _currentAccount = null;
    public bool IsLogin => _currentAccount != null;
    public string Email => _currentAccount?.Email ?? string.Empty;
    

    private void Awake()
    {
        Instance = this;
    }


    public bool TryLogin(string email, string password)
    {
        Account account = null;
        
        try
        {
            account = new Account(email, password);
        }
        catch(Exception ex)
        {
            // 1. 유효성 검증 통과 못하면 실패
            return false;
        }
        
        // 2. 가입한적 없다면 실패!
        if (!PlayerPrefs.HasKey(email))
        {
            // _messageTextUI.text = "아이디/비밀번호를 확인해주세요.";
            return false;
        }
        
        // 3. 비밀번호 틀렸다면 실패.
        string myPassword = PlayerPrefs.GetString(email);
        if (myPassword != account.Password)
        {
            //_messageTextUI.text = "아이디/비밀번호를 확인해주세요.";
            return false;
        }
        
        _currentAccount = account;

        return true;
    }

    public bool TryRegister(string email, string password)
    {
        // 1. 이메일 중복검사
        if (PlayerPrefs.HasKey(email))
        {
            // _messageTextUI.text = "중복된 아이디입니다.";
            return false;
        }
        
        try
        {
            Account account = new Account(email, password);
        }
        catch(Exception ex)
        {
            // 2. 유효성 검증 통과 못하면 실패
            return false;
        }
        
        // 3. 성공하면 저장!
        PlayerPrefs.SetString(email, password);

        
        return true;
    }

    public void Logout()
    {
        
    }
}