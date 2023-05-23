using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun; //ライブラリの追加
using Photon.Realtime;

public class PhotonManager : MonoBehaviourPunCallbacks
{   

    public static PhotonManager instance;
    public GameObject loagingPanel;

    public Text LoadingText;

    public GameObject buttons;

    public GameObject creatRoomPanel;
    public Text enterRoomName;

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
        creatRoomPanel.SetActive(false);
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

    // ルームを作るボタン用のUI
    public void OpenCreatRoomPanel(){
        CloseMenuUI();
        creatRoomPanel.SetActive(true);
    }

    // ルーム作成ボタン用の関数
    public void CreatRoomButton(){
        //入力されていない場合True
        if (!string.IsNullOrEmpty(enterRoomName.text))
        {
            RoomOptions options = new RoomOptions();
            options.MaxPlayers = 8;

            // ルーム作成
            PhotonNetwork.CreateRoom(enterRoomName.text,options);
            CloseMenuUI();

            LoadingText.text = "ルーム作成中...";
            loagingPanel.SetActive(true);
        }
    }

}
