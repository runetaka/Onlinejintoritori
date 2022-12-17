using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class UiManager : MonoBehaviourPunCallbacks
{
    public TextMeshProUGUI timeLabel;
    public GameObject endPanel;
    public float resttime;

    private bool finished;

    // Update is called once per frame
    void Update()
    {

            TimeState();
        
    }

    public void TimeState()
    {
        // まだルームに参加していない場合は更新しない
        if (!PhotonNetwork.InRoom) { return; }
        // まだゲームの開始時刻が設定されていない場合は更新しない
        if (!PhotonNetwork.CurrentRoom.TryGetStartTime(out int timestamp)) { return; }

        // ゲームの経過時間を求めて、小数第一位まで表示する
        float elapsedTime = Mathf.Max(0f, unchecked(PhotonNetwork.ServerTimestamp - timestamp) / 1000f);
        float timeover = resttime - elapsedTime;
        

        if (timeover <= 0)
        {
            if ( finished == false) {
                finished = true;
                Time.timeScale = 0;
                timeover = 0;
                timeLabel.text = timeover.ToString("f0");
                winner();
            }
            return;
            //OpenEndPanel();
        }
        else
        {
            timeLabel.text = timeover.ToString("f0");
        }

    }

    public void OpenEndPanel()
    {
        endPanel.SetActive(true);
    }

    public void winner()
    {
        Debug.Log("winner");
        int playerlengh = 0;
        int enemylengh = 0;

        var nodes = GameObject.FindGameObjectsWithTag("Node");
        Debug.Log(nodes.Length);
        foreach (var node in nodes)
        {
            if (node.GetComponent<Node>().fraction == Fraction.PLAYER)
            {
                playerlengh++;
            }
            else if (node.GetComponent<Node>().fraction == Fraction.ENEMY)
            {
                enemylengh++;
            }
            else
            {
                continue;
            }
        }

        if (playerlengh>enemylengh)
        {
            Debug.Log("勝利");   
        }
        else if(playerlengh < enemylengh)
        {
            Debug.Log("大敗");
        }
        else
        {
            Debug.Log("たわけ");
        }
    }
}
