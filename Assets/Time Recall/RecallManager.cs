using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* ##################################################################################################
 * 1. 스크립트명: RecallManager.cs
 * 2. 역할: 기록 및 시간 역행 관련 변수 및 함수 담당
 * ################################################################################################## */

public class RecallManager : MonoBehaviour
{
    // 싱글톤 패턴을 위한 인스턴스
    private static RecallManager instance;

    /*##################################################################################################
    * Action 델리게이트
    * ################################################################################################## */
    // 오브젝트의 위치 기록을 실행할 때 호출되는 Action 델리게이트
    public static Action Recorder;

    // 시간 역행을 실행할 때 호출되는 Action 델리게이트
    public static Action<float, bool> Recaller; // (파라미터: lerpTime[보간 시간], endOfFrame[새로운 기록 시점 도달 여부])

    /*##################################################################################################
    * Recaller 컴포넌트 리스트 및 딕셔너리
    * ################################################################################################## */

    // Recaller 컴포넌트들을 저장해두는 리스트
    public static List<Recaller> RecallerList { get; set; } = new List<Recaller>();

    // Recaller들을 대상으로 <게임 오브젝트, Recaller 컴포넌트>의 형태로 저장하는 딕셔너리
    public static Dictionary<GameObject, Recaller> RecallerDctn { get; set; } = new Dictionary<GameObject, Recaller>();

    /*##################################################################################################
    * Recall 관련 함수
    * ################################################################################################## */
    [Tooltip("'frame' 변수는 반드시 1 보다 커야 함")]
    public int recordFrame = 1;    // 기록 주기(프레임 단위)
    private int frame = 0;         // 현재 프레임 카운트 변수

    /*##################################################################################################
    * 변수 초기화
    * ################################################################################################## */
    private void Awake()
    {
        // 싱글톤 인스턴스 설정
        instance = this;

        // recordFrame 초기화
        if (recordFrame <= 0) { recordFrame = 1; }
    }

    /*##################################################################################################
    * 기록 및 시간 역행 실행
    * ################################################################################################## */
    private void FixedUpdate()
    {
        // 기록 주기마다 실행
        if (!(recordFrame <= 0) && frame <= 0)
        {
            // 모든 Recaller의 기록 함수 호출
            Recorder?.Invoke();

            // 기록 주기 초기화
            frame = recordFrame;
        }

        // 보간 시간 계산 [현재 프레임이 다음 기록 시점까지 얼마나 진행 되었는지 계산]
        float lerpTime = (float)(recordFrame - (frame - 1)) / recordFrame;

        // 모든 Recaller의 시간 역행 함수 호출 (frame == 1 -> 새로운 기록 시점 도달)
        Recaller?.Invoke(lerpTime, frame == 1);
        frame -= 1;
    }

    /*##################################################################################################
    * Recall 관련 함수
    * ################################################################################################## */

    // 모든 Recaller의 시간 역행 기능 활성화 함수
    public static void RecallAll()
    {
        foreach (var Recaller in RecallerList)
        {
            Recaller.RecallEnable();
        }
    }

    public static void Recall(GameObject gameObject) { RecallerDctn[gameObject].RecallEnable(); }   // 파라미터의 "게임 오브젝트"에 연결된 Recaller의 시간 역행 기능 활성화
    public static void Recall(Recaller Recaller) { Recaller.RecallEnable(); }                       // 파라미터의 "Recaller 인스턴스"의 시간 역행 기능 활성화

    // 모든 Recaller의 최대 기록 개수 설정
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
    public static int GetRecordFrame() { return instance.recordFrame; }            // 현재 기록 주기 반환
    public static void SetRecordFrame(int frame) { instance.recordFrame = frame; } // 현재 기록 주기 설정
    
    public static Recaller GetRecaller(GameObject gameObject) { return RecallerDctn[gameObject]; } // 파라미터의 게임 오브젝트에 연결된 Recaller 반환
    public static Recaller[] GetRecallerAll() { return RecallerList.ToArray(); } // 모든 Recaller를 배열로 반환
}
