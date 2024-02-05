using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Account_Manager : MonoBehaviour     //���� �Ŵ���. ���� Ŭ������ ����Ʈ, �α���, ȸ������, ���� ����, ���� ���� ����, �ε带 �����մϴ�.
{
    public InputField Login_ID_InputField;
    public InputField Login_PW_InputField;

    public InputField Signup_ID_InputField;
    public InputField Signup_Name_InputField;
    public InputField Signup_PW_InputField;
    public InputField Signup_PWCF_InputField;

    public account current_account = null;

    private static Account_Manager instance;
    public static Account_Manager Instance        //�̱���ȭ. �ٸ� ��ũ��Ʈ���� ���� ���� ����
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
        // ID�� �����ϴ� ����
        public string ID;

        // Name�� �����ϴ� ����
        public string Name;

        // �н����带 �����ϴ� ����
        public string PW;

        // ���� �ܱ��� �����ϴ� ����
        public int account_Money = 50000;

        // �α��� ���¸� �����ϴ� ����
        public bool isLog_in = false;

        // ������
        public account(string id, string name, string pw)
        {
            ID = id;
            Name = name;
            PW = pw;
        }
    }

    public List<account> account_list = new List<account>();


    // �� account Ŭ���� �ν��Ͻ��� �����ϴ� �޼���
    public account CreateAccount(string id, string name, string pw)     
    {
        account newAccount = new account(id, name, pw);
        account_list.Add(newAccount); //���� ������ ����Ʈ�� ���� �߰���
        return newAccount;
    }

    private void Awake() //�����ũ�� ��, �⺻ ������ ������ �α��ε� ���·� �����մϴ�.
    {
        LoadAccountList(); //���� �ε�

        // ������ ������ �⺻ ���� ����
        if (account_list.Count == 0)
        {
            current_account = CreateAccount("jhwoo", "������", "1234");
            current_account.isLog_in = true;
        }
        PrintAccountList();


    }

    public void Log_in() //�α��� �޼���
    {
        // 1. account_list���� Login_ID_InputField.text�� ���� ID���� ���� account�� ã�´�.
        account foundAccount = account_list.Find(acc => acc.ID == Login_ID_InputField.text);

        // 2. Login_ID_InputField.text�� ���� ID���� ���� account�� ���� ��,
        if (foundAccount != null)
        {
            // �ش� account�� PW���� InputField Login_PW_InputField.text�� ��ġ�ϴ��� üũ�Ѵ�.
            if (foundAccount.PW == Login_PW_InputField.text)
            {
                // 3. �ش� account�� ID�� PW���� ��� �Էµ� ���� ��ġ�� ���, �ش� account�� isLog_in�� true�� �����.
                foundAccount.isLog_in = true;
                current_account = foundAccount;

                Debug.Log("�α��� ����!");
                General_Manager.Instance.CurrentState_Manager = "ATM_lobby";
                Login_PW_InputField.text = null;
                General_Manager.Instance.Error("�α��� ����!");
            }
            else
            {
                Debug.Log("�߸��� ��й�ȣ");
                General_Manager.Instance.Error("�߸��� ��й�ȣ�Դϴ�.");
                Login_PW_InputField.text = null;
            }
        }
        else
        {
            Debug.Log("�ش� ���̵��� ������ ã�� �� �����ϴ�.");
            General_Manager.Instance.Error("������ ã�� �� �����ϴ�.");
            Login_ID_InputField.text = null;
            Login_PW_InputField.text = null;
        }
    }

    public void Log_out()  //�α׾ƿ� �޼���
    {
        current_account.isLog_in = false;
        current_account = null;
    }

    public void Sign_up()
    {
        // 1. Signup_ID_InputField.text�� ������ ������ �����ϴ��� üũ.
        // ID�� ����� ���ڷθ� �����Ǿ�� �ϰ�, 3���� �̻� ~ 10���� ����
        // account_list�� �Էµ� ID ���� ������ ���� ���� account�� �������.
        string signupID = Signup_ID_InputField.text;
        if (System.Text.RegularExpressions.Regex.IsMatch(signupID, "^[a-zA-Z0-9]{3,10}$"))
        {
            if (account_list.Find(acc => acc.ID == signupID) == null)
            {
                // 2. Signup_Name_InputField.text�� ������ ������ �����ϴ��� üũ.
                // 2���� �̻�, 5���� ����
                string signupName = Signup_Name_InputField.text;
                if (signupName.Length >= 2 && signupName.Length <= 5)
                {
                    // 3. Signup_PW_InputField.text�� ������ ������ �����ϴ��� üũ.
                    // ����� ���ڷθ� �����Ǿ�� �ϰ�, 5���� �̻� ~ 15���� ����
                    string signupPW = Signup_PW_InputField.text;
                    if (System.Text.RegularExpressions.Regex.IsMatch(signupPW, "^[a-zA-Z0-9]{5,15}$"))
                    {
                        // 4. Signup_PWCF_InputField.text�� Signup_PW_InputField.text�� ���� ������ üũ.
                        if (signupPW == Signup_PWCF_InputField.text)
                        {
                            // 5. ���� ��� ������ �����Ѵٸ�, �� ������ ����.
                            CreateAccount(signupID, signupName, signupPW);
                            SaveAccountList(); //���� ����

                            Debug.Log("ȸ������ ����!");
                            General_Manager.Instance.CurrentState_Manager = "log_in_ing";

                            Signup_ID_InputField.text = null;
                            Signup_Name_InputField.text = null;
                            Signup_PW_InputField.text = null;
                            Signup_PWCF_InputField.text = null;
                            General_Manager.Instance.Error("ȸ������ ����!");
                        }
                        else
                        {
                            Debug.Log("��й�ȣ�� ��ġ���� �ʽ��ϴ�.");
                            General_Manager.Instance.Error("��й�ȣ�� Ʋ���ϴ�.");
                            Signup_PW_InputField.text = null;
                            Signup_PWCF_InputField.text = null;
                        }
                    }
                    else
                    {
                        Debug.Log("�߸��� ��й�ȣ ���");
                        General_Manager.Instance.Error("�߸��� ��й�ȣ�Դϴ�.");
                        Signup_PW_InputField.text = null;
                        Signup_PWCF_InputField.text = null;
                    }
                }
                else
                {
                    Debug.Log("�߸��� �̸� ���");
                    General_Manager.Instance.Error("�߸��� �̸� ����Դϴ�.");
                    Signup_Name_InputField.text = null;
                }
            }
            else
            {
                Debug.Log("�̹� �����ϴ� ID");
                General_Manager.Instance.Error("�̹� �����ϴ� ID�Դϴ�.");
                Signup_ID_InputField.text = null;
            }
        }
        else
        {
            Debug.Log("�߸��� ID ���");
            General_Manager.Instance.Error("�߸��� ID ����Դϴ�.");
            Signup_ID_InputField.text = null;
        }
    }

    void LoadAccountList()
    {
        // ����� ������ �ҷ�����
        string json = PlayerPrefs.GetString("account_list", "");
        if (!string.IsNullOrEmpty(json))
        {
            // Json�� ����Ʈ�� ��ȯ
            account_list = JsonUtility.FromJson<List<account>>(json).ToList();
        }
    }

    void SaveAccountList()
    {
        // ����Ʈ�� Json���� ��ȯ�Ͽ� ����
        string json = JsonUtility.ToJson(account_list);
        PlayerPrefs.SetString("account_list", json);
        PlayerPrefs.Save();
        Debug.Log("���� ���� �Ϸ�");
    }

    void PrintAccountList()
    {
        // ��� ���� ���� ���
        foreach (var account in account_list)
        {
            Debug.Log($"Username: {account.Name}");
        }
    }
}
