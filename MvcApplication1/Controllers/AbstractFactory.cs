using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcApplication1.Models
{
    public class AbstractFactory
    {
        // AbstractFactory - дозволить нам у проекті ввести тип заявки і отримати інормацію 
        //про те, що з заявкою такого типу можна роибити і в яких станах вона буває
        //тип заявки 1) заявка про допомогу (заявка учня)
        //           2) заявка, що надійшла (заявка ментора)
        abstract class RequestFactory
        {
            public abstract void SetInf();
            public abstract string GetInf();
        };

        // ConcreteFactory1
        class StudentRequestFactory: RequestFactory
        {
            public StudentRequestFactory() {}	       

	        public override void SetInf() {                
		        info = "States: not resolved, resolved, canceled.\n You can resolve or cancel this request.";
	        }

            public override string GetInf()
            {
                return info;
            }

            private string info;
            
        };

        // ConcreteFactory2
        class MentorRequestFactory: RequestFactory
        {
            public MentorRequestFactory() { }	       

	        public override void SetInf() {                
		        info = "States: not resolved, resolved, canceled.\n You can remove this request only from your list.";
	        }

            public override string GetInf()
            {
                return info;
            }
            private string info;
            
        };

        // Client, request - AbstractProduct
        void CreateRequest(RequestFactory r)
        {
	        r.SetInf();	        
        }

        void Init()
        {
	        // ConcreteProducts
            StudentRequestFactory sr = new StudentRequestFactory();
            MentorRequestFactory mr = new MentorRequestFactory();

	        CreateRequest(sr);
	        CreateRequest(mr);

	        sr.SetInf();
	        mr.SetInf();

            var info1 = sr.GetInf();
            var info2 = mr.GetInf();
        }
    }
}