using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Account_Manager : MonoBehaviour     //계정 매니저. 계정 클래스와 리스트, 로그인, 회원가입, 현재 계정, 계정 정보 저장, 로드를 관할합니다.
{
    public InputField Login_ID_InputField;
    public InputField Login_PW_InputField;

    public InputField Signup_ID_InputField;
    public InputField Signup_Name_InputField;
    public InputField Signup_PW_InputField;
    public InputField Signup_PWCF_InputField;

    public account current_account = null;

    private static Account_Manager instance;
    public static Account_Manager Instance        //싱글톤화. 다른 스크립트에서 쉽게 접근 가능
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<Account_Manager>();

                if (instance == null)
                {
                    GameObject singletonObject = new GameObject("Account_Manager");
                    instance = singletonObject.AddComponent<Account_Manager>();
                }
            }

            return instance;
        }
    }

    public class account
    {
        // ID를 저장하는 변수
        public string ID;

        // Name을 저장하는 변수
        public string Name;

        // 패스워드를 저장하는 변수
        public string PW;

        // 소지 잔금을 저장하는 변수
        public int account_Money = 50000;

        // 로그인 상태를 저장하는 변수
        public bool isLog_in = false;

        // 생성자
        public account(string id, string name, string pw)
        {
            ID = id;
            Name = name;
            PW = pw;
        }
    }

    public List<account> account_list = new List<account>();


    // 새 account 클래스 인스턴스를 생성하는 메서드
    public account CreateAccount(string id, string name, string pw)     
    {
        account newAccount = new account(id, name, pw);
        account_list.Add(newAccount); //만든 계정을 리스트에 새로 추가함
        return newAccount;
    }

    private void Awake() //어웨이크할 때, 기본 계정을 가지고 로그인된 상태로 시작합니다.
    {
        LoadAccountList(); //계정 로드

        // 계정이 없으면 기본 계정 생성
        if (account_list.Count == 0)
        {
            current_account = CreateAccount("jhwoo", "정현우", "1234");
            current_account.isLog_in = true;
        }
        PrintAccountList();


    }

    public void Log_in() //로그인 메서드
    {
        // 1. account_list에서 Login_ID_InputField.text와 같은 ID값을 가진 account를 찾는다.
        account foundAccount = account_list.Find(acc => acc.ID == Login_ID_InputField.text);

        // 2. Login_ID_InputField.text와 같은 ID값을 가진 account가 있을 시,
        if (foundAccount != null)
        {
            // 해당 account의 PW값이 InputField Login_PW_InputField.text와 일치하는지 체크한다.
            if (foundAccount.PW == Login_PW_InputField.text)
            {
                // 3. 해당 account의 ID와 PW값이 모두 입력된 값과 일치할 경우, 해당 account의 isLog_in을 true로 만든다.
                foundAccount.isLog_in = true;
                current_account = foundAccount;

                Debug.Log("로그인 성공!");
                General_Manager.Instance.CurrentState_Manager = "ATM_lobby";
                Login_PW_InputField.text = null;
                General_Manager.Instance.Error("로그인 성공!");
            }
            else
            {
                Debug.Log("잘못된 비밀번호");
                General_Manager.Instance.Error("잘못된 비밀번호입니다.");
                Login_PW_InputField.text = null;
            }
        }
        else
        {
            Debug.Log("해당 아이디의 계정을 찾을 수 없습니다.");
            General_Manager.Instance.Error("계정을 찾을 수 없습니다.");
            Login_ID_InputField.text = null;
            Login_PW_InputField.text = null;
        }
    }

    public void Log_out()  //로그아웃 메서드
    {
        current_account.isLog_in = false;
        current_account = null;
    }

    public void Sign_up()
    {
        // 1. Signup_ID_InputField.text가 다음의 조건을 만족하는지 체크.
        // ID는 영어와 숫자로만 구성되어야 하고, 3글자 이상 ~ 10글자 이하
        // account_list에 입력된 ID 값과 동일한 값을 가진 account가 없어야함.
        string signupID = Signup_ID_InputField.text;
        if (System.Text.RegularExpressions.Regex.IsMatch(signupID, "^[a-zA-Z0-9]{3,10}$"))
        {
            if (account_list.Find(acc => acc.ID == signupID) == null)
            {
                // 2. Signup_Name_InputField.text가 다음의 조건을 만족하는지 체크.
                // 2글자 이상, 5글자 이하
                string signupName = Signup_Name_InputField.text;
                if (signupName.Length >= 2 && signupName.Length <= 5)
                {
                    // 3. Signup_PW_InputField.text가 다음의 조건을 만족하는지 체크.
                    // 영어와 숫자로만 구성되어야 하고, 5글자 이상 ~ 15글자 이하
                    string signupPW = Signup_PW_InputField.text;
                    if (System.Text.RegularExpressions.Regex.IsMatch(signupPW, "^[a-zA-Z0-9]{5,15}$"))
                    {
                        // 4. Signup_PWCF_InputField.text가 Signup_PW_InputField.text와 같은 값인지 체크.
                        if (signupPW == Signup_PWCF_InputField.text)
                        {
                            // 5. 위의 모든 조건을 만족한다면, 새 계정을 생성.
                            CreateAccount(signupID, signupName, signupPW);
                            SaveAccountList(); //계정 저장

                            Debug.Log("회원가입 성공!");
                            General_Manager.Instance.CurrentState_Manager = "log_in_ing";

                            Signup_ID_InputField.text = null;
                            Signup_Name_InputField.text = null;
                            Signup_PW_InputField.text = null;
                            Signup_PWCF_InputField.text = null;
                            General_Manager.Instance.Error("회원가입 성공!");
                        }
                        else
                        {
                            Debug.Log("비밀번호가 일치하지 않습니다.");
                            General_Manager.Instance.Error("비밀번호가 틀립니다.");
                            Signup_PW_InputField.text = null;
                            Signup_PWCF_InputField.text = null;
                        }
                    }
                    else
                    {
                        Debug.Log("잘못된 비밀번호 양식");
                        General_Manager.Instance.Error("잘못된 비밀번호입니다.");
                        Signup_PW_InputField.text = null;
                        Signup_PWCF_InputField.text = null;
                    }
                }
                else
                {
                    Debug.Log("잘못된 이름 양식");
                    General_Manager.Instance.Error("잘못된 이름 양식입니다.");
                    Signup_Name_InputField.text = null;
                }
            }
            else
            {
                Debug.Log("이미 존재하는 ID");
                General_Manager.Instance.Error("이미 존재하는 ID입니다.");
                Signup_ID_InputField.text = null;
            }
        }
        else
        {
            Debug.Log("잘못된 ID 양식");
            General_Manager.Instance.Error("잘못된 ID 양식입니다.");
            Signup_ID_InputField.text = null;
        }
    }

    void LoadAccountList()
    {
        // 저장된 데이터 불러오기
        string json = PlayerPrefs.GetString("account_list", "");
        if (!string.IsNullOrEmpty(json))
        {
            // Json을 리스트로 변환
            account_list = JsonUtility.FromJson<List<account>>(json).ToList();
        }
    }

    void SaveAccountList()
    {
        // 리스트를 Json으로 변환하여 저장
        string json = JsonUtility.ToJson(account_list);
        PlayerPrefs.SetString("account_list", json);
        PlayerPrefs.Save();
        Debug.Log("계정 저장 완료");
    }

    void PrintAccountList()
    {
        // 모든 계정 정보 출력
        foreach (var account in account_list)
        {
            Debug.Log($"Username: {account.Name}");
        }
    }
}
