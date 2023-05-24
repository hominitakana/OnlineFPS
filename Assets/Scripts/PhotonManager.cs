using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

//photonViewと、PUNが呼び出すことのできるすべてのコールバック/イベントを提供します。使用したいイベント/メソッドをオーバーライドしてください。
public class PhotonManager : MonoBehaviourPunCallbacks
{
    //よく見るドキュメントページ
    //https://doc-api.photonengine.com/ja-jp/pun/current/class_photon_1_1_pun_1_1_photon_network.html
    //https://doc-api.photonengine.com/ja-jp/pun/current/class_photon_1_1_pun_1_1_mono_behaviour_pun_callbacks.html
    //https://doc-api.photonengine.com/ja-jp/pun/current/namespace_photon_1_1_realtime.html

    public static PhotonManager instance;//static
    public GameObject loadingPanel;//ロードパネル
    public Text loadingText;//ロードテキスト
    public GameObject buttons;//ボタン


    public GameObject createRoomPanel;//ルーム作成パネル
    public Text enterRoomName;//入力されたルーム名テキスト


    public GameObject roomPanel;//ルームパネル
    public Text roomName;//ルーム名テキスト


    private void Awake()
    {
        instance = this;//格納
    }


    void Start()
    {
        //メニューをすべて閉じる
        CloseMenuUI();

        //ロードパネルを表示してテキスト更新
        loadingPanel.SetActive(true);
        loadingText.text = "ネットワークに接続中...";

        //ネットワークに接続しているのか確認
        if (!PhotonNetwork.IsConnected)
        {
            //最初に設定したPhotonServerSettingsファイルの設定に従ってPhotonに接続
            PhotonNetwork.ConnectUsingSettings();
        }
    }


    /// <summary>
    /// 一旦すべてを非表示にする
    /// </summary>
    void CloseMenuUI()//なぜ作るのか：UI切り替えが非常に楽だから
    {
        loadingPanel.SetActive(false);//ロードパネル非表示

        buttons.SetActive(false);//ボタン非表示

        createRoomPanel.SetActive(false);//ルーム作成パネル
        
        roomPanel.SetActive(false);//ルームパネル
    }



    //継承元のメソッドでは「virtual」のキーワード
    //継承先では「override」のキーワード
    /// <summary>
    /// クライアントがMaster Serverに接続されていて、マッチメイキングやその他のタスクを行う準備が整ったときに呼び出されます
    /// </summary>
    public override void OnConnectedToMaster()//
    {

        PhotonNetwork.JoinLobby();//マスターサーバー上で、デフォルトロビーに入ります

        loadingText.text = "ロビーへの参加...";//テキスト更新

    }


    /// <summary>
    /// マスターサーバーのロビーに入るときに呼び出されます。
    /// </summary>
    public override void OnJoinedLobby()//
    {

        LobbyMenuDisplay();//

    }


    //ロビーメニュー表示(エラーパネル閉じる時もこれ)
    public void LobbyMenuDisplay()
    {
        CloseMenuUI();
        buttons.SetActive(true);
    }

    //タイトルの部屋作成ボタン押下時に呼ぶ：UIから呼び出す
    public void OpenCreateRoomPanel()
    {
        CloseMenuUI();
        createRoomPanel.SetActive(true);
    }

    //部屋作成ボタン押下時に呼ぶ：UIから呼び出す
    public void CreateRoomButton()
    {
        //インプットフィールドのテキストに何か入力されていた場合
        if (!string.IsNullOrEmpty(enterRoomName.text))
        {
            //ルームのオプションをインスタンス化して変数に入れる 
            RoomOptions options = new RoomOptions();
            options.MaxPlayers = 6;// プレイヤーの最大参加人数の設定（無料版は20まで。1秒間にやり取りできるメッセージ数に限りがあるので10以上は難易度上がる）

            //ルームを作る(ルーム名：部屋の設定)
            PhotonNetwork.CreateRoom(enterRoomName.text, options);

            Debug.Log(enterRoomName.text);


            CloseMenuUI();//メニュー閉じる
            loadingText.text = "ルーム作成中...";
            loadingPanel.SetActive(true);
        }
    }


    //ルームに参加したら呼ばれる関数
    public override void OnJoinedRoom()
    {
        CloseMenuUI();//一旦すべてを閉じる
        roomPanel.SetActive(true);//ルームパネル表示

        roomName.text = PhotonNetwork.CurrentRoom.Name;//現在いるルームを取得し、テキストにルーム名を反映
    }
}