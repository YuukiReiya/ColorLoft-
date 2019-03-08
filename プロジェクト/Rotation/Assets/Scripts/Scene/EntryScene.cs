using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GamePadInput;
using System.Linq;
using MethodExpansion;

/// <summary>
/// プレイヤーのエントリーシーン
/// </summary>
public class EntryScene : IScene {

    //  static param!
    private static DataBase.TimeLimit timeLimit = DataBase.TimeLimit.TWO_MINUTE;//遊ぶゲーム時間
    public static DataBase.TimeLimit PlayTime { get { return timeLimit; } }     //遊ぶ時間

    //  const param!
    private const int TIMELIMIT_BETWEEN_VALUE = 30;//制限時間の増減量

    /// <summary>
    /// 参加データが入った構造体
    /// </summary>
    public class EntryData
    {
        public GamePad.Index index;     //コントローラー番号
        public RegistState registState; //登録状況
        public int id;                  //ID
        public bool isInput;            //スティックの連続入力防止フラグ
    }

    /// <summary>
    /// 登録状況
    /// </summary>
    public enum RegistState
    {
        Entry,  //参加
        Select, //キャラ選択
        End,    //終了
    }


    /// <summary>
    /// 定数宣言
    /// </summary>
    private const int iTIMELIMIT_HEAD = (int)DataBase.TimeLimit.ZERO_MINUTE_H;  //30秒
    private const int iTIMELIMIT_TAIL = (int)DataBase.TimeLimit.TREE_MINUTE;    //3分
    private const int SE_CHANNEL_INDEX = 5;         //SE再生用チャネルのインデックス
    private const int RESET_FRAME = 30;             //キー入力をリセットするフレーム
    private const float MAX_RETURN_VALUE = 1.0f;    //蓄積値の最高値
    private const float MIN_RETURN_VALUE = 0.0f;    //蓄積値の最低値
    private const float ACCUMULATION_FRAME = 90;    //蓄積フレーム
    private const float BGM_VOLUME = 0.2f;          //BGMの音量

    //  private param!
    EntryData[] data;   //参加者用データ
    SceneController.SCENE isTransition = SceneController.SCENE.ENTRY;
    //bool isTransition;  //シーン遷移フラグ
    float returnValue;  //B長押しの蓄積値。この値が 1.0 になったらタイトルシーンへ遷移


    // property
    bool isReady    //準備完了
    {
        get
        {
            //エントリーした人の中から準備完了しているプレイヤーの取得
            var entryPlayers = data.Where(it => it.registState != RegistState.Entry).ToArray();
            var finishPlayer = data.Where(it => it.registState == RegistState.End).ToArray();

#if UNITY_EDITOR
            //誰一人準備完了してない
            if (finishPlayer.Length == 0) { return false; }
#else
            //2人未満なら遊べない
            if (finishPlayer.Length <= 1) { return false; }
#endif

            //全員が登録を終えたら
            return entryPlayers.Length == finishPlayer.Length;
        }
    }

    /// <summary>
    /// 初期化
    /// </summary>
    public void Start()
    {
        //遷移フラグ
        //isTransition = false;
        isTransition = SceneController.SCENE.ENTRY;

        //プレイヤーの準備
        PlayerManager.Instance.Initialize();

        //最大4人までなので4つ用意しておく！
        data = new EntryData[DataBase.PLAYER_NUM];
       
        //データを初期化
        for(int i = 0; i < data.Length; ++i)
        {
            //インスタンス
            data[i]             = new EntryData();

            //初期化
            data[i].index       = (GamePad.Index)(i + 1);
            data[i].registState = RegistState.Entry;
            data[i].isInput     = false;

            //ID
            //int id_i = (int)data[i].index * DataBase.ID_INDEX;                int id_m = (int)DataBase.MODEL.UTC_S * DataBase.ID_PREFAB;  //モデル番号(初期はUTC_S)
            //コントローラー番号
            //int id_c = (int)data[i].index * DataBase.ID_COLOR - 1;      //色番号(初期はコントローラー番号に応じた色が設定)
            //int id = id_i + id_m + id_c;
            //data[i].id          = id;
        }

        //キャラクター名
        foreach(var it in data)
        {
            //Debug.Log(it.id);
            int index = DataBase.GetControllerID(it.id) - 1;
            EntryTextManager.Instance.SetCharacterName(index, PlayerManager.NOT_ENTRY);
            EntryTextManager.Instance.SetCharacterArrowActive(index, false);//矢印は消しておく
        }

        //ビューワーの初期化
        ViewerManager.Instance.Initialize();

        //制限時間のUIを設定時間に設定
        EntryTextManager.Instance.SetTimeLimit(timeLimit);

        //タイトルへ戻るUIのFillを設定
        ReturnTitleUI.Instance.SetFillAmountValue(returnValue);

        //BGMチャネルの音量をゼロにしてあげる
        SoundManager.Instance.GetChannel(SoundManager.DEFAULT_CHANNEL_INDEX).volume = 0;

        //BGM
        float fadeFrame = SceneController.Instance.FadeFrame;               //フェードさせるフレーム
        SoundManager.Instance.StartSoundFadeOut(fadeFrame, BGM_VOLUME);     //フェードアウト命令
        SoundManager.Instance.PlayOnBGM("BGM");
    }

    /// <summary>
    /// 更新
    /// </summary>
    public void Update()
    {
        //シーン遷移のフラグを判定
        if (isTransition == SceneController.SCENE.GAME)
        {
            //効果音
            SoundManager.Instance.PlayOnSE("transition", SE_CHANNEL_INDEX);

            //サウンドフェード
            int fadeFrame = SceneController.Instance.FadeFrame;
            SoundManager.Instance.StartSoundFadeIn(fadeFrame);

            StartGame();
        }
        else if(isTransition == SceneController.SCENE.TITLE)
        {
            //効果音
            SoundManager.Instance.PlayOnSE("return", SE_CHANNEL_INDEX);

            //サウンドフェード
            int fadeFrame = SceneController.Instance.FadeFrame;
            SoundManager.Instance.StartSoundFadeIn(fadeFrame);

            ReturnTitle();
            return;//以降の処理に入らずに処理を抜ける
        }


        bool before = isReady;  //1フレーム前の準備完了フラグ

        PlayingCancel();    //遊ぶのを止める更新処理
        ChangeTimeLimit();  //制限時間の変更
        Entry();            //登録受付
        EAfterInput();      //登録中処理
        EFinishInput();     //登録後処理

        bool after = isReady;   //更新された準備完了フラグ

        //参加プレイヤーが全員準備完了
        if (isReady)
        {
            //参加締め切り処理
            ClosingEntry();
        }

        //フラグの更新された瞬間
        if (before != after)
        {
            //準備完了の瞬間
            if (after)
            {
                //効果音
                SoundManager.Instance.PlayOnSE("readyGo", SE_CHANNEL_INDEX);

                EntryTextManager.Instance.StartArrivalReadyGoUI();
            }

            //準備完了の解除
            else
            {
                EntryTextManager.Instance.ResetReadyGoUI();
            }
        }


        //モデルの回転
        ViewerManager.Instance.RotateModels();

        //モデルの注視
        ViewerManager.Instance.LookAtFront();
    }

    /// <summary>
    /// 破棄
    /// </summary>
    public void Destroy()
    {

    }

    /// <summary>
    /// 参加プレイヤーのエントリー
    /// </summary>
    private void Entry()
    {

        //非参加プレイヤーの取得
        var player = data.Where(it => it.registState == RegistState.Entry).ToArray();

        //非参加プレイヤーに対して入力を受け付ける
        foreach(var it in player)
        {
            //入力
            var input = MyInputManager.GetController(it.index);

            //プレイヤー番号
            int index = (int)it.index - 1;

            //登録時処理
            if (input.START)
            {
                //テキスト変更
                EntryTextManager.Instance.StartCloseAndOpenVertical(index);
                EntryTextManager.Instance.SetSelectCharacterText(index);

                //初期化
                int id_i = (int)data[index].index * DataBase.ID_INDEX;          //コントローラー番号
                int id_m = (int)DataBase.MODEL.UTC_S * DataBase.ID_PREFAB;      //モデル番号(初期はUTC_S)
                int id_c = (int)data[index].index * DataBase.ID_COLOR - 1;      //色番号(初期はコントローラー番号に応じた色が設定)

                //ID決定
                it.id = id_i + id_m + id_c;

                //矢印
                EntryTextManager.Instance.SetCharacterArrowActive(index, true);
                EntryTextManager.Instance.StartArrowCursol(index);

                //一番最初のモデルビュー(UTC_S)
                int iModel = (int)DataBase.MODEL.UTC_S;
                ViewerManager.Instance.Create(it.index, iModel);

                //登録状況の更新
                it.registState = RegistState.Select;

                //効果音
                int channel = index + 1;//AudioSourceのインデックスを算出
                SoundManager.Instance.Play("entry", channel);
            }

        }

    }

    /// <summary>
    /// エントリー後の操作
    /// </summary>
    private void EAfterInput()
    {
        //キャラ選択中のプレイヤーを取得
        var player = data.Where(it => it.registState == RegistState.Select).ToArray();

        foreach(var it in player)
        {
            int index = (int)it.index - 1;

            //キャラ選択
            CharacterChoice(it.index, data[index]);

            //登録キャンセル
            EntryCancel(it.index, data[index]);
        }
    }

    /// <summary>
    /// キャラクター選択
    /// </summary>
    /// <param name="index">コントローラー番号</param>
    /// <param name="data">エントリーデータ</param>
    private void CharacterChoice(GamePad.Index index, EntryData data)
    {
        var input = MyInputManager.GetController(index);

        //IDからコントローラーインデックス取得
        int id_i = DataBase.GetControllerID(data.id);

        //IDから選択モデル取得
        int id_m = DataBase.GetPrefabID(data.id);

        int before = DataBase.GetPrefabID(data.id);//比較用変更前のモデル番号

        //左
        if (!data.isInput && input.LStick.x < 0)
        {
            id_m--;
            if ((DataBase.MODEL)id_m < DataBase.MODEL.UTC_S)
            {
                id_m = (int)DataBase.MODEL.YUKO_W;
            }

            //連続入力防止用フラグを立てる
            data.isInput = true;

            //キーフラグリセット用コルーチンの開始
            IEnumerator coroutine = ResetKeyFlags(data);
            CoroutineManager.Instance.StartCoroutineMethod(coroutine);

            //効果音
            int channel = (int)index;
            SoundManager.Instance.Play("charaSelect",channel);
        }
        //右
        else if (!data.isInput && input.LStick.x > 0)
        {
            id_m++;
            if ((DataBase.MODEL)id_m > DataBase.MODEL.YUKO_W)
            {
                id_m = (int)DataBase.MODEL.UTC_S;
            }

            //連続入力防止用フラグを立てる
            data.isInput = true;

            //キーフラグリセット用コルーチンの開始
            IEnumerator coroutine = ResetKeyFlags(data);
            CoroutineManager.Instance.StartCoroutineMethod(coroutine);

            //効果音
            int channel = (int)index;
            SoundManager.Instance.Play("charaSelect", channel);
        }

        int textIndex = id_i - 1;
        //キャラクター決定時処理
        if (input.A)
        {

            data.registState = RegistState.End;
            EntryTextManager.Instance.StartCloseAndOpenVertical(textIndex);
            EntryTextManager.Instance.SetReadyText(textIndex);
            EntryTextManager.Instance.SetCharacterArrowActive(textIndex, false);
            ViewerManager.Instance.StartWinAnimation((int)index - 1);
            ViewerManager.Instance.SetRotationFlags((int)index - 1, false);


            //ボイス
            PlayVoice(id_m, id_i);
        }

        //キャラ名
        EntryTextManager.Instance.SetCharacterName(textIndex, id_m);

        //IDの算出
        id_i *= DataBase.ID_INDEX;
        id_m *= DataBase.ID_PREFAB;
        int id_c = DataBase.GetColorID(data.id);

        //IDの再設定
        data.id = id_i + id_m + id_c;

        //IDの変更があった場合モデルビューに表示するモデルを切り替える
        if (DataBase.GetPrefabID(data.id) != before)
        {
           ViewerManager.Instance.Create(index, (id_m / DataBase.ID_PREFAB));
        }

    }

    /// <summary>
    /// キャラクターのボイスを再生
    /// </summary>
    /// <param name="modelNo"></param>
    void PlayVoice(int modelNo,int channel)
    {
        string key = string.Empty;

        switch (modelNo)
        {
            //こはくちゃん
            case (int)DataBase.MODEL.UTC_S:
            case (int)DataBase.MODEL.UTC_W:
                key = "KohakuVoice";
                break;

            //みさきちゃん
            case (int)DataBase.MODEL.MISAKI_S:
            case (int)DataBase.MODEL.MISAKI_W:
                key = "MisakiVoice";
                break;

            //ゆうこちゃん
            case (int)DataBase.MODEL.YUKO_S:
            case (int)DataBase.MODEL.YUKO_W:
                key = "YukoVoice";
                break;
        }

        SoundManager.Instance.Play(key, channel);
    }

    /// <summary>
    /// 登録のキャンセル処理
    /// </summary>
    /// <param name="index"></param>
    /// <param name="data"></param>
    private void EntryCancel(GamePad.Index index, EntryData data)
    {
        var input = MyInputManager.GetController(index);

        //参加登録をキャンセル
        if (input.B)
        {
            int textIndex = (int)index - 1;

            data.registState = RegistState.Entry;
            EntryTextManager.Instance.StartCloseAndOpenVertical(textIndex);
            EntryTextManager.Instance.SetWaitText(textIndex);
            ViewerManager.Instance.Destroy(index);

            EntryTextManager.Instance.SetCharacterName(textIndex, PlayerManager.NOT_ENTRY);
            EntryTextManager.Instance.SetCharacterArrowActive(textIndex, false);

            //効果音
            int chanel = (int)index;
            SoundManager.Instance.Play("entryCancel", chanel);
        }
    }

    /// <summary>
    /// 遊ぶのを止める
    /// </summary>
    private void PlayingCancel()
    {
        //非参加プレイヤーの取得
        var player = data.Where(it => it.registState == RegistState.Entry).ToArray();

        bool isInput = false;

        //Bボタンが押されているか判定
        foreach(var it in player)
        {
            var input = MyInputManager.GetController(it.index);

            if (input.B_Hold)
            {
                isInput = true;
                break;
            }
        }

        //非参加プレイヤーの誰かがBボタンを長押ししている
        if (isInput)
        {
            RiseReturnValue();
        }
        //誰も長押ししていない
        else
        {
            FallReturnValue();
        }

        //蓄積値をUIのFill値としてセット
        ReturnTitleUI.Instance.SetFillAmountValue(returnValue);
    }

    /// <summary>
    /// B長押しの蓄積値上昇
    /// </summary>
    private void RiseReturnValue()
    {
        //加算値の算出
        float value = MAX_RETURN_VALUE / ACCUMULATION_FRAME;

        //値の加算
        returnValue += value;

        //蓄積値が溜まり切ったらシーン遷移フラグを入れる
        if (returnValue >= MAX_RETURN_VALUE)
        {
            isTransition = SceneController.SCENE.TITLE;
        }
    }

    /// <summary>
    /// B長押しの蓄積値下降
    /// </summary>
    private void FallReturnValue()
    {
        //減算値の算出
        float value = MAX_RETURN_VALUE / ACCUMULATION_FRAME;

        //値の減算
        returnValue -= value;

        //値の補正
        if (returnValue < MIN_RETURN_VALUE)
        {
            returnValue = MIN_RETURN_VALUE;
        }
    }

    /// <summary>
    /// 制限時間の選択
    /// </summary>
    private void ChangeTimeLimit()
    {
        //全てのコントローラーの入力に対応
        var input = MyInputManager.AllController;

        //キャスト
        int iTimeLimit = (int)timeLimit;

        //左上トリガー
        if (input.LB)
        {
            iTimeLimit -= TIMELIMIT_BETWEEN_VALUE;
            if (iTimeLimit < iTIMELIMIT_HEAD)
            {
                iTimeLimit = iTIMELIMIT_TAIL;
            }

            //効果音
            SoundManager.Instance.PlayOnSE("timelimit", SE_CHANNEL_INDEX);
        }
        //右上トリガー
        else if (input.RB)
        {
            iTimeLimit += TIMELIMIT_BETWEEN_VALUE;
            if (iTimeLimit > iTIMELIMIT_TAIL)
            {
                iTimeLimit = iTIMELIMIT_HEAD;
            }

            //効果音
            SoundManager.Instance.PlayOnSE("timelimit", SE_CHANNEL_INDEX);
        }

        //キャスト
        timeLimit = (DataBase.TimeLimit)iTimeLimit;

        //UI更新
        EntryTextManager.Instance.SetTimeLimit(timeLimit);
    }

    /// <summary>
    /// 色選択
    /// </summary>
    /// <param name="index"></param>
    /// <param name="data"></param>
    private void ColorChoice(GamePad.Index index, EntryData data)
    {

    }

    /// <summary>
    /// スティックの連続入力防止用コルーチン
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    private IEnumerator ResetKeyFlags(EntryData data)
    {
        for (int i = 0; i < RESET_FRAME; ++i) yield return null;
        data.isInput = false;
    }

    /// <summary>
    /// エントリー終了の操作
    /// </summary>
    private void EFinishInput()
    {
        //登録し終わったプレイヤーを取得
        var player = data.Where(it => it.registState == RegistState.End).ToArray();

        //参加プレイヤー全員が準備を完了していなければ処理しない
        if (!player.All(it => it.registState == RegistState.End)) { return; }

        foreach(var it in player)
        {
            var input = MyInputManager.GetController(it.index);

            //キャラ選択に戻る
            if (input.B)
            {
                int index = (int)it.index - 1;
                it.registState = RegistState.Select;

                int modelNo = DataBase.GetPrefabID(it.id);

                EntryTextManager.Instance.StartCloseAndOpenVertical(index);
                EntryTextManager.Instance.SetSelectCharacterText(index);
                EntryTextManager.Instance.SetCharacterArrowActive(index, true);
                EntryTextManager.Instance.StartArrowCursol(index);
                ViewerManager.Instance.Create(it.index, modelNo);
                ViewerManager.Instance.SetRotationFlags(index, true);

                continue;
            }
        }
    }

    /// <summary>
    /// 参加締め切り処理
    /// </summary>
    private void ClosingEntry()
    {
        var input = MyInputManager.AllController;

        //参加締め切り
        if (input.START)
        {
            //未参加プレイヤーのIDを変更してあげる
            foreach(var it in data)
            {
                if (it.registState != RegistState.End)
                {
                    it.id = PlayerManager.NOT_ENTRY;
                }
            }

            //シーン遷移のフラグ
            isTransition = SceneController.SCENE.GAME;
        }
    }

    /// <summary>
    /// ゲームの開始
    /// </summary>
    private void StartGame()
    {
        SceneController.Instance.LoadFadeScene(SceneController.SCENE.GAME);

        //IDの設定
        //foreach文だと書き換えできないためfor文
        for (int i = 0; i < data.Length; ++i)
        {
            PlayerManager.Instance.SetID(data[i].id);
        }
    }

    /// <summary>
    /// タイトルに戻る
    /// </summary>
    private void ReturnTitle()
    {
        SceneController.Instance.LoadFadeScene(SceneController.SCENE.TITLE);
    }
}
