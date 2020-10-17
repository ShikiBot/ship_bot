using System;
using MihaZupan;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using System.Threading;
using System.Threading.Tasks;
using System.IO;

namespace ship_bot
{
    class Program
    {
        static TelegramBotClient Bot;
        public static bool timerCheck = true;
        public static int kd = 43200000;
        static void Main(string[] args)
        {
            try
            {
                string ip = "";
                int port = 0;
                using (StreamReader sr = new StreamReader("settings.txt", System.Text.Encoding.Default))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        if (line.Contains("proxy"))
                        {
                            try
                            {
                                ip = line.Remove(line.LastIndexOf(@":"));
                                ip = ip.Remove(0, ip.LastIndexOf(@" ") + 1);
                                port = Convert.ToInt32(line.Remove(0, line.LastIndexOf(@":") + 1));
                                //Console.WriteLine(ip);
                                //Console.WriteLine(port);
                            }
                            catch
                            {
                                //Console.WriteLine("proxy не задан");
                            }
                        }
                        if (line.Contains("timeout"))
                        {
                            kd = Convert.ToInt32(line.Remove(0, line.LastIndexOf(@" ") + 1))*60*60*1000;
                            //Console.WriteLine(kd);
                        }
                    }
                }                
                if (ip == "" || port == 0)
                {
                    Bot = new TelegramBotClient("873090403:AAGZ3Lbh3j1WGuY2JQRGd8dKO1Jn3cpePGE");                    
                }
                else
                {
                    var proxy = new HttpToSocks5Proxy(ip, port);
                    Bot = new TelegramBotClient("998416041:AAEt6jRlNJSQlIDOFiCJuHtqHum-VcBytx4", proxy);
                }
                var me = Bot.GetMeAsync().Result;
                Bot.OnMessage += Bot_OnMessageReceived;
                Console.WriteLine(me.FirstName + " online");
                Bot.StartReceiving();
                Console.ReadLine();
                Bot.StopReceiving();
            }
            catch
            {
                Console.WriteLine("Error");
                Thread.Sleep(5000);
                Environment.Exit(0);
            }
        }        

        private static async void Bot_OnMessageReceived(object sender, Telegram.Bot.Args.MessageEventArgs e)
        {
            Random rnd = new Random();
            var message = e.Message;
            if (message == null || message.Type != MessageType.Text) return;
            string name = $"{DateTime.Now} | {message.From.FirstName} {message.From.LastName}", msg;
            Console.WriteLine($"{name}: {message.Text}");
            string[] persons = new string[9] { "@bled_navelny", "@koteyka7383", "@YuraIvan0v", "@SandNikita", "@mamaksimka", "@FunkyWaer", "@Kronus915", "@sunnyrs", "@German_supercat" };
            switch (message.Text)
            {
                case "/shipper":
                    if (timerCheck) answer();                    
                    break;
                case "/shipper@ShipperChan_bot":
                    if (timerCheck) answer();
                    break;                
                default:
                    break;
            }
            async void answer()
            {
                
                int LeftDogHand = (int)rnd.Next(0, 9);
                int RighDogHand = (int)rnd.Next(0, 9);
                msg = $"И в любовной паре замирают {persons[LeftDogHand]} + {persons[RighDogHand]} = ♥ ";
                await Bot.SendTextMessageAsync(message.Chat.Id, "Море волнуется раз", replyToMessageId: message.MessageId);
                Thread.Sleep(100);
                await Bot.SendTextMessageAsync(message.Chat.Id, "Море волнуется два");
                Thread.Sleep(100);
                await Bot.SendTextMessageAsync(message.Chat.Id, "Море волнуется три");
                Thread.Sleep(100);
                await Bot.SendTextMessageAsync(message.Chat.Id, msg);
                Console.WriteLine(DateTime.Now + " | " + Bot.GetMeAsync().Result.FirstName + ": " + persons[LeftDogHand] + " + " + persons[RighDogHand] + " = <3");
                timerCheck = false;
                await Task.Delay(kd);
                timerCheck = true;
            }
        }
    }
    class Randomize
    {
        UInt32 state0 = 1;
        UInt32 state1 = 2;
        float rnd;
        public float mwc1616_M()
        {
            state0 = 18030 * ((UInt32)((44655 * DateTime.Now.Ticks + 21154) % 79) & 0xffff) + (state0 >> 16);
            state1 = 30903 * (state1 & 0xffff) + (state1 >> 16);
            rnd = (float)Math.Round(((float)((state0 << 16) + (state1 & 0xffff) - 91818513)) / 4228782262, 4);
            rnd = Math.Round(rnd, 3) == 0 ? 0 : rnd;
            rnd = rnd >= 1 ? mwc1616_M() : rnd;
            Thread.Sleep(100);
            return rnd;
        }
        public float mwc1616_M(int up)
        {
            rnd = (up + 1) * mwc1616_M();
            rnd = (rnd >= up) ? mwc1616_M(up) : rnd;
            return rnd;
        }
        public float mwc1616_M(int down, int up)
        {
            rnd = (up - down + 1) * mwc1616_M() + down;
            rnd = (rnd >= up) ? mwc1616_M(down, up) : rnd;
            return rnd;
        }
    }
}

