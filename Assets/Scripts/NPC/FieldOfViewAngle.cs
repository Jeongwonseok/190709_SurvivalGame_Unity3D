using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfViewAngle : MonoBehaviour
{
    [SerializeField] private float viewAngle; // 시야각 (120도)
    [SerializeField] private float viewDistance; // 시야 거리 (10미터)
    [SerializeField] private LayerMask targetMask; // 타겟 마스크 (플레이어)

    // 필요한 컴포넌트
    private Pig thePig;

    void Start()
    {
        thePig = GetComponent<Pig>();  
    }

    // Update is called once per frame
    void Update()
    {
        View();
    }

    // 각도 변수에 현재 위치의 y축값 더하기
    // 삼각함수 이용해서 (sin,cos) 리턴
    private Vector3 BoundaryAngle(float _angle)
    {
        _angle += transform.eulerAngles.y;
        return new Vector3(Mathf.Sin(_angle * Mathf.Deg2Rad), 0f, Mathf.Cos(_angle * Mathf.Deg2Rad));
    }

    // 
    private void View()
    {
        // 좌,우 시야각 구현
        Vector3 _leftBoundary = BoundaryAngle(-viewAngle * 0.5f);
        Vector3 _rightBoundary = BoundaryAngle(viewAngle * 0.5f);

        // 씬에서만 확인할수있는 레이저
        Debug.DrawRay(transform.position + transform.up, _leftBoundary, Color.red);
        Debug.DrawRay(transform.position + transform.up, _rightBoundary, Color.red);

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
                            thePig.Run(_hit.transform.position); // thePig 객체의 Run 메서드 실행
                        }
                    }
                }
            }
        }
    }
}
