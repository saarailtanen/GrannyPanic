using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class EspOhjainVasen : MonoBehaviour
{
    [SerializeField]
    string serverilleMenevaViesti1 = "heipodei";

    //int new_rpm1;


    public ESPDatat espData1 = new ESPDatat();

    CancellationTokenSource yhteysPoletti1;

    void Start()
    {
        TaskinKaynnistys();
    }

    void TaskinKaynnistys()
    {
        Debug.Log("taskin käynnistys on käynnistetty");
        var sokettiYhteys1 = Task.Run(() => ESPYhteys());
    }

    async Task ESPYhteys()
    {
        Debug.Log("ESPyhteys käynnistetty");

        using (ClientWebSocket soketti1 = new ClientWebSocket())
        {
            //muistakaa vaihtaa ip osoite wifin mukaan
            Uri osoite1 = new Uri("");
            yhteysPoletti1 = new CancellationTokenSource();
            //yhteysPoletti.CancelAfter(20000);
            await soketti1.ConnectAsync(osoite1, yhteysPoletti1.Token);
            Debug.Log("Soketti yhteys ESP32 luotu");

            while (soketti1.State == WebSocketState.Open && yhteysPoletti1.IsCancellationRequested == false)
            {
                //viesti arduinon suuntaan
                if (serverilleMenevaViesti1.Length > 0)
                {
                    ArraySegment<byte> viesti1 = new ArraySegment<byte>(Encoding.UTF8.GetBytes(serverilleMenevaViesti1));
                    await soketti1.SendAsync(viesti1, WebSocketMessageType.Text, true, yhteysPoletti1.Token);
                    serverilleMenevaViesti1 = "";
                }

                //viestin vastaanotto
                var vastaanOtto1 = new byte[250];
                int offset1 = 0;
                int datayhdessapaketissa1 = 10;
                while (yhteysPoletti1.IsCancellationRequested == false)
                {
                    ArraySegment<byte> vastaanotettavaviesti1 = new ArraySegment<byte>(
                        vastaanOtto1, offset1, datayhdessapaketissa1);
                    WebSocketReceiveResult tulokset1 = await soketti1.ReceiveAsync(vastaanotettavaviesti1, yhteysPoletti1.Token);
                    offset1 = offset1 + tulokset1.Count;
                    if (tulokset1.EndOfMessage)
                    {
                        break;
                    }
                }
                string vastaanottettuviesti1 = Encoding.UTF8.GetString(vastaanOtto1, 0, offset1);
                //Debug.Log("Vastaanotettiin viesti: " + vastaanottettuviesti1);
                espData1 = JsonUtility.FromJson<ESPDatat>(vastaanottettuviesti1);
            }
        }
        yhteysPoletti1.Dispose();

    }

    void OnDisable()
    {
        Debug.Log("OnDisable; postetaan thread");
        if (yhteysPoletti1 != null) yhteysPoletti1.Cancel();
    }
}

