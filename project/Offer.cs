using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace project
{
    [Serializable]
    public class Offer
    {
        String Offeror;
        
        float Amount;
        
      public float getamount()
      {
            return Amount;
      }
        public void setamount(float _amount)
        {
            Amount = _amount;
        }

        public Offer(float amount)
        {
            this.Amount = amount;
           
        }
        public Offer()
        { }
    }
}
