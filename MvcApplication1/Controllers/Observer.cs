using MvcApplication1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcApplication1.Controllers
{
    /// <summary>
    /// The 'Subject' abstract class
    /// </summary>
    public abstract class Stock
    {
        private RequestModel req;
        private List<IObserver> mentors = new List<IObserver>();

        // Constructor
        public Stock(RequestModel r)
        {            
            this.req = r;
        }

        public void Attach(IObserver mentor)
        {
            mentors.Add(mentor);
        }

        public void Detach(IObserver mentor)
        {
            mentors.Remove(mentor);
        }

        public void Notify()
        {
            foreach (IObserver i in mentors)
            {
                i.Update(this);
            }

            Console.WriteLine("");
        }

        // Gets or sets the price
        public RequestModel Request
        {
            get { return req; }
            set
            {
                if (req != value)
                {
                    req = value;
                    Notify();
                }
            }
        }

    }

    /// <summary>
    /// The 'ConcreteSubject' class
    /// </summary>
    class Requests : Stock
    {
        // Constructor
        public Requests(RequestModel r) : base(r)
        {
        }
    }

    /// <summary>
    /// The 'Observer' interface
    /// </summary>
    public abstract class IObserver
    {
        public abstract void Update(Stock stock);
    }

    /// <summary>
    /// The 'ConcreteObserver' class
    /// </summary>
    public class Observer : IObserver
    {
        private string _name;
        private Stock _stock;

        // Constructor
        public Observer(string name)
        {
            this._name = name;
        }

        public override void Update(Stock stock)
        {
            RequestModel r = new RequestModel();
            r.isUpdate = true;                 
        }

        // Gets or sets the stock
        Stock Stock
        {
            get { return _stock; }
            set { _stock = value; }
        }
    }    
}