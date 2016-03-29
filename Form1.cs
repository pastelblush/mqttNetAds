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
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		private TcAdsClient tcClient;		
		private int[] hConnect;
        private AdsStream dataStream;
        private BinaryReader binRead;

        private const string IotEndpoint = "localhost";
        private const int BrokerPort = 1883;
        private const string Topic = "presence";
        private MqttClient client;
        private inputs tags;
        private int bPump1, bPump2, bAuto, irThermocoupler_h;
        private Single[] irThermocoupler, rTempLimit;
        private bool iValue;

        private bool tbBool;
        private float tbReal;


		public Form1()
		{
			InitializeComponent();
            tags = new inputs();

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
            this.SuspendLayout();
            // 
            // Form1
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(352, 111);
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

			dataStream = new AdsStream(107);
			binRead = new BinaryReader(dataStream);

          	tcClient = new TcAdsClient();			
			tcClient.Connect(801);

			hConnect = new int[10];

            irThermocoupler = new Single[2];
            rTempLimit = new Single[2];

			try
			{
				hConnect[0] = tcClient.AddDeviceNotification(".DiI",dataStream,0,11,AdsTransMode.OnChange,100,0,tbBool);
                hConnect[1] = tcClient.AddDeviceNotification(".wcState", dataStream, 11, 11, AdsTransMode.OnChange, 100, 0, tbBool);
                hConnect[2] = tcClient.AddDeviceNotification(".Relay", dataStream, 22, 11, AdsTransMode.OnChange, 100, 0, tbBool);
                hConnect[3] = tcClient.AddDeviceNotification(".DiO", dataStream, 33, 11, AdsTransMode.OnChange, 100, 0, tbBool);
                hConnect[4] = tcClient.AddDeviceNotification(".bAuto", dataStream, 44, 1, AdsTransMode.OnChange, 100, 0, tbBool);
                hConnect[5] = tcClient.AddDeviceNotification(".bPump1", dataStream, 45, 1, AdsTransMode.OnChange, 100, 0, tbBool);
                hConnect[6] = tcClient.AddDeviceNotification(".bPump2", dataStream, 46, 1, AdsTransMode.OnChange, 100, 0, tbBool);
                hConnect[7] = tcClient.AddDeviceNotification(".irThermocoupler", dataStream, 47, (2*4), AdsTransMode.OnChange, 100, 0, tbReal);
                hConnect[8] = tcClient.AddDeviceNotification(".rTempLimit", dataStream, 55, (2 * 4), AdsTransMode.OnChange, 100, 0, tbReal);
                hConnect[9] = tcClient.AddDeviceNotification(".rTankLevel", dataStream, 63, (11 * 4), AdsTransMode.OnChange, 100, 0, tbBool);

				tcClient.AdsNotification += new AdsNotificationEventHandler(OnNotification);
			}
			catch(Exception err)
			{
				MessageBox.Show(err.Message);
			}

            bPump1 = tcClient.CreateVariableHandle(".bPump1");
            bPump2 = tcClient.CreateVariableHandle(".bPump2");
            bAuto = tcClient.CreateVariableHandle(".bAuto");
            irThermocoupler_h = tcClient.CreateVariableHandle(".rTempLimit");

            //event handler for inbound messages
            client.MqttMsgPublishReceived += ClientMqttMsgPublishReceived;
            client.Subscribe(new[] { "bAuto" }, new[] { MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE });
            client.Subscribe(new[] { "bPump1" }, new[] { MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE });
            client.Subscribe(new[] { "bPump2" }, new[] { MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE });
            client.Subscribe(new[] { "rTempLimit" }, new[] { MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE }); 
		}

        public void ClientMqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            if(e.Topic == "bAuto"){
                var chars = Encoding.UTF8.GetString(e.Message);
                jsontagbool tag = JsonConvert.DeserializeObject<jsontagbool>(chars);
                tcClient.WriteAny(bAuto, tag.val);
            }

            if (e.Topic == "bPump1")
            {
                var chars = Encoding.UTF8.GetString(e.Message);
                jsontagbool tag = JsonConvert.DeserializeObject<jsontagbool>(chars);
                tcClient.WriteAny(bPump1, tag.val);
            }

            if (e.Topic == "bPump2")
            {
                var chars = Encoding.UTF8.GetString(e.Message);
                jsontagbool tag = JsonConvert.DeserializeObject<jsontagbool>(chars);
                tcClient.WriteAny(bPump2, tag.val);
            }

            if (e.Topic == "rTempLimit")
            {
                var chars = Encoding.UTF8.GetString(e.Message);
                jsontagreal tag = JsonConvert.DeserializeObject<jsontagreal>(chars);
                rTempLimit[0] = tag.val;
                tcClient.WriteAny(irThermocoupler_h, rTempLimit);
            }
            



            //tcClient.WriteAny(iHandle, tag.val);
        }

		private void OnNotification(object sender, AdsNotificationEventArgs e)
		{
			e.DataStream.Position = e.Offset;					       
            if( e.NotificationHandle == hConnect[0])
            {
                tags.bFloat1 = binRead.ReadBoolean();
                tags.bFloat2 = binRead.ReadBoolean();
                tags.bFloat3 = binRead.ReadBoolean();
                tags.bAuto = binRead.ReadBoolean();
            }

            if (e.NotificationHandle == hConnect[1])
            {
                tags.bIOTerm = binRead.ReadBoolean();
            }

            if (e.NotificationHandle == hConnect[2])
            {
                tags.bTemp = binRead.ReadBoolean();
                tags.bTemp = binRead.ReadBoolean();
            }

            if (e.NotificationHandle == hConnect[3])
            {
                //tags.bPump1 = binRead.ReadBoolean();
                //tags.bPump2 = binRead.ReadBoolean();
            }

            if (e.NotificationHandle == hConnect[4])
            {
                tags.bAuto = binRead.ReadBoolean();
            }
            
            if (e.NotificationHandle == hConnect[5])
            {
                tags.bPump1 = binRead.ReadBoolean();
            }

            if (e.NotificationHandle == hConnect[6])
            {
                tags.bPump2 = binRead.ReadBoolean();
            }

            if (e.NotificationHandle == hConnect[7])
            {
                //tags.rTemp = binRead.ReadSingle();
                irThermocoupler[0] = binRead.ReadSingle();
                irThermocoupler[1] = binRead.ReadSingle();
                tags.rTemp = irThermocoupler[0];
            }

            if (e.NotificationHandle == hConnect[8])
            {
                tags.rTempLimit = binRead.ReadSingle();
            }

            if (e.NotificationHandle == hConnect[9])
            {
                tags.rTankLevel = binRead.ReadSingle();
            }
            



            string json = JsonConvert.SerializeObject(tags);		
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


    public class inputs
    {
        public bool bEmergency;
        public bool bIOTerm;
        public float rTemp;
        public bool bTemp;
        public bool bFloat1;
        public bool bFloat2;
        public bool bFloat3;
        public float rTankLevel;
        public bool bPump1;
        public bool bPump2;
        public bool bAuto;
        public float rTempLimit;
    }

    public class jsontagbool
    {
        public bool val;
    }
    public class jsontagreal
    {
        public Single val;
    }
}
