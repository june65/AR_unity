using System.Collections; 
using System.Collections.Generic; 
using UnityEngine; 
using UnityEngine.UI; 
using GoogleARCore; 

public class MeasureMgr : MonoBehaviour 
{ 
    public GameObject marker; 
    public Text lengthText; 

    private TrackableHit hit; 
    private TrackableHitFlags flag = TrackableHitFlags.Default; //? 

    // 탭 카운트 저장 
    private int tapCount = 0; 
    // 첫번째 탭의 위치(좌표) 
    private Vector3 firstPos = Vector3.zero; 

    void Update() 
    { 
        if (Input.touchCount == 0) return; // 첫번째 손가락 터치 정보를 저장 

        Touch touch = Input.GetTouch(0); // 두번째 터치 이후 아래 로직 처리하지 않음 

        if (tapCount == 2) return; 

        if (touch.phase == TouchPhase.Began 
            && Frame.Raycast(touch.position.x, touch.position.y, flag, out hit)) 
        { 
            ++tapCount; // 앵커 포인트 생성

            Anchor anchor = hit.Trackable.CreateAnchor(hit.Pose); 
            Instantiate(marker, // 생성할 마커 
                    hit.Pose.position, // 생성 위치 
                    Quaternion.LookRotation(hit.Pose.up), // 각도 
                    anchor.transform); // 부모 객체 
            if (tapCount == 2) 
            { 
                DisplayLength(firstPos, hit.Pose.position); 
            } 
            else 
            { 
                firstPos = anchor.transform.position; 
            } 
        } 
                
    } 

    void DisplayLength(Vector3 _firstPos, Vector3 _secondPos) 
    { 
        lengthText.text = Vector3.Distance(_firstPos, _secondPos).ToString("000.00"); 
    } 

    public void OnInitMarker() 
    { 
        GameObject[] markers = GameObject.FindGameObjectsWithTag("MARKER"); 
        foreach (var marker in markers) 
        { 
            Destroy(marker);
        } 
        lengthText.text = ""; 
        firstPos = Vector3.zero; 
        tapCount = 0; 
    } 
}