using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Sockets;
using WebSocketSharp;
using System.Configuration;

namespace kiosko
{
    public partial class Form1 : Form
    {
        string CIS_ID;
        string MOD_ID;
        string TURNO_NUM;
        string PERSON_ID;
        WebSocket ws;
        TcpClient client;
        string server_ws = "ws://127.0.0.1:3000/socket.io/?EIO=2&transport=websocket";

        public Form1()
        {
            InitializeComponent();
            client = new TcpClient();
            CIS_ID = ConfigurationManager.AppSettings["CIS_ID"];
            MOD_ID = ConfigurationManager.AppSettings["MOD_ID"];
            PERSON_ID = ConfigurationManager.AppSettings["PERSON_ID"];
        }

        private void nxt_btn_Click(object sender, EventArgs e)
        {
            /*------------------------------------------------------------------------------------------------------------------------------------*/
            /* ###  EL SCRIPT REQUIRE TOMAR EL TURNO CON MENOR NUM_TURNO DE UNA CONSULTA OREDENADA POR FECHA ASC QUE CUMPLAN CON ESTAS CONDICIONES
            /* ###  TENER STATUS 1 , PERTENECER AL MISMO CIS ID Y AL MISMO GRUPO DE MODULOS 
            /* ### ASIGNAR A ESTE TURNO EN EL CAMPO MODULO Y PERSONA LOS DATOS DE ESTE MODULO, ES NECESARIO TOMAR ESTA INFO DEL ARCHIVO DE CONFIGURACION 
            /*------------------------------------------------------------------------------------------------------------------------------------*/
            if(textBox1.Text != "") {
                TURNO_NUM = textBox1.Text;
            }
            if (textBox2.Text != "")
            {
                MOD_ID = textBox2.Text;
            }

            ws = new WebSocket(server_ws);
            ws.Connect();
            ws.OnError += (senderx, ex) =>
            {
                Console.Write("Error!:" + ex.Message);
            };
            ws.OnMessage += (sender1, e1) =>
            {
                int n;
                bool isNumeric = int.TryParse(e1.Data, out n);
                if (isNumeric)
                {
                    Console.WriteLine("code: " + e1.Data);
                }
                else
                {
                    if (e1.Data.Contains("message") && !e1.Data.Contains(":"))
                    {
                        Console.WriteLine("message: " + e1.Data);
                        string json = "42[\"message\",\"" + MOD_ID + ":" + TURNO_NUM + ":" + PERSON_ID + "  \"]";
                        ws.Send(json);
                    }
                    else
                    {
                        Console.WriteLine("handshake: " + e1.Data);
                    }
                }
            };
        }
    }
}
