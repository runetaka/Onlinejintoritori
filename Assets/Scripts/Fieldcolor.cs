using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Fieldcolor : MonoBehaviourPunCallbacks, IPunInstantiateMagicCallback
{
    public GameObject parentObject;
    public Material[] fieldmaterials;
    // Start is called before the first frame update
 
    void IPunInstantiateMagicCallback.OnPhotonInstantiate(PhotonMessageInfo info)
    {
        if (!photonView.IsRoomView)
        {
            if (info.Sender.IsLocal)
            {
                Debug.Log("自身がネットワークオブジェクトを生成しました");
                transform.GetChild(0).GetComponent<Renderer>().material = Node.instance.materials[1];
                GetComponent<Renderer>().material = Node.instance.fieldmaterials[1];

                transform.GetChild(0).GetComponent<Node>().fraction = Fraction.PLAYER;

            }
            else
            {
                Debug.Log("他プレイヤーがネットワークオブジェクトを生成しました");
                transform.GetChild(0).GetComponent<Renderer>().material = Node.instance.materials[2];
                GetComponent<Renderer>().material = Node.instance.fieldmaterials[2];
                //追加
                //Fieldsetting();

                //追加
                //transform.parent.GetComponent<Renderer>().material = fieldmaterials[2];
                //EnemyField();
                transform.GetChild(0).GetComponent<Node>().fraction = Fraction.ENEMY;

                //追加　位置変更
                //var enemyposition = new Vector3(0,6,0);
                //Enemypos = enemyposition;
                //this.Pos = new Vector3(0, -5, 0);


            }
        }
        


    }

    
}

