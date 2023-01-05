using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

// MonoBehaviourPunCallbacksを継承して、PUNのコールバックを受け取れるようにする
public class Samplescene : MonoBehaviourPunCallbacks
{
    List<string> room = new List<string>();

    Vector3[] playerpos = new Vector3[2];
    Dictionary<string, Vector3> Prefpos = new Dictionary<string, Vector3>
        {
         {"千葉",new Vector3(2.21121192f,1.52016139f,10.008172f)},  
         {"東京",new Vector3(-0.488788128f,-1.24983859f,10.008172f)},   
         {"栃木",new Vector3(-1.87878823f,2.19016147f,10.008172f)},   
         {"埼玉",new Vector3(-0.958788157f,0.0571363568f,10.008172f)},   
         {"茨城",new Vector3(2.21121192f,1.52016139f,10.008172f)},  
         {"群馬",new Vector3(0.741211891f,2.7601614f,10.008172f)},  
         {"神奈川",new Vector3(-0.784505904f,-2.50983858f,10.008172f)},
         

        };

    Dictionary<string, Vector3> nodePos = new Dictionary<string, Vector3>  //nodeのろーかるポジション
        {
         {"千葉",new Vector3(0, 0, 0)},
         {"東京",new Vector3(0, 0, 0)},
         {"栃木",new Vector3(0, 0, 0)},
         {"埼玉",new Vector3(0, 0, 0)},
         {"茨城",new Vector3(0, 0, 0)},
         {"群馬",new Vector3(0, 0, 0)},
         {"神奈川",new Vector3(0, 0, 0)},

        };



    List<string> npcname = new List<string>();
    public Material[] materials;
    
    public Material[] fieldmaterials;

    Sprite Prefstatas;
    public Sprite[] prefsprites;
    


    //nodeのろーかるポジション

    private void Start()
    {
        //Titleから入る場合
        if (PhotonNetwork.IsConnected)
        {
            NodeInstantiate();
            PhotonNetwork.OfflineMode = true;
        }
        else
        {
            SceneManager.LoadScene(0);
        }

        //PhotonServerSettingsの設定内容を使ってマスターサーバーへ接続する

        
        //PhotonNetwork.ConnectUsingSettings();



    }



    // マスターサーバーへの接続が成功した時に呼ばれるコールバック
    //public override void OnConnectedToMaster()
    //{
    //    //    // "Room"という名前のルームに参加する（ルームが存在しなければ作成して参加する）
    //    PhotonNetwork.JoinOrCreateRoom("Room", new RoomOptions(), TypedLobby.Default);


    //}

    private void Update()
    {
        double photonTime = PhotonNetwork.Time;
    }

    // ゲームサーバーへの接続が成功した時に呼ばれるコールバック
    //public override void OnJoinedRoom()
    //{
    //    NodeInstantiate();
        


    //}
    // ランダムな座標に自身のアバター（ネットワークオブジェクト）を生成する
    //var position = new Vector3(0,0,0);
    public void NodeInstantiate()
    {
        
        

        Dictionary<string, Material> fieldcolor = new Dictionary<string, Material>
    {
        {"Glay", fieldmaterials[0]},
        {"Red", fieldmaterials[1]},
        {"Blue", fieldmaterials[2]},


    };


        Vector3 position = new Vector3(2.41121197f, -2.11983871f, 10.008172f);
        if (PhotonNetwork.IsMasterClient)
        {
        var field = PhotonNetwork.Instantiate("千葉", position, Quaternion.identity);
        //var node = PhotonNetwork.Instantiate("Node", position, Quaternion.identity);
        field.GetComponent<Renderer>().material = fieldcolor["Red"];
        Sprite sprite = Resources.Load<Sprite>("Sprites/千葉");
        field.GetComponent<SpriteRenderer>().sprite = sprite;

        }
        var dic = this.Prefpos;
        //dic.Remove("栃木");
        dic.Remove("千葉");
        dic.Remove("栃木");




        Vector3[] positions =
            {
                new Vector3(0, 0, 0),
                new Vector3(2.33f, -1.34f, 0),
                new Vector3(4.66f, 0, 0),
                new Vector3(-4.66f, 0, 0),
                new Vector3(-2.33f, -1.34f, 0),
                new Vector3(-2.33f, 1.34f, 0),
                new Vector3(2.33f, 1.34f, 0)

            };

        if (PhotonNetwork.IsMasterClient)
        {
           
            foreach (var (name,fieldPos) in dic)
            {
            
                //var Npcnode = PhotonNetwork.InstantiateRoomObject("Node", fieldPos, Quaternion.identity);
                //Vector3 localPos = nodePos[name];
                var NpcField = PhotonNetwork.InstantiateRoomObject("千葉", fieldPos, Quaternion.identity);
                NpcField.GetComponent<Renderer>().material = fieldcolor["Glay"];
                Sprite fieldsprite = Resources.Load<Sprite>("Sprites/" + name);
                NpcField.GetComponent<SpriteRenderer>().sprite = fieldsprite;
                NpcField.name = name;
                Node node = NpcField.transform.GetChild(0).gameObject.GetComponent<Node>();
                node.setSprite(name);
                npcname.Add(name);

            }
            

        }

        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.CurrentRoom.SetStartTime(PhotonNetwork.ServerTimestamp);
        }
        else
        {
            Prefpos.Add("栃木", new Vector3(-1.87878823f, 2.19016147f, 10.008172f));
            //Debug.Log("栃木");
            var myPos = Prefpos["栃木"];
            var field = PhotonNetwork.Instantiate("千葉", myPos, Quaternion.identity);
            field.GetComponent<Renderer>().material = fieldcolor["Red"];
            Sprite sprite = Resources.Load<Sprite>("Sprites/栃木");
            field.GetComponent<SpriteRenderer>().sprite = sprite;
            Node node = field.transform.GetChild(0).gameObject.GetComponent<Node>();
            node.setSprite("栃木");
            if (PhotonNetwork.IsMasterClient)
            {
                Destroy(field.gameObject);
            }
            
        }
    }

       
    public Vector3 PlayerPoint()
    {
        playerpos[0] = new Vector3(2.41121197f, -2.11983871f, 10.008172f);
        //playerpos[1] = new Vector3(-1, -1, 0);
        return playerpos[Random.Range(0, playerpos.Length)];
    }

}