using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FieldOfViewAngle : MonoBehaviour
{
    [SerializeField] private float viewAngle; // 시야각 (120도)
    [SerializeField] private float viewDistance; // 시야 거리 (10미터)
    [SerializeField] private LayerMask targetMask; // 타겟 마스크 (플레이어)

    private PlayerController thePlayer;
    private NavMeshAgent nav;

    void Start()
    {
        thePlayer = FindObjectOfType<PlayerController>();
        nav = GetComponent<NavMeshAgent>();
        
    }

    public Vector3 GetTargetPos()
    {
        return thePlayer.transform.position;
    }

    public bool View()
    {
        // 거리내에 있는 물체 인식하고 배열에 집어넣기
        Collider[] _target = Physics.OverlapSphere(transform.position, viewDistance, targetMask);

        // 타겟 이름이 "Player" 이면 시야내에 있는지 판별 후 돼지 도망치게하기  
        for (int i = 0; i < _target.Length; i++)
        {
            Transform _targetTf = _target[i].transform;
            if(_targetTf.name == "Player")
            {
                Vector3 _direction = (_targetTf.position - transform.position).normalized;
                float _angle = Vector3.Angle(_direction, transform.forward);

                // 만약 시야각 내에 있으면
                if(_angle < viewAngle * 0.5f)
                {
                    // 만약 시야거리 내에 있으면
                    RaycastHit _hit;
                    if (Physics.Raycast(transform.position + transform.up, _direction, out _hit, viewDistance))
                    {
                        // 만약 이름이 "Player" 이면 
                        if (_hit.transform.name == "Player")
                        {
                            Debug.Log("플레이어가 돼지 시야 내에 있습니다.");
                            Debug.DrawRay(transform.position + transform.up, _direction, Color.blue);
                            return true;
                            // thePig.Run(_hit.transform.position); // thePig 객체의 Run 메서드 실행
                        }
                    }
                }
            }

            if(thePlayer.GetRun())
            {
                if(CalcPathLength(thePlayer.transform.position) <= viewDistance)
                {
                    Debug.Log("돼지가 주변에서 뛰고있는 플레이어의 움직임을 파악했습니다.");
                    return true;
                }
            }
        }
        return false;
    }

    private float CalcPathLength(Vector3 _targetPos)
    {
        // 경로 기억하도록 객체 생성
        NavMeshPath _path = new NavMeshPath();
        nav.CalculatePath(_targetPos, _path);

        // 배열 크기 지정
        Vector3[] _wayPoint = new Vector3[_path.corners.Length + 2];

        _wayPoint[0] = transform.position;
        _wayPoint[_path.corners.Length + 1] = _targetPos;

        float _pathLength = 0;
        for (int i = 0; i < _path.corners.Length; i++)
        {
            _wayPoint[i + 1] = _path.corners[i]; // 웨이포인트의 경로 넣기
            _pathLength += Vector3.Distance(_wayPoint[i], _wayPoint[i + 1]); // 경로 길이 계산
        }

        return _pathLength;
    }
}
