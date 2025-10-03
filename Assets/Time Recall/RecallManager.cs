using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* ##################################################################################################
 * 1. ��ũ��Ʈ��: RecallManager.cs
 * 2. ����: ��� �� �ð� ���� ���� ���� �� �Լ� ���
 * ################################################################################################## */

public class RecallManager : MonoBehaviour
{
    // �̱��� ������ ���� �ν��Ͻ�
    private static RecallManager instance;

    /*##################################################################################################
    * Action ��������Ʈ
    * ################################################################################################## */
    // ������Ʈ�� ��ġ ����� ������ �� ȣ��Ǵ� Action ��������Ʈ
    public static Action Recorder;

    // �ð� ������ ������ �� ȣ��Ǵ� Action ��������Ʈ
    public static Action<float, bool> Recaller; // (�Ķ����: lerpTime[���� �ð�], endOfFrame[���ο� ��� ���� ���� ����])

    /*##################################################################################################
    * Recaller ������Ʈ ����Ʈ �� ��ųʸ�
    * ################################################################################################## */

    // Recaller ������Ʈ���� �����صδ� ����Ʈ
    public static List<Recaller> RecallerList { get; set; } = new List<Recaller>();

    // Recaller���� ������� <���� ������Ʈ, Recaller ������Ʈ>�� ���·� �����ϴ� ��ųʸ�
    public static Dictionary<GameObject, Recaller> RecallerDctn { get; set; } = new Dictionary<GameObject, Recaller>();

    /*##################################################################################################
    * Recall ���� �Լ�
    * ################################################################################################## */
    [Tooltip("'frame' ������ �ݵ�� 1 ���� Ŀ�� ��")]
    public int recordFrame = 1;    // ��� �ֱ�(������ ����)
    private int frame = 0;         // ���� ������ ī��Ʈ ����

    /*##################################################################################################
    * ���� �ʱ�ȭ
    * ################################################################################################## */
    private void Awake()
    {
        // �̱��� �ν��Ͻ� ����
        instance = this;

        // recordFrame �ʱ�ȭ
        if (recordFrame <= 0) { recordFrame = 1; }
    }

    /*##################################################################################################
    * ��� �� �ð� ���� ����
    * ################################################################################################## */
    private void FixedUpdate()
    {
        // ��� �ֱ⸶�� ����
        if (!(recordFrame <= 0) && frame <= 0)
        {
            // ��� Recaller�� ��� �Լ� ȣ��
            Recorder?.Invoke();

            // ��� �ֱ� �ʱ�ȭ
            frame = recordFrame;
        }

        // ���� �ð� ��� [���� �������� ���� ��� �������� �󸶳� ���� �Ǿ����� ���]
        float lerpTime = (float)(recordFrame - (frame - 1)) / recordFrame;

        // ��� Recaller�� �ð� ���� �Լ� ȣ�� (frame == 1 -> ���ο� ��� ���� ����)
        Recaller?.Invoke(lerpTime, frame == 1);
        frame -= 1;
    }

    /*##################################################################################################
    * Recall ���� �Լ�
    * ################################################################################################## */

    // ��� Recaller�� �ð� ���� ��� Ȱ��ȭ �Լ�
    public static void RecallAll()
    {
        foreach (var Recaller in RecallerList)
        {
            Recaller.RecallEnable();
        }
    }

    public static void Recall(GameObject gameObject) { RecallerDctn[gameObject].RecallEnable(); }   // �Ķ������ "���� ������Ʈ"�� ����� Recaller�� �ð� ���� ��� Ȱ��ȭ
    public static void Recall(Recaller Recaller) { Recaller.RecallEnable(); }                       // �Ķ������ "Recaller �ν��Ͻ�"�� �ð� ���� ��� Ȱ��ȭ

    // ��� Recaller�� �ִ� ��� ���� ����
    public static void SetRecordLimitAll(int limit)
    {
        foreach (var Recaller in RecallerList)
        {
            Recaller.recordLimit = limit;
        }
    }

    /*##################################################################################################
    * Getter & Setter
    * ################################################################################################## */
    public static int GetRecordFrame() { return instance.recordFrame; }            // ���� ��� �ֱ� ��ȯ
    public static void SetRecordFrame(int frame) { instance.recordFrame = frame; } // ���� ��� �ֱ� ����
    
    public static Recaller GetRecaller(GameObject gameObject) { return RecallerDctn[gameObject]; } // �Ķ������ ���� ������Ʈ�� ����� Recaller ��ȯ
    public static Recaller[] GetRecallerAll() { return RecallerList.ToArray(); } // ��� Recaller�� �迭�� ��ȯ
}
