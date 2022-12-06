
using UnityEngine;
using System;
using Photon.Pun;
using Photon.Realtime;


public class Unit : MonoBehaviour
{
    public Fraction fraction;
    Node goal;

    public float degree;
    public bool isLastUnit; // 5�Ԗڂ̋ʂȂ̂�?
    float speed = 0.5f;
    private float interpolateAmount;
    Vector3 startPos;
    public int senderID; //�U�����ID,�I�u�W�F�N�g�̎󂯓n���ɂ���
    //float Vecx;






    public void SetUnit(Fraction _fraction, Node goalNode, Material mat,float _degree)
    {
        fraction = _fraction;
        goal = goalNode;
        GetComponent<SpriteRenderer>().material = mat;
        startPos = transform.position;
        degree = _degree;
    }

    void Update()
    {
        //Vecx = UnityEngine.Random.Range(-0.5f,-0.2f);
        //startPos.x = startPos.x + Vecx;
        double x = 1 - (Math.Pow((goal.transform.position.x - transform.position.x), 2) + Math.Pow((goal.transform.position.y - transform.position.y), 2)) / (Math.Pow((goal.transform.position.x - startPos.x), 2) + Math.Pow((goal.transform.position.y - startPos.y), 2));


        speed = easeInExpo((float)x);
        if (speed < 0.1f)
        {
            speed = 0.1f;
        }
        else if (speed > 5f)
        {
            speed = 5f;
        }
        interpolateAmount = (interpolateAmount + Time.deltaTime * speed) % 1f;

        transform.position = QuadraticLeap(startPos, goal.transform.position, interpolateAmount);

        //photonView.RPC("QuadraticLeap", RpcTarget.All, startPos, goal.transform.position, interpolateAmount); 
            
        
        
    }
    

        /*private Vector3 QuadraticLeapBefore(Vector3 a, Vector3 c, float t)
        {
        Vector3 b = new Vector3(c.x - 2, c.y - 1, 0);
        Vector3 ab = Vector3.Lerp(a, b, t);
        Vector3 bc = Vector3.Lerp(b, c, t);

        return Vector3.Lerp(ab, bc, interpolateAmount);}*/
    
    private Vector3 QuadraticLeap(Vector3 a, Vector3 c, float t)
    {
        Vector2 dt = c - a;
        float alpha = Mathf.Atan2(dt.y, dt.x);
        float length = dt.magnitude / 2;
        float bx = length * Mathf.Cos(alpha + degree) + a.x;
        float by = length * Mathf.Sin(alpha + degree) + a.y;

        Vector3 b = new Vector3(bx, by, 0);
        Vector3 ab = Vector3.Lerp(a, b, t);
        Vector3 bc = Vector3.Lerp(b, c, t);

        return Vector3.Lerp(ab, bc, interpolateAmount);
        
    }

    void OnTriggerEnter2D(Collider2D col)
    {
       Node n = col.GetComponent<Node>();
       if(n == goal)
       {
        Debug.Log("�G�ɂ�������");
        n.HandleIncomingUnit(fraction,senderID);
        Destroy(this.gameObject);
       } 
    }
    float easeInExpo(float x)
    {
        return x == 0f ? 0f : (float)(Math.Pow(2f, 7 * x - 5));
    }
    

}