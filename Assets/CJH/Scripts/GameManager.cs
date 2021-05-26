using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    Rigidbody rigid;                    //hit 게임 오브젝트의 rigid 컴포넌트
    public GameObject [] preView;       //퍼즐 프리뷰 배열
    int preViewIndex;                   //선택한 퍼즐의 인덱스
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        //if (Input.GetMouseButtonDown(0))
        //{
        //    if(Physics.Raycast(ray , out hit))
        //    {
        //        rigid = hit.transform.GetComponent<Rigidbody>();
        //        Vector3 dir = Camera.main.transform.position - hit.transform.position;
        //        rigid.AddForce(dir * 0.2f, ForceMode.Impulse);

        //    }
        //}

        //if (Input.GetMouseButtonDown(1))
        //{
        //    if(Physics.Raycast(ray , out hit))
        //    {
        //        rigid = hit.transform.GetComponent<Rigidbody>();
        //        hit.transform.position = transform.position;
        //        rigid.isKinematic = true;
        //    }
        //}
        if(Physics.Raycast(ray , out hit))
        {
            if (Input.GetMouseButtonDown(0))                    //오른손으로 클릭 시 멈추게 
            {                                 
                for (int i = 0; i < preView.Length; i++)
                {
                    if (preView[i].name == hit.transform.gameObject.name) //프리뷰 인덱스 저장 및 Catch상태로 변환 
                    {
                        rigid = hit.transform.GetComponent<Rigidbody>();
                        PuzzleManager.instance.state = PuzzleManager.PuzzleState.Catch; //Catch상태 전환
                        rigid.isKinematic = true;                        //물리 효과 적용 상태로 전환
                        preViewIndex = i;
                        break;
                    }
                }
            }
            else if (Input.GetKey(KeyCode.Space))               //멈춘 물체를 끌어당기기
            {
                rigid.isKinematic = false;
                Vector3 dir = Camera.main.transform.position - hit.transform.position;
                rigid.AddForce(dir * 0.05f, ForceMode.Impulse);
            }
            else if (Input.GetKeyUp(KeyCode.Space))
            {
                PuzzleManager.instance.state = PuzzleManager.PuzzleState.Revolution;
            }
            
            
            if (Input.GetMouseButton(1))                //왼손
            {
                if (hit.transform.gameObject.name == "Canvas")  //프리뷰 생성 위치 지정 캔버스 한정
                {
                    preView[preViewIndex].SetActive(true);

                    int x = (int)hit.point.x;
                    int y = (int)hit.point.y;

                    preView[preViewIndex].transform.position = new Vector2(x, y);
                }
            }
            else if (Input.GetMouseButtonUp(1))                //지정 된 위치로 물체 보내기 및 프리뷰 비활성화
            {
                PuzzleManager.instance.state = PuzzleManager.PuzzleState.Catch;
                rigid.isKinematic = false;                     //잡았을 때 true 상태이므로 전환 시켜줌.
                Vector3 dir = preView[preViewIndex].transform.position - rigid.transform.position;
                
                rigid.AddForce(dir * 1, ForceMode.Impulse);
                preView[preViewIndex].SetActive(false);
            }
        }
    }
}
