using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* ##################################################################################################
 * 1. ��ũ��Ʈ��: Recaller.cs
 * 2. ����: ���� ������Ʈ�� ��� �� �ð� ���� ���
 * ################################################################################################## */

public class Recaller : MonoBehaviour
{
    // Unity ���� �ִ� Transform, Rigidbody�� �ƴ� �Ʒ����� ���� ������ ����ϱ� ���� 'new' Ű���� ���
    [SerializeField] private new Transform transform;
    [SerializeField] private new Rigidbody rigidbody;

    /* ����� ��Ҹ� �����ϴ� ����ü (�ð� ���࿡ �ʿ��� ��ҵ� �߰� ����) */
    public struct Recorder
    {
        public Vector3 position;    // ��ġ
        public Quaternion rotation; // ȸ��

        // ������
        public Recorder(Vector3 position, Quaternion rotation)
        {
            this.position = position;
            this.rotation = rotation;
        }
    }

    // ��� �� �ð� ���� Ȱ��ȭ ����
    private new bool enabled = true;

    // �ܺο��� ���� ������ Ȱ��ȭ �Ӽ� (OnEnable / OnDisable ���� ��� / ����)
    public bool Enabled
    {
        get { return enabled; }
        set
        {
            if (value) OnEnable();
            else OnDisable();
            enabled = value;
        }
    }

    // Transform ���� ����� �����ϴ� ����Ʈ
    private List<Recorder> recordList = new List<Recorder>();

    // �ִ� ��� ���� (�� �� �з��� �����͸� ������ �� ����)
    public int recordLimit = 250;

    // ���� �ð� ���� ������ ����
    public bool IsRecalling { get; private set; } = false;

    // Rigidbody�� isKinematic �Ӽ� ����
    private bool isKinematic = false;

    /*##################################################################################################
    * �ð� ������ ������ �ִ� �ð� ���
    * ################################################################################################## */
    public float RecallTime
    {
        get
        {
            // RecallManager���� ��� �ֱ⸦ �����ͼ�
            int frame = RecallManager.GetRecordFrame();

            // �ִ� �ð� ��ȯ [�ִ� �ð� -> �ִ� ��� ���� * (FixedUpdate �ֱ� * ��� �ֱ�)]
            return recordLimit * (Time.fixedDeltaTime * frame);
        }
    }

    /*##################################################################################################
    * ���� �ʱ�ȭ
    * ################################################################################################## */
    private void Awake()
    {
        // Transform �� Rigidbody ������Ʈ ����
        if (transform == null) transform = GetComponent<Transform>();
        if (rigidbody == null) rigidbody = GetComponent<Rigidbody>();
    }

    /*##################################################################################################
    * ������Ʈ�� Ȱ��ȭ�� �� (�Ǵ� Enabled �Ӽ��� ���� Ȱ��ȭ�� ��)
    * ################################################################################################## */
    private void OnEnable()
    {
        if (Enabled)
        {
            // RecallManager�� ����Ʈ�� ��ųʸ��� �ڽ� ���
            RecallManager.RecallerList.Add(this);
            RecallManager.RecallerDctn.Add(gameObject, this);

            // RecallManager�� Recorder ��������Ʈ�� �ڽ��� Record �Լ� ����
            RecallManager.Recorder += Record;
        }
    }

    /*##################################################################################################
    * ������Ʈ�� ��Ȱ��ȭ �� ��
    * ################################################################################################## */
    private void OnDisable()
    {
        // RecallManager�� ����Ʈ�� ��ųʸ��� �ڽ� ����
        RecallManager.RecallerList.Remove(this);
        RecallManager.RecallerDctn.Remove(gameObject);

        // RecallManager�� Recorder ��������Ʈ ���� ����
        RecallManager.Recorder -= Record;
    }

    /*##################################################################################################
    * �ð� ���� �Լ� [�Ķ����: lerpTime(���� �ð�), endOfFrame(���ο� ��� ���� ���� ����)]
    * ################################################################################################## */
    public void Recall(float lerpTime, bool endOfFrame)
    {
        // �ð� ���� Ȱ��ȭ �� ��ϵ� ������ ����
        if (enabled && recordList.Count > 0)
        {
            /* ���� ���¿��� ���� ������ ��� (recordList[0]) ���·� ���� */
            transform.position = Vector3.Lerp(transform.position, recordList[0].position, lerpTime);
            transform.rotation = Quaternion.Slerp(transform.rotation, recordList[0].rotation, lerpTime);

            // ���ο� ��� ������ �������� �� (�� �ֱⰡ ������ ��)
            if (endOfFrame) { recordList.RemoveAt(0); } // ���� ������ ���(�ð� ���� ��ǥ ����) ����
        }

        // �ð� ���� ���� �Ǵ� ��ϵ� ������ ����
        else
        {
            // �ð� ���� ��������Ʈ ���� �� ��� ��������Ʈ ����
            RecallManager.Recaller -= Recall;
            RecallManager.Recorder += Record;

            // Rigidbody�� �ְ� ���� isKinematic�� �ƴϾ��ٸ� isKinematic�� �����Ͽ� ���� �ۿ��� �簳
            if (rigidbody != null && !isKinematic) { rigidbody.isKinematic = false; }
            IsRecalling = false;
        }
    }

    /*##################################################################################################
    * �ð� ���� Ȱ��ȭ �� Rigidbody ���� ����
    * ################################################################################################## */
    public void RecallEnable()
    {
        if (enabled)
        {
            IsRecalling = true;

            if (rigidbody != null)
            {
                // ���� isKinematic ���� ����
                isKinematic = rigidbody.isKinematic;
                // �ð� ���� �߿��� isKinematic�� true�� �ؼ� ���� ���� ������
                if (!isKinematic) rigidbody.isKinematic = true;
            }

            // ��� ��������Ʈ ���� �� �ð� ���� ��������Ʈ ����
            RecallManager.Recorder -= Record;
            RecallManager.Recaller += Recall;
        }
    }

    /*##################################################################################################
    * ���� ��� �Ǵ� �ʱ�ȭ
    * ################################################################################################## */
    public void Record()
    {
        if (enabled)
        {

            // �ִ� ��� ������ �ʰ��ϸ� ���� ������ ��� ����
            if (recordList.Count >= recordLimit)
            {
                recordList.RemoveAt(recordList.Count - 1);
            }

            /* ���� ����(��ġ, ȸ��)�� ���ο� Recorder ����ü�� ����� ����Ʈ�� �� ��(0�� �ε���)�� �߰� */
            // �ʿ信 ���� ���� ���¿� �ٸ� ��� �߰� ����
            recordList.Insert(0, new Recorder(transform.position, transform.rotation));
        }

        else if (!enabled)
        {
            // ��� ����Ʈ �ʱ�ȭ
            if (recordList.Count > 0) { recordList.Clear(); }
        }
    }
}
