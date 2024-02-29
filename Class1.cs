using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ermakova
{
    
    internal class classUser
    {
        public static Users user;
    }
    partial class Trainers
    {
        public override string ToString()
        {
            return  last_name + " "+ name[0] + ". " + fathers_name[0] +". ";
        }
    }
    partial class Clients
    {
        public override string ToString()
        {
            return last_name + " " + name[0] + ". " + fathers_name[0] + ". ";
        }
    }
    partial class Types_season_tickets
    {
        public override string ToString()
        {
            return type;
        }
    }
    partial class Season_tickets
    {
        Entities1 entities = new Entities1();
        public override string ToString()
        {
            var client = (from clnt in entities.Clients
                          where clnt.Id == this.id_client
                          select clnt
                            ).First<Clients>();

            string clientFIO = client.last_name + " "
                                + client.name[2] + " "
                                + client.fathers_name[2];


            return "Абонемент №" + Id +  ". заморожен:" + freeze_status + " " + clientFIO;
                    
        }
    }
    partial class Visits
    {
        public override string ToString()
        {
            DateTime datetime = new DateTime();
            datetime = (DateTime)this.date;

            return "Запись №"+Id + "-" + id_ticket + " " + inOutType +" " + datetime.ToShortDateString() + " " + time ;
        }
    }
}
