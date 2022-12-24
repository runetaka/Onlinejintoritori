using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Fieldcolor : MonoBehaviourPunCallbacks, IPunInstantiateMagicCallback
{
    public GameObject parentObject;
    public Material[] fieldmaterials;

    Dictionary<string, Vector3> Prefpos = new Dictionary<string, Vector3>
        {
         {"��t",new Vector3(2.21121192f,1.52016139f,10.008172f)},
         {"����",new Vector3(-0.488788128f,-1.24983859f,10.008172f)},
         {"�Ȗ�",new Vector3(-1.87878823f,2.19016147f,10.008172f)},
         {"���",new Vector3(-0.958788157f,0.0571363568f,10.008172f)},
         {"���",new Vector3(2.21121192f,1.52016139f,10.008172f)},
         {"�Q�n",new Vector3(0.741211891f,2.7601614f,10.008172f)},
         {"�_�ސ�",new Vector3(-0.784505904f,-2.50983858f,10.008172f)},


        };
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
        //else
        //{
            

            //if (this.photonView.IsRoomView) //master�͂Ȃɂ����Ȃ��Aothers�͈�̌��𐶐��A�������łɐ����������ǂ����𒲂ׂ�
            //{
                
            //    var myPos = Prefpos["�Ȗ�"];
            //    var field = PhotonNetwork.Instantiate("��t", myPos, Quaternion.identity);
            //    //field.GetComponent<Renderer>().material = fieldcolor["Red"];
            //    Sprite sprite = Resources.Load<Sprite>("Sprites/�Ȗ�");
            //    field.GetComponent<SpriteRenderer>().sprite = sprite;
            //    field.name = "�Ȗ�";
            //        Debug.Log("����");
            //        Destroy(this);
               
                

            //}

            
        //}
        


    }

    
}

