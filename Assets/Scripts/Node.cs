using System.Collections;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

using TMPro;
public enum Fraction
    {
        NPC,
        ENEMY,
        PLAYER
    }

public class Node : MonoBehaviourPunCallbacks, IPunObservable ,IPunInstantiateMagicCallback
{
    int maxAmount;
    int startAmount = 10;
    int currentAmount;
    


    public Material[] materials;
   


    public TMP_Text amountText;
    public enum NodeType
    {
        BOSS_CITY,
        BIG_CITY,
        MEDIUM_CITY,
        SMALL_CITY
    }

    

    public Fraction fraction; 

    public NodeType type;

    public GameObject unitPrefab;
    private Vector3 Pos;

    void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            this.name = Random.Range(0, 10).ToString();

        }


        currentAmount = startAmount;

        switch (type)

        {
            case NodeType.SMALL_CITY:
                {
                    maxAmount = 10;
                }
                break;

            case NodeType.MEDIUM_CITY:
                {
                    maxAmount = 20;
                }
                break;

            case NodeType.BIG_CITY:
                {
                    maxAmount = 50;
                }
                break;

            case NodeType.BOSS_CITY:
                {
                    maxAmount = 100;
                }
                break;
        }
        if (this.fraction == Fraction.PLAYER){
            StartCoroutine(Produce());
            UpdateAmountText();
        }

        //Debug.Log(photonView.Owner.UserId);
    }
    

    void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        //Debug.Log("発生");
        if (stream.IsWriting  && this.photonView.IsMine)
        {
            
            stream.SendNext(this.name);
            stream.SendNext(currentAmount);
           Debug.Log("send");
            
        }
        else
        {
            this.name = (string)stream.ReceiveNext();
            currentAmount = (int)stream.ReceiveNext();
            UpdateAmountText();
            Debug.Log("received");
        }
    }

    void Produceunit()
    {
        //Debug.Log("数を増やします");
        currentAmount++;
        if(currentAmount>maxAmount)
        {
            currentAmount = maxAmount;
        }

    UpdateAmountText(); 

    } 
    IEnumerator Produce()
        {
            while(true)
            {
                yield return new WaitForSeconds(1);
                Produceunit();
            }
        }

    void UpdateAmountText()
    {
       // Debug.Log("玉の情報を更新します。");
        amountText.text = currentAmount+"|"+ maxAmount;
    }
    
    IEnumerator SendALLunits(string _tag)
    {
        Node goalNode = GameObject.Find(_tag).GetComponent<Node>();

        int unitsToSend = currentAmount;
        while(unitsToSend > 0)
        {
            int unitsPerSend = unitsToSend <= 5 ? unitsToSend : 5;

            AllUnit(goalNode,unitsPerSend);
            unitsToSend -= unitsPerSend;
            currentAmount -= unitsPerSend;
            yield return new WaitForSeconds(0.5f);
            UpdateAmountText();
            

        }
         
        
    }

    public void AllUnit(Node goalNode, int numberOfUnits)
    {
        for (int i = 0; i < numberOfUnits; i++)
        {
            Debug.Log(i);
            Pos = this.transform.position;
            GameObject newUnit = Instantiate(unitPrefab, Pos, Quaternion.identity);
            Unit unit = newUnit.GetComponent<Unit>();
            unit.senderID = photonView.OwnerActorNr;
            float intervalDeg = 10f;
            float minDeg = -(numberOfUnits - 1) * intervalDeg / 2;
            float degree = (minDeg + intervalDeg * (float)i) / 180f * Mathf.PI;

            newUnit.GetComponent<Unit>().SetUnit(fraction, goalNode, GetComponent<SpriteRenderer>().material,degree);

        }

    }
    [PunRPC]
    public void SendUnits(string tag)
    {
        if (photonView.IsMine)
        {
            photonView.RPC(nameof(SendUnits), RpcTarget.Others, tag);    
        }
        StartCoroutine(SendALLunits(tag));

    }

    

    public void HandleIncomingUnit(Fraction f,int senderID)
    {
        Debug.Log(f);
       if(f == fraction)
       {
        Debug.Log("Nodeの情報を変更します");
        Produceunit();
            return;
       }
       else
       {
        DestroyUnit(f,senderID);
       }
    }

    void DestroyUnit(Fraction f, int senderID)
    {
        UpdateAmountText();
        Debug.Log("今の数字を減らします");
        currentAmount--;
        if(currentAmount<=0)
        {
            FractionHander(f,senderID);
        }
    }
    void FractionHander(Fraction f, int senderID)
    {
        Debug.Log(f);
        fraction = f;
        switch(fraction)
        {
           case Fraction.PLAYER: //相手のオブジェクトが自分のになるとき
            GetComponent<SpriteRenderer>().material = materials[1];
                StartCoroutine(Produce());
                photonView.RequestOwnership();


                break;
           
           case Fraction.ENEMY:
            GetComponent<SpriteRenderer>().material = materials[2];
                StopCoroutine(Produce());
                photonView.TransferOwnership(senderID);

           break;
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        throw new System.NotImplementedException();
    }

    void IPunInstantiateMagicCallback.OnPhotonInstantiate(PhotonMessageInfo info)
    {
        if (info.Sender.IsLocal)
        {
            Debug.Log("自身がネットワークオブジェクトを生成しました");
            GetComponent<SpriteRenderer>().material = materials[1];
            this.fraction = Fraction.PLAYER;
            /*if (!photonView.IsRoomView)
            {
                var position = new Vector3(5f, 3f);
                this.transform.position = position;
            }*/

        }
        else
        {
            Debug.Log("他プレイヤーがネットワークオブジェクトを生成しました");
            GetComponent<SpriteRenderer>().material = materials[2];
            this.fraction = Fraction.ENEMY;
        }
    }
}
