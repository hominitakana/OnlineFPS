using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun; //ライブラリの追加

public class PhotonManager : MonoBehaviourPunCallbacks
{   

    public static PhotonManager instance;
    public GameObject loagingPanel;

    public Text LoadingText;

    public GameObject buttons;

    private void Awake() {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        CloseMenuUI();
        loagingPanel.SetActive(true);
        LoadingText.text = "ネットワークに接続中...";

        //ネットワークに接続してなかったら接続する。
        if(!PhotonNetwork.IsConnected){
            PhotonNetwork.ConnectUsingSettings();
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CloseMenuUI(){
        loagingPanel.SetActive(false);
        buttons.SetActive(false);
    }

    public void LobbyMenuDisplay(){
        CloseMenuUI();
        buttons.SetActive(true);

    }

    // マスターサーバーに接続されたときに呼ばれる関数
    public override void OnConnectedToMaster()
    {
        //ロビーに接続
        PhotonNetwork.JoinLobby();

        //テキスト更新
        LoadingText.text = "ロビーへ参加中...";

    }

    public override void OnJoinedLobby()
    {
        LobbyMenuDisplay();
    }


}
