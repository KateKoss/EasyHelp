using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace MvcApplication1.Models
{
    public class Prototype
    {
        [Serializable]
        class Request
        {
            public string state { get; set; }
         
            public Request Clone()
            {
                return this.MemberwiseClone() as Request;
            }
            public void GetInfo()
            {
                Console.WriteLine("State: ", state);
            }
            public object DeepCopy()
            {
                object figure = null;
                using (MemoryStream tempStream = new MemoryStream())
                {
                    BinaryFormatter binFormatter = new BinaryFormatter(null,
                        new StreamingContext(StreamingContextStates.Clone));

                    binFormatter.Serialize(tempStream, this);
                    tempStream.Seek(0, SeekOrigin.Begin);

                    figure = binFormatter.Deserialize(tempStream);
                }
                return figure;
            }
        }

        public void Init()
        {
            Request figure = new Request();
            figure.state = "request not resolved";
            // применяем глубокое копирование
            Request clonedFigure = figure.DeepCopy() as Request;
            
            figure.GetInfo();
            figure.state = "request resolved";
            clonedFigure.GetInfo();
        }
    }
}