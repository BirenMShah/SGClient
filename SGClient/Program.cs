using Newtonsoft.Json;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Configuration;
using System.Threading.Tasks;

namespace SGClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Execute().Wait();
           

            Console.ReadLine();
        }

        static async Task Execute()
        {
            try
            {
                //create email message and serilaze it to JSON string and insert into queue database
                //this will happen in the app that is trying to send email

                var msg = new SendGridMessage
                {
                    From = new EmailAddress("from@from.com")
                };
                msg.AddTo(new EmailAddress("to@to.com"));
                msg.Subject = "sent using API";
                var templateID as string = "*********************";
                msg.SetTemplateId(templateID);
                string jsonString = msg.Serialize();

                //insert above json string to queue database.
                //queue table 
                // update queue entry with 
                // queue table has 7 fields
                // queue_id : int
                // message: varchar(max)
                // flgProcessed: boolean (default is false)
                // messageSubmitStatus: varchar(255)
                // entryDate: datetime
                // sentDate: dateTime
                // serviceServer : varchar(255)


                //following happens in service logic where records that not processed are bsing read, message gets deserialized and gets sent and record is updated with senttime, status, flgProcessed
                var key = ConfigurationManager.AppSettings["APIKEY"];
                var client = new SendGridClient(key);
                

                Object json = JsonConvert.DeserializeObject<Object>(jsonString);
                var response = await client.RequestAsync(SendGridClient.Method.POST,
                                                     json.ToString(),
                                                     urlPath: "mail/send");

                var responseCode = response.StatusCode.ToString();

                


            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
