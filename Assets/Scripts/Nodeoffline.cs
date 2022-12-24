using System.Collections;
using UnityEngine;
using ExitGames.Client.Photon;
using TMPro;
using System.Collections.Generic;

public class Nodeoffline : MonoBehaviour
{
    int maxAmount;
    int startAmount = 10;
    int currentAmount;
    public Material[] materials;
    public Material[] fieldmaterials;
    //�ǉ�
    //public Fieldcolor[] fieldcolors;
    //public List<Fieldcolor> fields = new List<Fieldcolor>();
    //�ǉ��@�t�B�[���h�I�u�W�F�N�g�̃}�e���A���ύX
    //public GameObject fieldObject;



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
    private Vector3 FieldPos;
    private Vector3 Playerpos;
    private Vector3 Enemypos;


    void Start()
    {
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

        if (this.fraction == Fraction.PLAYER)
        {
            StartCoroutine(Produce());
            UpdateAmountText();
        }

    }

    void Produceunit()
    {
        //Debug.Log("���𑝂₵�܂�");
        currentAmount++;
        if (currentAmount > maxAmount)
        {
            currentAmount = maxAmount;
        }

        UpdateAmountText();

    }
    IEnumerator Produce()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            Produceunit();
        }
    }

    void UpdateAmountText()
    {
        // Debug.Log("�ʂ̏����X�V���܂��B");
        amountText.text = currentAmount + "|" + maxAmount;
    }

    IEnumerator SendALLunits(string _tag)
    {
        Node goalNode = GameObject.Find(_tag).GetComponent<Node>();

        int unitsToSend = currentAmount;
        while (currentAmount > 0)
        {
            int unitsPerSend = currentAmount <= 5 ? currentAmount : 5;

            AllUnit(goalNode, unitsPerSend);
            currentAmount -= unitsPerSend;
            //currentAmount -= unitsPerSend;
            yield return new WaitForSeconds(0.5f);
            UpdateAmountText();


        }


    }

    public void AllUnit(Node goalNode, int numberOfUnits)
    {
        for (int i = 0; i < numberOfUnits; i++)
        {
            //Debug.Log(i);
            Pos = this.transform.position;
            GameObject newUnit = Instantiate(unitPrefab, Pos, Quaternion.identity);
            Unit unit = newUnit.GetComponent<Unit>();
            float intervalDeg = 10f;
            float minDeg = -(numberOfUnits - 1) * intervalDeg / 2;
            float degree = (minDeg + intervalDeg * (float)i) / 180f * Mathf.PI;
            newUnit.GetComponent<Unit>().SetUnit(fraction, goalNode, GetComponent<SpriteRenderer>().material, transform.parent.GetComponent<Fieldcolor>(), degree);

        }

    }
    public void SendUnits(string tag)
    {
        StartCoroutine(SendALLunits(tag));
    }



    public void HandleIncomingUnit(Fraction f, int senderID)
    {
        //Debug.Log(f);
        if (f == fraction)
        {
            //Debug.Log("Node�̏���ύX���܂�");
            Produceunit();
            return;
        }
        else
        {
            DestroyUnit(f, senderID);
        }
    }

    void DestroyUnit(Fraction f, int senderID)
    {
        UpdateAmountText();
        //Debug.Log("���̐��������炵�܂�");
        currentAmount--;
        if (currentAmount <= 0)
        {
            FractionHander(f, senderID);
        }
    }
    void FractionHander(Fraction f, int senderID)
    {
        fraction = f;
        switch (fraction)
        {
            case Fraction.PLAYER: //����̃I�u�W�F�N�g�������̂ɂȂ�Ƃ�
                GetComponent<SpriteRenderer>().material = materials[1];
                //�ǉ�
                //transform.GetChild(1).GetComponent<Renderer>().material = fieldmaterials[1];
                PlayerField();

                StartCoroutine(Produce());

                break;

            case Fraction.ENEMY:
                GetComponent<SpriteRenderer>().material = materials[2];
                //�ǉ�
                //transform.GetChild(1).GetComponent<Renderer>().material = fieldmaterials[2];
                EnemyField();

                StopCoroutine(Produce());

                break;
        }
    }
    public void PlayerField()
    {
        transform.parent.GetComponent<Renderer>().material = fieldmaterials[1];

    }
    public void EnemyField()
    {
        transform.parent.GetComponent<Renderer>().material = fieldmaterials[2];
    }


}
