using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
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
                    var w = Instantiate(EndWinPref, parentOfEndWin.transform);
                    w.GetComponent<FibalWindowInit>().Init(stts == 1);
                }
            }
        }
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
        string stry = "начало диолога:\n\n";
        foreach (var a in story)
        {
            stry += "пользователь: " + a.Key + "\n\n";
            stry += "твой предыдущий ответ: " + a.Value + "\n\n";
        }
         
        req.context = stry;

        scrolPan.Write(text);

        var b = Task.Run(() => CompliteReq(req));
    }

    private async Task CompliteReq(Request a)
    {
        Debug.Log("думаю");
        Answer ans = await _poli.GetAnswer(a);
        Debug.Log(ans.text);
        Debug.Log(ans.ansType);
        string[] answer;
        if (ans.ansType == Answer.answerType.Error)
        {
            answer = new string[] { "Err: сообщение слишком некорректно" };
            stts = 0;
        }
        else
        {
            answer = ans.text.Split("[~|~]");
            stts = int.Parse(answer[1]);
        }
        story.Add(a.request, answer[0]);
    }
}