using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class DragDrop_Photon : MonoBehaviourPun
{
    public GameObject selectedPiece;
    bool isClick;
    Ray ray;
    RaycastHit hit;
    LineRenderer lr;
    JigsawPuzzle_Photon jp;
    public PiecesScripts_Photon[] jigsPuz;
    public Transform RayPosition;
    public GameObject ClearUI;
    void Start()
    {
        ClearUI = GameObject.Find("Clear Canvas").transform.GetChild(0).gameObject;
        lr = GetComponent<LineRenderer>();
        jp = GameObject.Find("JigsawPuzzles").GetComponent<JigsawPuzzle_Photon>();
        jigsPuz = new PiecesScripts_Photon[jp.transform.childCount];
        for (int i = 0; i < jp.transform.childCount; i++)
        {
            jigsPuz[i] = jp.transform.GetChild(i).transform.GetComponent<PiecesScripts_Photon>();
            jigsPuz[i].SetPuzIndex(i);
        }
    }

    void Update()
    {
        DrawGuideLine();
        ObjectRay_Photon();
    }

    void DrawGuideLine()
    {
        if (photonView.IsMine)
        {
            //���̿� �ε��� �����
            if (Physics.Raycast(ray, out hit))
            {
                //�Ÿ��� ���ؼ� ������ �׸���.
                lr.SetPosition(0, RayPosition.position);
                lr.SetPosition(1, hit.point);
            }
            else
            {
                lr.SetPosition(0, RayPosition.position);
                lr.SetPosition(1, RayPosition.position + RayPosition.forward * 1);
            }
        }
    }

    bool isTrigger = false;
    int jigsIndex = 0;
    void ObjectRay_Photon()
    {
        if (photonView.IsMine == false) return;
        ray = new Ray(RayPosition.position, RayPosition.forward);
        float v = OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, OVRInput.Controller.RTouch);
        if (v > 0)
        {
            //selectedPiece = null;
            if (Physics.Raycast(ray, out hit, 100))
            {
                if (!isClick)
                {
                    jigsIndex = hit.transform.GetComponent<PiecesScripts_Photon>().puzIndex;
                    isClick = true;

                    //����Ȯ�ο� ī��Ʈ(���̳ʽ�)
                    photonView.RPC("RPC_CheckMinusQuestion", RpcTarget.All, jigsPuz[jigsIndex].Checkdist() == 0);
                }


                int layer = 1 << LayerMask.NameToLayer("Puzzle");
                if (Physics.Raycast(ray, out hit, 100, layer))
                {
                    float x = hit.point.x;
                    float y = hit.point.y;
                    //print(selectedPiece);
                    //���콺 ��Ŭ���� ������ 
                    //Ŭ���� ���� GameObject�� ������ �� �ְ� ����
                    //selectedPiece = hit.transform.gameObject;
                    //selectedPiece.transform.position = new Vector3(hit.point.x, hit.point.y, -0.1f); //<=Ʈ���� ��� ���� z���� �̵����� �ʰ� �ϰ� �ʹ�.
                    photonView.RPC("RPC_DragPuzzle", RpcTarget.All, x, y, jigsIndex);
                    isTrigger = true;

                }

            }
        }

        else if (isTrigger == true && v <= 0)
        {
            isClick = false;
            isTrigger = false;



            //����Ȯ�ο� ī��Ʈ
            photonView.RPC("RPC_CheckQuestion", RpcTarget.All , jigsPuz[jigsIndex].CheckCount() == true);


            //���࿡ ����ڰ� ����� ���� ������ �ٽ� �Ű�ٰ� ������ ī��Ʈ�� �߰��� ��������.
            //�̰� �������� ��� �ؾ� �ұ�?
            

            photonView.RPC("RPC_ClearUI", RpcTarget.All);

        }





        //if (ButtonManager.instance.settingUI.activeSelf && v > 0)
        //{
        //    if (Physics.Raycast(ray, out hit))
        //    {
        //        if (hit.transform.gameObject.name.Contains("Resume"))
        //        {
        //            ButtonManager.instance.OnClickResume();
        //        }
        //        if (hit.transform.gameObject.name.Contains("Retry"))
        //        {
        //            ButtonManager.instance.OnClickRetry();
        //        }
        //        if (hit.transform.gameObject.name.Contains("SelectMenu"))
        //        {
        //            ButtonManager.instance.OnClickSelectMenu();
        //        }
        //        if (hit.transform.gameObject.name.Contains("ExitGame"))
        //        {
        //            ButtonManager.instance.OnClickExitGame();
        //        }
        //        return;
        //    }
        //}
    }
    [PunRPC]
    void RPC_ClearUI()
    {
        if (jp.DragSign_Photon())
            ClearUI.SetActive(true);
    }

    //����Ȯ�ο� RPC(+)
    [PunRPC]
    void RPC_CheckQuestion(bool cq)
    {
        if (cq)
            jp.answerCnt++;
    }
    //����Ȯ�ο� RPC(-)
    [PunRPC]
    void RPC_CheckMinusQuestion(bool cq)
    {
        if (cq)  //���� ��ġ���� ���ٸ� 
            jp.answerCnt--;
    }

    [PunRPC]
    void RPC_DragPuzzle(float x, float y, int index)
    {
        //jigsPuz[0].transform.position += Vector3.right*0.01f;
        //print(index);
        jigsPuz[index].transform.position = new Vector3(x, y, -0.1f); //<=Ʈ���� ��� ���� z���� �̵����� �ʰ� �ϰ� �ʹ�.
    }
}








