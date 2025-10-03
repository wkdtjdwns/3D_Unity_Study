using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* ##################################################################################################
 * 1. ��ũ��Ʈ��: InputManager.cs
 * 2. ����: �ð� ���� �׽�Ʈ�� �ʿ��� ����(������Ʈ �̵�) �� �ð� ���� ���� ���
 * ################################################################################################## */

public class InputManager : MonoBehaviour
{
    public float explosionForce = 10f;      // ���� ��
    public float upwardForce = 1f;          // �������� �������� ��
    public KeyCode recallKey = KeyCode.E;   // �ð� ���� ���� Ű

    /* ##################################################################################################
     * ���� ȿ�� �� �ð� ���� ����
     * ################################################################################################## */
    private void Update()
    {
        // ���콺 ���� ��ư Ŭ�� �� ���� ����
        if (Input.GetMouseButtonDown(0))
        {
            ApplyExplosion();
            print("Boom");
        }

        // 'E' Ű �Է� �� �ð� ���� ����
        if (Input.GetKeyDown(recallKey))
        {
            RecallManager.RecallAll();
            print("Recall");
        }
    }

    /* ##################################################################################################
     * ���� ȿ�� ���� �Լ�
     * ################################################################################################## */
    private void ApplyExplosion()
    {
        // ���� ��ġ ����
        Vector3 explosionCenter = new Vector3(0, 0.55f, 0);

        foreach (Recaller recaller in RecallManager.RecallerList)
        {
            // ��� Recaller�� Rigidbody�� ������
            Rigidbody rigidbody = recaller.GetComponent<Rigidbody>();
            
            // Rigidbody�� �����ϰ� ���� �ð� ���� ���� �ƴϸ� �ڱ� �ڽ��� �ƴ� ��� ����
            if (rigidbody != null && !recaller.IsRecalling && rigidbody.gameObject != gameObject)
            {
                rigidbody.AddExplosionForce(explosionForce, explosionCenter, 1000f, upwardForce, ForceMode.Impulse);
            }
        }
    }
}
