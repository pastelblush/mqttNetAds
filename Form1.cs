using System;
using System.Text;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.IO;
using TwinCAT.Ads;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using Newtonsoft.Json;

namespace MQTT.Sample

{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
    /// 
   
	public class Form1 : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.TextBox tbInt;
		private System.Windows.Forms.TextBox tbDint;
		private System.Windows.Forms.TextBox tbSint;
		private System.Windows.Forms.TextBox tbLreal;
		private System.Windows.Forms.TextBox tbReal;
		private System.Windows.Forms.TextBox tbString;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		private TcAdsClient tcClient;		
		private int[] hConnect;
		private AdsStream dataStream;
		private System.Windows.Forms.Label label11;
		private System.Windows.Forms.TextBox tbBool;
		private BinaryReader binRead;

        private const string IotEndpoint = "localhost";
        private const int BrokerPort = 1883;
        private const string Topic = "presence";
        private MqttClient client;
        private jsonmsq tags;
        private int iHandle;
        private bool iValue;


        //private jsontriggers triggers;


		public Form1()
		{
			InitializeComponent();
            tags = new jsonmsq();
            //triggers = new jsontriggers();

		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.tbInt = new System.Windows.Forms.TextBox();
			this.tbDint = new System.Windows.Forms.TextBox();
			this.tbSint = new System.Windows.Forms.TextBox();
			this.tbLreal = new System.Windows.Forms.TextBox();
			this.tbReal = new System.Windows.Forms.TextBox();
			this.tbString = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.label8 = new System.Windows.Forms.Label();
			this.label9 = new System.Windows.Forms.Label();
			this.label10 = new System.Windows.Forms.Label();
			this.label11 = new System.Windows.Forms.Label();
			this.tbBool = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// tbInt
			// 
			this.tbInt.Location = new System.Drawing.Point(104, 48);
			this.tbInt.Name = "tbInt";
			this.tbInt.Size = new System.Drawing.Size(240, 20);
			this.tbInt.TabIndex = 0;
			this.tbInt.Text = "";
			// 
			// tbDint
			// 
			this.tbDint.Location = new System.Drawing.Point(104, 80);
			this.tbDint.Name = "tbDint";
			this.tbDint.Size = new System.Drawing.Size(240, 20);
			this.tbDint.TabIndex = 1;
			this.tbDint.Text = "";
			// 
			// tbSint
			// 
			this.tbSint.Location = new System.Drawing.Point(104, 112);
			this.tbSint.Name = "tbSint";
			this.tbSint.Size = new System.Drawing.Size(240, 20);
			this.tbSint.TabIndex = 2;
			this.tbSint.Text = "";
			// 
			// tbLreal
			// 
			this.tbLreal.Location = new System.Drawing.Point(104, 144);
			this.tbLreal.Name = "tbLreal";
			this.tbLreal.Size = new System.Drawing.Size(240, 20);
			this.tbLreal.TabIndex = 3;
			this.tbLreal.Text = "";
			// 
			// tbReal
			// 
			this.tbReal.Location = new System.Drawing.Point(104, 176);
			this.tbReal.Name = "tbReal";
			this.tbReal.Size = new System.Drawing.Size(240, 20);
			this.tbReal.TabIndex = 4;
			this.tbReal.Text = "";
			// 
			// tbString
			// 
			this.tbString.Location = new System.Drawing.Point(104, 208);
			this.tbString.Name = "tbString";
			this.tbString.Size = new System.Drawing.Size(240, 20);
			this.tbString.TabIndex = 5;
			this.tbString.Text = "";
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(8, 48);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(88, 23);
			this.label1.TabIndex = 6;
			this.label1.Text = "MAIN.intVal :";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(8, 80);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(88, 23);
			this.label2.TabIndex = 7;
			this.label2.Text = "MAIN.dintVal :";
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(8, 112);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(88, 23);
			this.label3.TabIndex = 8;
			this.label3.Text = "MAIN.sintVal :";
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(8, 144);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(88, 23);
			this.label4.TabIndex = 9;
			this.label4.Text = "MAIN.lrealVal :";
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(8, 176);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(88, 23);
			this.label5.TabIndex = 10;
			this.label5.Text = "MAIN.realVal :";
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(8, 208);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(88, 23);
			this.label6.TabIndex = 11;
			this.label6.Text = "MAIN.stringVal :";
			// 
			// label7
			// 
			this.label7.Location = new System.Drawing.Point(8, 144);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(72, 23);
			this.label7.TabIndex = 9;
			this.label7.Text = "label4";
			// 
			// label8
			// 
			this.label8.Location = new System.Drawing.Point(8, 112);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(72, 23);
			this.label8.TabIndex = 8;
			this.label8.Text = "label3";
			// 
			// label9
			// 
			this.label9.Location = new System.Drawing.Point(8, 48);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(72, 23);
			this.label9.TabIndex = 6;
			this.label9.Text = "label1";
			// 
			// label10
			// 
			this.label10.Location = new System.Drawing.Point(8, 80);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(72, 23);
			this.label10.TabIndex = 7;
			this.label10.Text = "label2";
			// 
			// label11
			// 
			this.label11.Location = new System.Drawing.Point(8, 16);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(88, 23);
			this.label11.TabIndex = 13;
			this.label11.Text = "MAIN.boolVal :";
			// 
			// tbBool
			// 
			this.tbBool.Location = new System.Drawing.Point(104, 16);
			this.tbBool.Name = "tbBool";
			this.tbBool.Size = new System.Drawing.Size(240, 20);
			this.tbBool.TabIndex = 12;
			this.tbBool.Text = "";
			// 
			// Form1
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(352, 237);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.label11,
																		  this.tbBool,
																		  this.label6,
																		  this.label5,
																		  this.label4,
																		  this.label3,
																		  this.label2,
																		  this.label1,
																		  this.tbString,
																		  this.tbReal,
																		  this.tbLreal,
																		  this.tbSint,
																		  this.tbDint,
																		  this.tbInt,
																		  this.label7,
																		  this.label8,
																		  this.label9,
																		  this.label10});
			this.Name = "Form1";
			this.Text = "Form1";
			this.Closing += new System.ComponentModel.CancelEventHandler(this.Form1_Closing);
			this.Load += new System.EventHandler(this.Form1_Load);
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new Form1());
		}

		private void Form1_Load(object sender, System.EventArgs e)
		{
            client = new MqttClient(IotEndpoint);
            client.Connect("clientid1");

			dataStream = new AdsStream(31);
			//Encoding wird auf ASCII gesetzt, um Strings lesen zu können
			binRead = new BinaryReader(dataStream, System.Text.Encoding.ASCII);
			// Instanz der Klasse TcAdsClient erzeugen
			tcClient = new TcAdsClient();			
			// Verbindung mit Port 801 auf dem lokalen Computer herstellen
			tcClient.Connect(801);

			hConnect = new int[7];

			try
			{
				hConnect[0] = tcClient.AddDeviceNotification("MAIN.boolVal",dataStream,0,1,
					AdsTransMode.OnChange,100,0,tbBool);
				hConnect[1] = tcClient.AddDeviceNotification("MAIN.intVal",dataStream,1,2,
					AdsTransMode.OnChange,100,0,tbInt);
				hConnect[2] = tcClient.AddDeviceNotification("MAIN.dintVal",dataStream,3,4,
					AdsTransMode.OnChange,100,0,tbDint);
				hConnect[3] = tcClient.AddDeviceNotification("MAIN.sintVal",dataStream,7,1,
					AdsTransMode.OnChange,100,0,tbSint);
				hConnect[4] = tcClient.AddDeviceNotification("MAIN.lrealVal",dataStream,8,8,
					AdsTransMode.OnChange,100,0,tbLreal);
				hConnect[5] = tcClient.AddDeviceNotification("MAIN.realVal",dataStream,16,4,
					AdsTransMode.OnChange,100,0,tbReal);
				hConnect[6] = tcClient.AddDeviceNotification("MAIN.stringVal",dataStream,20,11,
					AdsTransMode.OnChange,100,0,tbString);

				tcClient.AdsNotification += new AdsNotificationEventHandler(OnNotification);
			}
			catch(Exception err)
			{
				MessageBox.Show(err.Message);
			}

            iHandle = tcClient.CreateVariableHandle("MAIN.boolVal");


            //event handler for inbound messages
            client.MqttMsgPublishReceived += ClientMqttMsgPublishReceived;

            client.Subscribe(new[] { "bVal" }, new[] { MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE }); 
		}

        public void ClientMqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            //Console.WriteLine("We received a message...");
            //Console.WriteLine(Encoding.UTF8.GetChars(e.Message));

            var chars = Encoding.UTF8.GetString(e.Message);
            //
            jsontag tag = JsonConvert.DeserializeObject<jsontag>(chars);
            //MessageBox.Show(triggers.boolVal.ToString());
            tcClient.WriteAny(iHandle, tag.val);
            //client.Publish(Topic, Encoding.UTF8.GetBytes(json));
        }

		private void OnNotification(object sender, AdsNotificationEventArgs e)
		{
			DateTime time = DateTime.FromFileTime(e.TimeStamp);
			e.DataStream.Position = e.Offset;					
			string strValue = "";
            string tag = "";

            

			
			if( e.NotificationHandle == hConnect[0])
            {
                tags.boolVal = binRead.ReadBoolean().ToString();
                tag = "boolVal";
            }
			else if( e.NotificationHandle == hConnect[1] )
            {
                tags.intVal = binRead.ReadInt16().ToString();
                tag = "intVal";	
            }
            else if (e.NotificationHandle == hConnect[2])
            {
                tags.dintVal = binRead.ReadInt32().ToString();
                tag = "dintVal";
            }
            else if (e.NotificationHandle == hConnect[3])
            {
                tags.sintVal = binRead.ReadSByte().ToString();
                tag = "sintVal";
            }
            else if (e.NotificationHandle == hConnect[4])
            {
                tags.lrealVal = binRead.ReadDouble().ToString();
                tag = "lrealVal";
            }
            else if (e.NotificationHandle == hConnect[5])
            {
                tags.realVal = binRead.ReadSingle().ToString();
                tag = "realtVal";
            }
            else if (e.NotificationHandle == hConnect[6])
            {
                tags.stringVal = new String(binRead.ReadChars(11)).TrimEnd();
                //strValue = strValue.TrimEnd();
                tag = "stringVal";
            }


            //tags.tag = tag;
            //tags.value = strValue;

            string json = JsonConvert.SerializeObject(tags);
					
			((TextBox)e.UserData).Text = String.Format("DateTime: {0},{1}ms; {2}",time,time.Millisecond,strValue);

            client.Publish(Topic, Encoding.UTF8.GetBytes(json));
            
		}

		private void Form1_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			try
			{
				tcClient.Dispose();
			}
			catch(Exception err)
			{
				MessageBox.Show(err.Message);
			}			
		}	

	}

    public class jsonmsq
    {
        public string boolVal;
        public string intVal;
        public string dintVal;
        public string sintVal;
        public string lrealVal;
        public string realVal;
        public string stringVal;
    }

    public class jsontag
    {
        public bool val;
    }
}
