using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using static PoliConnect;

public class DiologTracker : MonoBehaviour
{
    [SerializeField] private PoliConnect.client client;
    [SerializeField] private TMP_InputField getMsg;
    [SerializeField] private ScrolPanControl scrolPan; 

    private Dictionary<string, string> story = new(); //  слева - юзер, с права - нейро
    private PoliConnect _poli;

    void Start()
    {
        _poli = GetComponent<PoliConnect>();
    }

    public void SendReq()
    {
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

        _ = Task.Run(() => CompliteReq(req));

        scrolPan.Write(text, "You");
    }

    private async Task CompliteReq(Request a)
    {
        Debug.Log("думаю");
        Answer ans = await _poli.GetAnswer(a);
        if (ans.ansType == Answer.answerType.Error)
        {
            Debug.Log("поломалось");
            // свапнуть на функцию ошибки
            return;
        }

        var answer = ans.text.Split("[~|~]");

        Debug.Log(answer[0]);

        scrolPan.Write(answer[0], client.ToString());

        story.Add(a.request, answer[0]);
    }
}
