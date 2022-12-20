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
                Debug.Log("���g���l�b�g���[�N�I�u�W�F�N�g�𐶐����܂���");
                transform.GetChild(0).GetComponent<Renderer>().material = Node.instance.materials[1];
                GetComponent<Renderer>().material = Node.instance.fieldmaterials[1];

                transform.GetChild(0).GetComponent<Node>().fraction = Fraction.PLAYER;

            }
            else
            {
                Debug.Log("���v���C���[���l�b�g���[�N�I�u�W�F�N�g�𐶐����܂���");
                transform.GetChild(0).GetComponent<Renderer>().material = Node.instance.materials[2];
                GetComponent<Renderer>().material = Node.instance.fieldmaterials[2];
                //�ǉ�
                //Fieldsetting();

                //�ǉ�
                //transform.parent.GetComponent<Renderer>().material = fieldmaterials[2];
                //EnemyField();
                transform.GetChild(0).GetComponent<Node>().fraction = Fraction.ENEMY;

                //�ǉ��@�ʒu�ύX
                //var enemyposition = new Vector3(0,6,0);
                //Enemypos = enemyposition;
                //this.Pos = new Vector3(0, -5, 0);


            }
        }
        


    }

    
}

