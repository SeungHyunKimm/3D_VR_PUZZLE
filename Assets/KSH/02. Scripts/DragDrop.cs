using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragDrop : MonoBehaviour
{
    public GameObject selectedPiece;
    bool isClick;
    Ray ray;
    RaycastHit hit;
    LineRenderer lr;
    JigsawPuzzle jp;
    AudioSource btnSound;

    void Start()
    {

        lr = GetComponent<LineRenderer>();
        jp = GameObject.Find("JigsawPuzzle").GetComponent<JigsawPuzzle>();
        btnSound = GetComponent<AudioSource>();

    }

    void Update()
    {
        DrawGuideLine();
        ObjectRay();
    }

    void DrawGuideLine()
    {
        //���̿� �ε��� �����
        if (Physics.Raycast(ray, out hit))
        {
            //�Ÿ��� ���ؼ� ������ �׸���.
            lr.SetPosition(0, transform.position);
            lr.SetPosition(1, hit.point);
        }
        else
        {
            lr.SetPosition(0, transform.position);
            lr.SetPosition(1, transform.position + transform.forward * 1);
        }
    }

    bool isTrigger = false;
    void ObjectRay()
    {
        ray = new Ray(transform.position, transform.forward);
        float v = OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, OVRInput.Controller.RTouch);
        if (v > 0)
        {
            
            selectedPiece = null;
            if (Physics.Raycast(ray, out hit, 100))
            {
                
                    isClick = true;
                    print(hit.transform.gameObject);
                    selectedPiece = hit.transform.gameObject;
                    //for (int i = 0; i < 36; i++)
                    //{
                        int layer = 1 << LayerMask.NameToLayer("Puzzle");
                        if (Physics.Raycast(ray, out hit, 100, layer))
                        {
                            //print(selectedPiece);
                            //���콺 ��Ŭ���� ������ 
                            //Ŭ���� ���� GameObject�� ������ �� �ְ� ����
                            selectedPiece.transform.position = new Vector3(hit.point.x, hit.point.y, -0.1f); //<=Ʈ���� ��� ���� z���� �̵����� �ʰ� �ϰ� �ʹ�.
                            isTrigger = true;
                        }
                    //}
                
            }
        }
        else if (isTrigger == true && v <= 0)
        {
            isClick = false;
            jp.DragSign();
            isTrigger = false;
            print("d����");
            btnSound.Play();
        }


        if (ButtonManager.instance.settingUI.activeSelf && v > 0)
        {
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.gameObject.name.Contains("Resume"))
                {
                    ButtonManager.instance.OnClickResume();
                }
                if (hit.transform.gameObject.name.Contains("Retry"))
                {
                    ButtonManager.instance.OnClickRetry();
                }
                if (hit.transform.gameObject.name.Contains("SelectMenu"))
                {
                    ButtonManager.instance.OnClickSelectMenu();
                }
                if (hit.transform.gameObject.name.Contains("ExitGame"))
                {
                    ButtonManager.instance.OnClickExitGame();
                }
                return;
            }
        }
    }
}








