using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* ##################################################################################################
 * 1. 스크립트명: Recaller.cs
 * 2. 역할: 게임 오브젝트의 기록 및 시간 역행 담당
 * ################################################################################################## */

public class Recaller : MonoBehaviour
{
    // Unity 내에 있는 Transform, Rigidbody가 아닌 아래에서 새로 정의해 사용하기 위해 'new' 키워드 사용
    [SerializeField] private new Transform transform;
    [SerializeField] private new Rigidbody rigidbody;

    /* 기록할 요소를 정의하는 구조체 (시간 역행에 필요한 요소들 추가 가능) */
    public struct Recorder
    {
        public Vector3 position;    // 위치
        public Quaternion rotation; // 회전

        // 생성자
        public Recorder(Vector3 position, Quaternion rotation)
        {
            this.position = position;
            this.rotation = rotation;
        }
    }

    // 기록 및 시간 역행 활성화 여부
    private new bool enabled = true;

    // 외부에서 접근 가능한 활성화 속성 (OnEnable / OnDisable 통해 등록 / 해제)
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

    // Transform 상태 기록을 저장하는 리스트
    private List<Recorder> recordList = new List<Recorder>();

    // 최대 기록 개수 (몇 초 분량의 데이터를 저장할 지 결정)
    public int recordLimit = 250;

    // 현재 시간 역행 중인지 여부
    public bool IsRecalling { get; private set; } = false;

    // Rigidbody의 isKinematic 속성 상태
    private bool isKinematic = false;

    /*##################################################################################################
    * 시간 역행이 가능한 최대 시간 계산
    * ################################################################################################## */
    public float RecallTime
    {
        get
        {
            // RecallManager에서 기록 주기를 가져와서
            int frame = RecallManager.GetRecordFrame();

            // 최대 시간 반환 [최대 시간 -> 최대 기록 개수 * (FixedUpdate 주기 * 기록 주기)]
            return recordLimit * (Time.fixedDeltaTime * frame);
        }
    }

    /*##################################################################################################
    * 변수 초기화
    * ################################################################################################## */
    private void Awake()
    {
        // Transform 및 Rigidbody 컴포넌트 참조
        if (transform == null) transform = GetComponent<Transform>();
        if (rigidbody == null) rigidbody = GetComponent<Rigidbody>();
    }

    /*##################################################################################################
    * 컴포넌트가 활성화될 때 (또는 Enabled 속성을 통해 활성화될 때)
    * ################################################################################################## */
    private void OnEnable()
    {
        if (Enabled)
        {
            // RecallManager의 리스트와 딕셔너리에 자신 등록
            RecallManager.RecallerList.Add(this);
            RecallManager.RecallerDctn.Add(gameObject, this);

            // RecallManager의 Recorder 델리게이트에 자신의 Record 함수 연결
            RecallManager.Recorder += Record;
        }
    }

    /*##################################################################################################
    * 컴포넌트가 비활성화 될 떄
    * ################################################################################################## */
    private void OnDisable()
    {
        // RecallManager의 리스트와 딕셔너리에 자신 해제
        RecallManager.RecallerList.Remove(this);
        RecallManager.RecallerDctn.Remove(gameObject);

        // RecallManager의 Recorder 델리게이트 연결 해제
        RecallManager.Recorder -= Record;
    }

    /*##################################################################################################
    * 시간 역행 함수 [파라미터: lerpTime(보간 시간), endOfFrame(새로운 기록 시점 도달 여부)]
    * ################################################################################################## */
    public void Recall(float lerpTime, bool endOfFrame)
    {
        // 시간 역행 활성화 및 기록된 데이터 존재
        if (enabled && recordList.Count > 0)
        {
            /* 현재 상태에서 가장 오래된 기록 (recordList[0]) 상태로 보간 */
            transform.position = Vector3.Lerp(transform.position, recordList[0].position, lerpTime);
            transform.rotation = Quaternion.Slerp(transform.rotation, recordList[0].rotation, lerpTime);

            // 새로운 기록 시점에 도달했을 때 (한 주기가 끝났을 때)
            if (endOfFrame) { recordList.RemoveAt(0); } // 가장 오래된 기록(시간 역행 목표 시점) 제거
        }

        // 시간 역행 종료 또는 기록된 데이터 없음
        else
        {
            // 시간 역행 델리게이트 해제 및 기록 델리게이트 연결
            RecallManager.Recaller -= Recall;
            RecallManager.Recorder += Record;

            // Rigidbody가 있고 원래 isKinematic이 아니었다면 isKinematic을 해제하여 물리 작용을 재개
            if (rigidbody != null && !isKinematic) { rigidbody.isKinematic = false; }
            IsRecalling = false;
        }
    }

    /*##################################################################################################
    * 시간 역행 활성화 및 Rigidbody 설정 변경
    * ################################################################################################## */
    public void RecallEnable()
    {
        if (enabled)
        {
            IsRecalling = true;

            if (rigidbody != null)
            {
                // 원래 isKinematic 상태 저장
                isKinematic = rigidbody.isKinematic;
                // 시간 역행 중에는 isKinematic을 true로 해서 물리 엔진 미적용
                if (!isKinematic) rigidbody.isKinematic = true;
            }

            // 기록 델리게이트 해제 및 시간 역행 델리게이트 연결
            RecallManager.Recorder -= Record;
            RecallManager.Recaller += Recall;
        }
    }

    /*##################################################################################################
    * 상태 기록 또는 초기화
    * ################################################################################################## */
    public void Record()
    {
        if (enabled)
        {

            // 최대 기록 개수를 초과하면 가장 오래된 기록 제거
            if (recordList.Count >= recordLimit)
            {
                recordList.RemoveAt(recordList.Count - 1);
            }

            /* 현재 상태(위치, 회전)를 새로운 Recorder 구조체로 만들어 리스트의 맨 앞(0번 인덱스)에 추가 */
            // 필요에 따라 현재 상태에 다른 요소 추가 가능
            recordList.Insert(0, new Recorder(transform.position, transform.rotation));
        }

        else if (!enabled)
        {
            // 기록 리스트 초기화
            if (recordList.Count > 0) { recordList.Clear(); }
        }
    }
}
