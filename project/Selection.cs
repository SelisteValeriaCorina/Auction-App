using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace project
{
    class Selection
    {//trebuie pusa lista de bidders
        List<Bidder> winners;
        Offer offer;

        private DateTime selectiondate;

        public List<Bidder> GetWinners()
        {
            return winners;
        }

        public DateTime Selectiondate
        {

            get { return selectiondate; }

            set
            {
                if(value>DateTime.Now)
                {
                    throw new ArgumentException("Check today's date!");
                }
                selectiondate = value;
            }
        }

        public Selection()
        {

        }

        public Selection(/*aici va primi ca parametru lista de bidders*/ List<Bidder> bidder)
        {
            winners = new List<Bidder>();

            ISet<String> produse = new SortedSet<String>();

            /*Cream un list de sorted lists. Fiecare sorted lists va avea participantii sortati in functie de amount*/
            List<List<Bidder>> container = new List<List<Bidder>>();

            foreach (var b in bidder)
            {
                if (!produse.Contains(b.Produs))
                {
                    List<Bidder> newListForProduct = new List<Bidder>();
                    container.Add(newListForProduct);
                }
                produse.Add(b.Produs); // adaugam produsele in set (fara duplicate)
                /*Pentru fiecare produs cream o lista*/
                
            }

            foreach (var b in bidder)
            {
                /*Set nu are functia indexOf. Convertim la lista si dupa cautam produsul respectivului participant. Cautarea returneaza pozitia sa in set si deci si numarul listei in care trebuie pus participanutl*/
                int rightList = produse.ToList().IndexOf(b.Produs);

                container.ElementAt(rightList).Add(b); // adaugam participantul in lista corecta
            }

            /*Sortam listele si extragem cea mai mare oferta din fiecare lista*/
            foreach (var list in container)
            {
                list.Sort();
                winners.Add(list.ElementAt(0));
            }
        }
            
    }
}
