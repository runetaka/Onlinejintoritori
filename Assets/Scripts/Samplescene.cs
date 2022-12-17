using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using System.Collections.Generic;

// MonoBehaviourPunCallbacks���p�����āAPUN�̃R�[���o�b�N���󂯎���悤�ɂ���
public class Samplescene : MonoBehaviourPunCallbacks
{
    Vector3[] playerpos = new Vector3[2];
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

    Dictionary<string, Vector3> nodePos = new Dictionary<string, Vector3>  //node�̂�[����|�W�V����
        {
         {"��t",new Vector3(0, 0, 0)},
         {"����",new Vector3(0, 0, 0)},
         {"�Ȗ�",new Vector3(0, 0, 0)},
         {"���",new Vector3(0, 0, 0)},
         {"���",new Vector3(0, 0, 0)},
         {"�Q�n",new Vector3(0, 0, 0)},
         {"�_�ސ�",new Vector3(0, 0, 0)},

        };

    
    

    public Material[] materials;
    public Material[] fieldmaterials;

    Sprite Prefstatas;
    public Sprite[] prefsprites;

   
    //node�̂�[����|�W�V����
       
    private void Start()
    {
        //Title�������ꍇ
        //if (PhotonNetwork.IsConnected)
        //{
        //    NodeInstantiate();
        //}
        // PhotonServerSettings�̐ݒ���e���g���ă}�X�^�[�T�[�o�[�֐ڑ�����
        PhotonNetwork.ConnectUsingSettings();

    }

    // �}�X�^�[�T�[�o�[�ւ̐ڑ��������������ɌĂ΂��R�[���o�b�N
    public override void OnConnectedToMaster()
    {
        // "Room"�Ƃ������O�̃��[���ɎQ������i���[�������݂��Ȃ���΍쐬���ĎQ������j
        PhotonNetwork.JoinOrCreateRoom("Room", new RoomOptions(), TypedLobby.Default);

    }

    // �Q�[���T�[�o�[�ւ̐ڑ��������������ɌĂ΂��R�[���o�b�N
    public override void OnJoinedRoom()
    {
        NodeInstantiate();
    }
    // �����_���ȍ��W�Ɏ��g�̃A�o�^�[�i�l�b�g���[�N�I�u�W�F�N�g�j�𐶐�����
    //var position = new Vector3(0,0,0);
    public void NodeInstantiate()
    {

        Dictionary<string, Material> fieldcolor = new Dictionary<string, Material>
    {
        {"Glay", fieldmaterials[0]},
        {"Red", fieldmaterials[1]},
        {"Blue", fieldmaterials[2]},


    };


        Vector3 position = PlayerPoint();
        //if (PhotonNetwork.IsMasterClient)
        //{
        var field = PhotonNetwork.Instantiate("��t", position, Quaternion.identity);
        //var node = PhotonNetwork.Instantiate("Node", position, Quaternion.identity);
        field.GetComponent<Renderer>().material = fieldcolor["Red"];
        Sprite sprite = Resources.Load<Sprite>("Sprites/��t");
        field.GetComponent<SpriteRenderer>().sprite = sprite;

        //}
        var dic = this.Prefpos;
        dic.Remove("��t");




        //Vector3[] positions =
        //    {
        //        new Vector3(0, 0, 0),
        //        new Vector3(2.33f, -1.34f, 0),
        //        new Vector3(4.66f, 0, 0),
        //        new Vector3(-4.66f, 0, 0),
        //        new Vector3(-2.33f, -1.34f, 0),
        //        new Vector3(-2.33f, 1.34f, 0),
        //        new Vector3(2.33f, 1.34f, 0)

        //    };

        if (PhotonNetwork.IsMasterClient)
        {
            foreach (var (name,fieldPos) in dic)
            {
            
                //var Npcnode = PhotonNetwork.InstantiateRoomObject("Node", fieldPos, Quaternion.identity);
                //Vector3 localPos = nodePos[name];
                var NpcField = PhotonNetwork.InstantiateRoomObject("��t", fieldPos, Quaternion.identity);
                NpcField.GetComponent<Renderer>().material = fieldcolor["Glay"];
                Sprite fieldsprite = Resources.Load<Sprite>("Sprites/" + name);
                NpcField.GetComponent<SpriteRenderer>().sprite = fieldsprite;
                NpcField.name = name;
                Node node = NpcField.transform.GetChild(0).gameObject.GetComponent<Node>();
                node.setSprite(name);

            }
            
        }

        //foreach (Sprite pre in prefsprites)
        //{
            
            
            
        //    foreach (Vector3 roomposition in positions)
        //    {
        //        Npcnode = PhotonNetwork.InstantiateRoomObject("Node", roomposition, Quaternion.identity);
        //        NpcField = PhotonNetwork.InstantiateRoomObject("�_�ސ�", roomposition, Quaternion.identity);
        //        Npcnode.transform.parent = NpcField.transform;
        //        NpcField.transform.position = roomposition + new Vector3(0, 0, 3);
        //        Npcnode.transform.position = roomposition + new Vector3(0, 0, -3);
        //        Prefstatas = pre;
        //        NpcField.GetComponent<SpriteRenderer>().sprite = Prefstatas;
        //        Debug.Log("����");
        //        break;
        //    }

        //}


        






        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.CurrentRoom.SetStartTime(PhotonNetwork.ServerTimestamp);
        }
    }

       
    public Vector3 PlayerPoint()
    {
        playerpos[0] = new Vector3(2.41121197f, -2.11983871f, 10.008172f);
        playerpos[1] = new Vector3(-1, -1, 0);
        return playerpos[Random.Range(0, playerpos.Length)];
    }

}