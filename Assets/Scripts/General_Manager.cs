using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class General_Manager : MonoBehaviour          //�Ѱ� �Ŵ���. '���� ����'�� �Ѱ��ϰ� ���� ���׿� ���� ��ü UI���� SetActive�� �����մϴ�. �׸��� ��ư���� Ŭ�� �� �� ȣ��Ǵ� �޼��带 �����ϰ� �ֽ��ϴ�. �׸��� ��Ÿ �Ŵ������� �ٷ��� �ʴ� ����� ��ɰ� ������ ó���մϴ�.
{
    public GameObject Title_Text_Obj;  //Ÿ��Ʋ
    public GameObject Current_Cash_Obj; //���� �ݾ�
    public GameObject Current_Account_Obj; //���� ����
    public GameObject ATM_Lobby_Obj; //ATM �κ�
    public GameObject Manage_Money_Share_Unit_Obj; //�Ա� ��� �۱�(���� ��ư)
    public GameObject Manage_Money_Share_Public__Obj; //�Ա� ��� �۱�(�ݾ� �Է�/�ڷΰ���)
    public GameObject Deposit_Cash_Obj; //�Ա�
    public GameObject Withdraw_Money_Obj; //���
    public GameObject Send_Money_Obj; //�۱�
    public GameObject Login_Obj; //�α���
    public GameObject Signup_Obj; //ȸ������
    public GameObject Error_Obj; //����

    public InputField Add_Money_InputField;
    public InputField Send_Money_Target_ID_InputField;

    public InputField Login_ID_InputField;
    public InputField Login_PW_InputField;

    public InputField Signup_ID_InputField;
    public InputField Signup_Name_InputField;
    public InputField Signup_PW_InputField; 
    public InputField Signup_PWCF_InputField;

    public Text Current_Cash_Number; //���� ������ �ؽ�Ʈ
    public Text Current_Account_Name_Text; //���� �̸��� �ؽ�Ʈ
    public Text Current_Account_Money_Number; //���� ���� ������ �ؽ�Ʈ
    public Text Error_Text; //���� �޼��� â�� �ؽ�Ʈ


    private static General_Manager instance;
    public static General_Manager Instance        //�̱���ȭ. �ٸ� ��ũ��Ʈ���� ���� ���� ����
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

    public String current_State; //���� ���¸� �����ϴ� ���� ����
    String past_State; //���� ���¸� �����ϴ� ����

    public event Action<string> OnStateChanged;
    private void HandleStateChanged(string newState)  //���°� ����� �� �̺�Ʈ�� ���� �ڵ����� ȣ��Ǵ� �޼����Դϴ�
    {
        /* current_State ���

        1. "log_in_ing"

        2. "sign_up_ing"

        3. "ATM_lobby"

        4. "Deposit_Cash_Ing"

        5. "Withdraw_Account_Money_ing"

        6. "Send_Account_Money_ing"

        7. "Error_ing"

        */

        Debug.Log("���°� ����Ǿ����ϴ�: " + newState);

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


    public string CurrentState_Manager //���� ���� ���� ������ �� �κ�ũ�� �ߵ��ϴ� �Ŵ��� ������Ƽ
    {
        set    //����� ��
        {
            if (current_State != value)
            {
                current_State = value;
                OnStateChanged?.Invoke(current_State);
            }
        }
    }

















    private void Start()        // ���� �� 
    {
        OnStateChanged += HandleStateChanged; //�̺�Ʈ�� �޼��� ���
        Title_Text_Obj.gameObject.SetActive(true);
        if (Account_Manager.Instance.current_account != null && Account_Manager.Instance.current_account.isLog_in == true) //���۽� �α��ε� ������ ������ ATM�κ񿡼�, �ƴ� ��� �α���â���� ����
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

        Add_Money_InputField.onValueChanged.AddListener(OnNumericInputChanged); //�ݾ� �Է� ��ǲ�ʵ忡 ���ڸ� �Էµǵ��� �����մϴ�.
    }






    //�� �Ʒ��� ��ư �޼����Դϴ�.
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

        // �Է°��� ������ �⺻������ 10000 ����
        if (string.IsNullOrEmpty(inputText))
        {
            inputValue = 10000;
        }
        else
        {
            // ����ȭ�ϰ� 10000�� ���ϱ�
            inputValue = int.Parse(inputText) + 10000;
        }

        // ����� �ٽ� ���ڿ��� ��ȯ�Ͽ� Add_Money_InputField.text�� ����
        Add_Money_InputField.text = inputValue.ToString();
    }

    public void Add_30000_Button()
    {
        int inputValue;
        string inputText = Add_Money_InputField.text;

        // �Է°��� ������ �⺻������ 30000 ����
        if (string.IsNullOrEmpty(inputText))
        {
            inputValue = 30000;
        }
        else
        {
            // ����ȭ�ϰ� 10000�� ���ϱ�
            inputValue = int.Parse(inputText) + 30000;
        }

        // ����� �ٽ� ���ڿ��� ��ȯ�Ͽ� Add_Money_InputField.text�� ����
        Add_Money_InputField.text = inputValue.ToString();
    }

    public void Add_50000_Button()
    {
        int inputValue;
        string inputText = Add_Money_InputField.text;

        // �Է°��� ������ �⺻������ 30000 ����
        if (string.IsNullOrEmpty(inputText))
        {
            inputValue = 50000;
        }
        else
        {
            // ����ȭ�ϰ� 10000�� ���ϱ�
            inputValue = int.Parse(inputText) + 50000;
        }

        // ����� �ٽ� ���ڿ��� ��ȯ�Ͽ� Add_Money_InputField.text�� ����
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




    public void Error(string message)  // ������ ���� �޼���
    {
        past_State = current_State;
        Error_Text.text = message;
        CurrentState_Manager = "Error_ing";
    }
    public void Error_OK()  // ���� â���� OK ��ư�� ������ �� �޼���
    {
        Error_Obj.gameObject.SetActive(false);
        CurrentState_Manager = past_State;
    }

    void OnNumericInputChanged(string input)
    {
        // �Է°��� ���ڰ� �ƴ� ��� ����
        string filteredInput = FilterNonNumeric(input);

        // �Է°� ������Ʈ
        if (filteredInput != input)
        {
            Add_Money_InputField.text = filteredInput;
        }
    }

    string FilterNonNumeric(string input)
    {
        // ���ڿ� '.'�� ����ϵ��� ���͸�
        string filteredInput = new string(input.Where(c => char.IsDigit(c) || c == '.').ToArray());
        return filteredInput;
    }
}
