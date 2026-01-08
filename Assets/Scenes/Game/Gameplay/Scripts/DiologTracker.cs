using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using static PoliConnect;

public class DiologTracker : MonoBehaviour
{
    [SerializeField] private PoliConnect.client client;
    [SerializeField] private TMP_InputField getMsg;
    [SerializeField] private ScrolPanControl scrolPan;

    [SerializeField] private GameObject EndWinPref;
    [SerializeField] private GameObject parentOfEndWin;

    private Dictionary<string, string> story = new(); // слева - юзер, с права - нейро
    private PoliConnect _poli;
    private bool update = false;
    private KeyValuePair<string, string> lastPair;
    private int stts = 0;

    void Start()
    {
        _poli = GetComponent<PoliConnect>();
        getMsg.onSubmit.AddListener(SendReq);
    }

    void Update()
    {
        if (update)
        {
            if (story.Count == 0) return;
            var currentLast = story.Last();

            if (!lastPair.Equals(currentLast))
            {
                scrolPan.Write(currentLast.Value);
                lastPair = currentLast;
                update = false;

                if (stts != 0)
                {
                    EndLevel();
                }
            }
        }
    }

    public void EndLevel()
    {
        var w = Instantiate(EndWinPref, parentOfEndWin.transform);
        w.GetComponent<FibalWindowInit>().Init(stts == 1);
    }

    public void SendReq(string text)
    {
        if (stts != 0) return;

        update = true;
        getMsg.text = "";

        Request req = new();
        req.clientType = client;
        req.request = text;
        string stry = "";
        foreach (var a in story)
        {
            stry += "\n\n" + a.Key + "\n\n";
            stry += a.Value;
        }

        req.context = stry;

        scrolPan.Write(text);

        string description;
        try
        {
            description = PlayerPrefs.GetString("Description");
            req.pers = description;
        }
        catch
        {
            Debug.Log("no decription");
        }

        var b = Task.Run(() => CompliteReq(req));
    }



    public void SendReq()
    {
        if (stts != 0) return;

        update = true;
        string text = getMsg.text;
        getMsg.text = "";

        Request req = new();
        req.clientType = client;
        req.request = text;
        string stry = "";
        foreach (var a in story)
        {
            stry += "\n\n" + a.Key + "\n\n";
            stry += a.Value;
        }
         
        req.context = stry;

        scrolPan.Write(text);

        var b = Task.Run(() => CompliteReq(req));
    }

    private async Task CompliteReq(Request a)
    {
        Answer ans = await _poli.GetAnswer(a);
        string[] answer;
        if (ans.ansType == Answer.answerType.Error)
        {
            answer = new string[] { "Err: broken msg" };
            stts = 0;
        }
        else
        {
            answer = ans.text.Split("~");
            stts = int.Parse(answer[1]);
        }

        story.Clear();
        story.Add(ans.context, "");
        story.Add(a.request, answer[0]);


        Debug.Log(ans.text + '\n' + ans.ansType + '\n' + ans.context);
    }
}