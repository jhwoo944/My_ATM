using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Account_Manager;

public class ATM_Manager : MonoBehaviour //ATM �Ŵ���. ����, �Ա�, ���, �۱��� �ݾ� ��� ���������� �ٷ�� �޼������ ���⿡ �ֽ��ϴ�.
{
    public InputField Add_Money_InputField;
    public InputField Send_Money_Target_ID_InputField;


    //���� ������ �����ϴ� ����
    public int cash;

    private static ATM_Manager instance;
    public static ATM_Manager Instance        //�̱���ȭ. �ٸ� ��ũ��Ʈ���� ���� ���� ����
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

    public void Deposit_Cash()   //�Ա� �޼���
    {
        // 1. Account_Manager.Instance.current_account.isLog_in�� true���� üũ.
        if (Account_Manager.Instance.current_account.isLog_in)
        {
            // 2. Add_Money_InputField.text�� int �������� �ٲ� �� �ִ� ������ üũ.
            if (int.TryParse(Add_Money_InputField.text, out int depositAmount))
            {
                // 3. cash�� Add_Money_InputField.text�� int�� �ٲ� �� �̻����� üũ.
                if (cash >= depositAmount)
                {
                    // 4. �� ��� ������ �����ϸ�,
                    // Add_Money_InputField.text�� int �������� �ٲ� ���� Account_Manager.Instance.current_account.account_Money�� ���Ѵ�.
                    Account_Manager.Instance.current_account.account_Money += depositAmount;

                    // Add_Money_InputField.text�� int �������� �ٲ� ���� cash���� ����.
                    cash -= depositAmount;

                    Debug.Log("�Ա� ����");
                }
                else
                {
                    Debug.Log("������ �����մϴ�!");
                    General_Manager.Instance.Error("������ �����մϴ�!");
                }
            }
            else
            {
                Debug.Log("�߸��� �ݾ� ����");
                General_Manager.Instance.Error("�߸��� �ݾ� �����Դϴ�.");
            }
        }
        else
        {
            Debug.Log("���� �α��� �� ������ �����ϴ�");
            General_Manager.Instance.Error("�α��� ���� �ƴմϴ�");
        }
    }

    public void Withdraw_Account_Money()  //��� �޼���
    {
        // 1. Account_Manager.Instance.current_account.isLog_in�� true���� üũ.
        if (Account_Manager.Instance.current_account.isLog_in)
        {
            // 2. Add_Money_InputField.text�� int �������� �ٲ� �� �ִ� ������ üũ.
            if (int.TryParse(Add_Money_InputField.text, out int withdrawAmount))
            {
                // 3. Account_Manager.Instance.current_account.account_Money�� Add_Money_InputField.text�� int�� �ٲ� �� �̻����� üũ.
                if (Account_Manager.Instance.current_account.account_Money >= withdrawAmount)
                {
                    // 4. �� ��� ������ �����ϸ�,
                    // Add_Money_InputField.text�� int �������� �ٲ� ���� cash�� ���Ѵ�.
                    cash += withdrawAmount;

                    // Add_Money_InputField.text�� int �������� �ٲ� ���� Account_Manager.Instance.current_account.account_Money���� ����.
                    Account_Manager.Instance.current_account.account_Money -= withdrawAmount;

                    Debug.Log("��� ����");
                }
                else
                {
                    Debug.Log("�ܾ��� �����մϴ�!");
                    General_Manager.Instance.Error("�ܾ��� �����մϴ�!");
                }
            }
            else
            {
                Debug.Log("�߸��� �ݾ� ����");
                General_Manager.Instance.Error("�߸��� �ݾ� �����Դϴ�.");
            }
        }
        else
        {
            Debug.Log("���� �α��� �� ������ �����ϴ�");
            General_Manager.Instance.Error("�α��� ���� �ƴմϴ�");
        }
    }

    public void Send_Account_Money() //�۱��ϴ� �޼���
    {
        // 1. Send_Money_Target_ID_InputField.text�� ���� ID ���� ���� account�� Account_Manager.Instance.account_list�� �ִ��� üũ.
        account send_Account = Account_Manager.Instance.account_list.Find(acc => acc.ID == Send_Money_Target_ID_InputField.text);

        if (send_Account != null)
        {
            // 2. Account_Manager.Instance.current_account.isLog_in�� true���� üũ.
            if (Account_Manager.Instance.current_account.isLog_in)
            {
                // 3. Account_Manager.Instance.current_account.account_Money�� Add_Money_InputField.text�� int�� �ٲ� �� �̻����� üũ.
                if (int.TryParse(Add_Money_InputField.text, out int sendAmount))
                {
                    if (Account_Manager.Instance.current_account.account_Money >= sendAmount)
                    {
                        // 4. �� ��� ������ �����ϸ�,
                        // Add_Money_InputField.text�� int �������� �ٲ� ���� send_Account.account_Money�� ���Ѵ�.
                        send_Account.account_Money += sendAmount;

                        // Add_Money_InputField.text�� int �������� �ٲ� ���� Account_Manager.Instance.current_account.account_Money���� ����.
                        Account_Manager.Instance.current_account.account_Money -= sendAmount;

                        // You can add additional logic or UI updates here for a successful money transfer.
                        Debug.Log("�۱� ����!");
                    }
                    else
                    {
                        Debug.Log("�ܾ��� �����մϴ�!");
                        General_Manager.Instance.Error("�ܾ��� �����մϴ�!");
                    }
                }
                else
                {
                    Debug.Log("�߸��� �ݾ� ����");
                    General_Manager.Instance.Error("�߸��� �ݾ� �����Դϴ�.");
                }
            }
            else
            {
                Debug.Log("���� �α��� �� ������ �����ϴ�");
                General_Manager.Instance.Error("�α��� ���� �ƴմϴ�");
            }
        }
        else
        {
            Debug.Log("�ش� ID�� ���� ������ ã�� �� �����ϴ�.");
            General_Manager.Instance.Error("������ ã�� �� �����ϴ�.");
            Send_Money_Target_ID_InputField.text = null;
        }
    }
}
