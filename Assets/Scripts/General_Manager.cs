using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class General_Manager : MonoBehaviour          //총괄 매니저. '현재 상태'를 총괄하고 현재 상테에 따라 전체 UI들의 SetActive를 통제합니다. 그리고 버튼들이 클릭 될 시 호출되는 메서드를 포함하고 있습니다. 그리고 기타 매니저들이 다루지 않는 잡다한 기능과 설정을 처리합니다.
{
    public GameObject Title_Text_Obj;  //타이틀
    public GameObject Current_Cash_Obj; //현재 금액
    public GameObject Current_Account_Obj; //현재 계정
    public GameObject ATM_Lobby_Obj; //ATM 로비
    public GameObject Manage_Money_Share_Unit_Obj; //입금 출금 송금(단위 버튼)
    public GameObject Manage_Money_Share_Public__Obj; //입금 출금 송금(금액 입력/뒤로가기)
    public GameObject Deposit_Cash_Obj; //입금
    public GameObject Withdraw_Money_Obj; //출금
    public GameObject Send_Money_Obj; //송금
    public GameObject Login_Obj; //로그인
    public GameObject Signup_Obj; //회원가입
    public GameObject Error_Obj; //에러

    public InputField Add_Money_InputField;
    public InputField Send_Money_Target_ID_InputField;

    public InputField Login_ID_InputField;
    public InputField Login_PW_InputField;

    public InputField Signup_ID_InputField;
    public InputField Signup_Name_InputField;
    public InputField Signup_PW_InputField; 
    public InputField Signup_PWCF_InputField;

    public Text Current_Cash_Number; //보유 현금의 텍스트
    public Text Current_Account_Name_Text; //계정 이름의 텍스트
    public Text Current_Account_Money_Number; //계정 보유 현금의 텍스트
    public Text Error_Text; //에러 메세지 창의 텍스트


    private static General_Manager instance;
    public static General_Manager Instance        //싱글톤화. 다른 스크립트에서 쉽게 접근 가능
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<General_Manager>();

                if (instance == null)
                {
                    GameObject singletonObject = new GameObject("General_Manager");
                    instance = singletonObject.AddComponent<General_Manager>();
                }
            }

            return instance;
        }
    }

    public String current_State; //현재 상태를 저장하는 실제 변수
    String past_State; //과거 상태를 저장하는 변수

    public event Action<string> OnStateChanged;
    private void HandleStateChanged(string newState)  //상태가 변경될 시 이벤트를 통해 자동으로 호출되는 메서드입니다
    {
        /* current_State 목록

        1. "log_in_ing"

        2. "sign_up_ing"

        3. "ATM_lobby"

        4. "Deposit_Cash_Ing"

        5. "Withdraw_Account_Money_ing"

        6. "Send_Account_Money_ing"

        7. "Error_ing"

        */

        Debug.Log("상태가 변경되었습니다: " + newState);

        Add_Money_InputField.text = null;
        if (Account_Manager.Instance.current_account != null && Account_Manager.Instance.current_account.isLog_in ==true) 
        {
            Current_Cash_Number.text = ATM_Manager.Instance.cash.ToString("C0");
            Current_Account_Name_Text.text = Account_Manager.Instance.current_account.Name;
            Current_Account_Money_Number.text = Account_Manager.Instance.current_account.account_Money.ToString("C0");
        }
        else
        {
            Current_Cash_Number.text = null;
            Current_Account_Name_Text.text = null;
            Current_Account_Money_Number.text = null;
        }

        switch (newState)
        {
            case "log_in_ing":
                Title_Text_Obj.gameObject.SetActive(true);
                Login_Obj.gameObject.SetActive(true);

                Signup_Obj.gameObject.SetActive(false);
                ATM_Lobby_Obj.gameObject.SetActive(false);
                Current_Account_Obj.gameObject.SetActive(false);
                Current_Cash_Obj.gameObject.SetActive(false);
                //
                break;

            case "sign_up_ing":
                Signup_Obj.gameObject.SetActive(true);

                Login_Obj.gameObject.SetActive(false);
                ATM_Lobby_Obj.gameObject.SetActive(false);
                Current_Account_Obj.gameObject.SetActive(false);
                Current_Cash_Obj.gameObject.SetActive(false);
                //
                break;

            case "ATM_lobby":
                Current_Cash_Obj.gameObject.SetActive(true);
                Current_Account_Obj.gameObject.SetActive(true);
                ATM_Lobby_Obj.gameObject.SetActive(true);

                Login_Obj.gameObject.SetActive(false);
                Signup_Obj.gameObject.SetActive(false);
                Manage_Money_Share_Unit_Obj.gameObject.SetActive(false);
                Manage_Money_Share_Public__Obj.gameObject.SetActive(false);
                Deposit_Cash_Obj.gameObject.SetActive(false);
                Withdraw_Money_Obj.gameObject.SetActive(false);
                Send_Money_Obj.gameObject.SetActive(false);
                //
                break;

            case "Deposit_Cash_Ing":
                Manage_Money_Share_Unit_Obj.gameObject.SetActive(true);
                Manage_Money_Share_Public__Obj.gameObject.SetActive(true);
                Deposit_Cash_Obj.gameObject.SetActive(true);

                ATM_Lobby_Obj.gameObject.SetActive(false);
                //
                break;

            case "Withdraw_Account_Money_ing":
                Manage_Money_Share_Unit_Obj.gameObject.SetActive(true);
                Manage_Money_Share_Public__Obj.gameObject.SetActive(true);
                Withdraw_Money_Obj.gameObject.SetActive(true);

                ATM_Lobby_Obj.gameObject.SetActive(false);
                //
                break;

            case "Send_Account_Money_ing":
                Manage_Money_Share_Unit_Obj.gameObject.SetActive(false);
                Manage_Money_Share_Public__Obj.gameObject.SetActive(true);
                Send_Money_Obj.gameObject.SetActive(true);

                ATM_Lobby_Obj.gameObject.SetActive(false);
                //
                break;

            case "Error_ing":
                Error_Obj.gameObject.SetActive(true);
                //
                break;

            default:
                //
                Debug.LogWarning("Unexpected state: " + newState);
                break;
        }
    }


    public string CurrentState_Manager //현재 상태 값을 변경할 시 인보크를 발동하는 매니저 프로퍼티
    {
        set    //변경될 시
        {
            if (current_State != value)
            {
                current_State = value;
                OnStateChanged?.Invoke(current_State);
            }
        }
    }

















    private void Start()        // 시작 시 
    {
        OnStateChanged += HandleStateChanged; //이벤트에 메서드 등록
        Title_Text_Obj.gameObject.SetActive(true);
        if (Account_Manager.Instance.current_account != null && Account_Manager.Instance.current_account.isLog_in == true) //시작시 로그인된 계정이 있으면 ATM로비에서, 아닐 경우 로그인창에서 시작
        {
            CurrentState_Manager = "ATM_lobby";
        }
        else
        {
            CurrentState_Manager = "log_in_ing";
        }

        Login_PW_InputField.contentType = InputField.ContentType.Password;
        Signup_PW_InputField.contentType = InputField.ContentType.Password;
        Signup_PWCF_InputField.contentType = InputField.ContentType.Password;

        Add_Money_InputField.onValueChanged.AddListener(OnNumericInputChanged); //금액 입력 인풋필드에 숫자만 입력되도록 제어합니다.
    }






    //이 아래는 버튼 메서드입니다.
    public void Goto_Deposit_Cash_Button()
    {
        CurrentState_Manager = "Deposit_Cash_Ing";
    }
    public void Goto_Withdraw_Money_Button()
    {
        CurrentState_Manager = "Withdraw_Account_Money_ing";
    }

    public void Goto_Send_Money_Button()
    {
        CurrentState_Manager = "Send_Account_Money_ing";
    }

    public void Goto_Logout_Button()
    {
        Account_Manager.Instance.Log_out();
        CurrentState_Manager = "log_in_ing";
    }

    public void Add_10000_Button()
    {
        int inputValue;
        string inputText = Add_Money_InputField.text;

        // 입력값이 없으면 기본값으로 10000 설정
        if (string.IsNullOrEmpty(inputText))
        {
            inputValue = 10000;
        }
        else
        {
            // 정수화하고 10000을 더하기
            inputValue = int.Parse(inputText) + 10000;
        }

        // 결과를 다시 문자열로 변환하여 Add_Money_InputField.text에 저장
        Add_Money_InputField.text = inputValue.ToString();
    }

    public void Add_30000_Button()
    {
        int inputValue;
        string inputText = Add_Money_InputField.text;

        // 입력값이 없으면 기본값으로 30000 설정
        if (string.IsNullOrEmpty(inputText))
        {
            inputValue = 30000;
        }
        else
        {
            // 정수화하고 10000을 더하기
            inputValue = int.Parse(inputText) + 30000;
        }

        // 결과를 다시 문자열로 변환하여 Add_Money_InputField.text에 저장
        Add_Money_InputField.text = inputValue.ToString();
    }

    public void Add_50000_Button()
    {
        int inputValue;
        string inputText = Add_Money_InputField.text;

        // 입력값이 없으면 기본값으로 30000 설정
        if (string.IsNullOrEmpty(inputText))
        {
            inputValue = 50000;
        }
        else
        {
            // 정수화하고 10000을 더하기
            inputValue = int.Parse(inputText) + 50000;
        }

        // 결과를 다시 문자열로 변환하여 Add_Money_InputField.text에 저장
        Add_Money_InputField.text = inputValue.ToString();
    }

    public void Manage_Money_Share_GoBack_Button()
    {
        CurrentState_Manager = "ATM_lobby";
        Send_Money_Target_ID_InputField.text = null;
    }

    public void Deposit_Cash_Execution_Button()
    {
        ATM_Manager.Instance.Deposit_Cash();
        Current_Cash_Number.text = ATM_Manager.Instance.cash.ToString("C0");
        Current_Account_Money_Number.text = Account_Manager.Instance.current_account.account_Money.ToString("C0");
    }

    public void Withdraw_Money_Execution_Button()
    {
        ATM_Manager.Instance.Withdraw_Account_Money();
        Current_Cash_Number.text = ATM_Manager.Instance.cash.ToString("C0");
        Current_Account_Money_Number.text = Account_Manager.Instance.current_account.account_Money.ToString("C0");
    }

    public void Send_Money_Execution_Button()
    {
        ATM_Manager.Instance.Send_Account_Money();
        Current_Cash_Number.text = ATM_Manager.Instance.cash.ToString("C0");
        Current_Account_Money_Number.text = Account_Manager.Instance.current_account.account_Money.ToString("C0");
    }

    public void Login_ToLogin_button()
    {
        Account_Manager.Instance.Log_in();
    }

    public void Login_ToSignup_button()
    {
        CurrentState_Manager = "sign_up_ing";
        Login_PW_InputField.text = null;
    }

    public void Signup_ToSignup_button()
    {
        Account_Manager.Instance.Sign_up();
    }

    public void Signup_Cancel_button()
    {
        Signup_ID_InputField.text = null;
        Signup_Name_InputField.text = null;
        Signup_PW_InputField.text = null;
        Signup_PWCF_InputField.text = null;
        CurrentState_Manager = "log_in_ing";
    }




    public void Error(string message)  // 에러를 띄우는 메서드
    {
        past_State = current_State;
        Error_Text.text = message;
        CurrentState_Manager = "Error_ing";
    }
    public void Error_OK()  // 에러 창에서 OK 버튼을 눌렀을 시 메서드
    {
        Error_Obj.gameObject.SetActive(false);
        CurrentState_Manager = past_State;
    }

    void OnNumericInputChanged(string input)
    {
        // 입력값이 숫자가 아닌 경우 제거
        string filteredInput = FilterNonNumeric(input);

        // 입력값 업데이트
        if (filteredInput != input)
        {
            Add_Money_InputField.text = filteredInput;
        }
    }

    string FilterNonNumeric(string input)
    {
        // 숫자와 '.'만 허용하도록 필터링
        string filteredInput = new string(input.Where(c => char.IsDigit(c) || c == '.').ToArray());
        return filteredInput;
    }
}
