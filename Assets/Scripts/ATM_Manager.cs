using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Account_Manager;

public class ATM_Manager : MonoBehaviour //ATM 매니저. 현금, 입금, 출금, 송금의 금액 제어를 직접적으로 다루는 메서드들이 여기에 있습니다.
{
    public InputField Add_Money_InputField;
    public InputField Send_Money_Target_ID_InputField;


    //소지 현금을 저장하는 변수
    public int cash;

    private static ATM_Manager instance;
    public static ATM_Manager Instance        //싱글톤화. 다른 스크립트에서 쉽게 접근 가능
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<ATM_Manager>();

                if (instance == null)
                {
                    GameObject singletonObject = new GameObject("ATM_Manager");
                    instance = singletonObject.AddComponent<ATM_Manager>();
                }
            }
            return instance;
        }
    }

    private void Awake()
    {
        cash = 100000;
    }

    public void Deposit_Cash()   //입금 메서드
    {
        // 1. Account_Manager.Instance.current_account.isLog_in이 true인지 체크.
        if (Account_Manager.Instance.current_account.isLog_in)
        {
            // 2. Add_Money_InputField.text가 int 형식으로 바꿀 수 있는 값인지 체크.
            if (int.TryParse(Add_Money_InputField.text, out int depositAmount))
            {
                // 3. cash가 Add_Money_InputField.text를 int로 바꾼 값 이상인지 체크.
                if (cash >= depositAmount)
                {
                    // 4. 위 모든 조건을 만족하면,
                    // Add_Money_InputField.text를 int 형식으로 바꾼 값을 Account_Manager.Instance.current_account.account_Money에 더한다.
                    Account_Manager.Instance.current_account.account_Money += depositAmount;

                    // Add_Money_InputField.text를 int 형식으로 바꾼 값을 cash에서 뺀다.
                    cash -= depositAmount;

                    Debug.Log("입금 성공");
                }
                else
                {
                    Debug.Log("현금이 부족합니다!");
                    General_Manager.Instance.Error("현금이 부족합니다!");
                }
            }
            else
            {
                Debug.Log("잘못된 금액 형식");
                General_Manager.Instance.Error("잘못된 금액 형식입니다.");
            }
        }
        else
        {
            Debug.Log("현재 로그인 된 계정이 없습니다");
            General_Manager.Instance.Error("로그인 중이 아닙니다");
        }
    }

    public void Withdraw_Account_Money()  //출금 메서드
    {
        // 1. Account_Manager.Instance.current_account.isLog_in이 true인지 체크.
        if (Account_Manager.Instance.current_account.isLog_in)
        {
            // 2. Add_Money_InputField.text가 int 형식으로 바꿀 수 있는 값인지 체크.
            if (int.TryParse(Add_Money_InputField.text, out int withdrawAmount))
            {
                // 3. Account_Manager.Instance.current_account.account_Money가 Add_Money_InputField.text를 int로 바꾼 값 이상인지 체크.
                if (Account_Manager.Instance.current_account.account_Money >= withdrawAmount)
                {
                    // 4. 위 모든 조건을 만족하면,
                    // Add_Money_InputField.text를 int 형식으로 바꾼 값을 cash에 더한다.
                    cash += withdrawAmount;

                    // Add_Money_InputField.text를 int 형식으로 바꾼 값을 Account_Manager.Instance.current_account.account_Money에서 뺀다.
                    Account_Manager.Instance.current_account.account_Money -= withdrawAmount;

                    Debug.Log("출금 성공");
                }
                else
                {
                    Debug.Log("잔액이 부족합니다!");
                    General_Manager.Instance.Error("잔액이 부족합니다!");
                }
            }
            else
            {
                Debug.Log("잘못된 금액 형식");
                General_Manager.Instance.Error("잘못된 금액 형식입니다.");
            }
        }
        else
        {
            Debug.Log("현재 로그인 된 계정이 없습니다");
            General_Manager.Instance.Error("로그인 중이 아닙니다");
        }
    }

    public void Send_Account_Money() //송금하는 메서드
    {
        // 1. Send_Money_Target_ID_InputField.text와 같은 ID 값을 가진 account가 Account_Manager.Instance.account_list에 있는지 체크.
        account send_Account = Account_Manager.Instance.account_list.Find(acc => acc.ID == Send_Money_Target_ID_InputField.text);

        if (send_Account != null)
        {
            // 2. Account_Manager.Instance.current_account.isLog_in이 true인지 체크.
            if (Account_Manager.Instance.current_account.isLog_in)
            {
                // 3. Account_Manager.Instance.current_account.account_Money가 Add_Money_InputField.text를 int로 바꾼 값 이상인지 체크.
                if (int.TryParse(Add_Money_InputField.text, out int sendAmount))
                {
                    if (Account_Manager.Instance.current_account.account_Money >= sendAmount)
                    {
                        // 4. 위 모든 조건을 만족하면,
                        // Add_Money_InputField.text를 int 형식으로 바꾼 값을 send_Account.account_Money에 더한다.
                        send_Account.account_Money += sendAmount;

                        // Add_Money_InputField.text를 int 형식으로 바꾼 값을 Account_Manager.Instance.current_account.account_Money에서 뺀다.
                        Account_Manager.Instance.current_account.account_Money -= sendAmount;

                        // You can add additional logic or UI updates here for a successful money transfer.
                        Debug.Log("송금 성공!");
                    }
                    else
                    {
                        Debug.Log("잔액이 부족합니다!");
                        General_Manager.Instance.Error("잔액이 부족합니다!");
                    }
                }
                else
                {
                    Debug.Log("잘못된 금액 형식");
                    General_Manager.Instance.Error("잘못된 금액 형식입니다.");
                }
            }
            else
            {
                Debug.Log("현재 로그인 된 계정이 없습니다");
                General_Manager.Instance.Error("로그인 중이 아닙니다");
            }
        }
        else
        {
            Debug.Log("해당 ID를 가진 계정을 찾을 수 없습니다.");
            General_Manager.Instance.Error("계정을 찾을 수 없습니다.");
            Send_Money_Target_ID_InputField.text = null;
        }
    }
}
