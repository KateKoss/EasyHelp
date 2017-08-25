using MvcApplication1.Contexts;
using MvcApplication1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcApplication1.Controllers
{
    /// <summary>
    /// The 'Command' abstract class
    /// </summary>
    public abstract class ICommand
    {
        public abstract void Execute(ProfileModel model);
        public abstract string Undo(string tegs, string str);
    }
    public class Command
    {     
          /// <summary>
          /// The 'ConcreteCommand' class
          /// </summary>
          class AddTegCommand : ICommand
          {            
            // Constructor
            public AddTegCommand()
            {
              
            }

            // Returns opposite operator for given operator
            public override string Undo(string tegs, string str)
            {
                if (tegs.Contains(' ' + str + ' '))
                    return tegs.Replace(' ' + str + ' ', " ");
                else
                {
                    if (tegs.Contains(' ' + str))
                        return tegs.Replace(' ' + str, " ");
                    else if (tegs.Contains(str + ' '))
                        return tegs.Substring(str.Length + 1);
                    else
                        return tegs + str + " ";
                }
            }
 
            // Execute new command
            public override void Execute(ProfileModel model)
            {
              
              var str = "";
              if (model.SelectedTeg != null)
                  foreach (string s in model.SelectedTeg)
                  {
                      //model.TegList.SingleOrDefault(x => x.Value == s);

                      str += model.TegList.SingleOrDefault(x => x.Value == s).Text;
                  }

              using (CustomDbContext db = new CustomDbContext())
              {
                  Singleton s = Singleton.Instance;
                  
                  string currentPerson;
                  currentPerson = s.user;
                  var user = db.UserProfiles.SingleOrDefault(x => x.UserName == currentPerson);
                  if (user != null)
                  {
                      var myModel = db.ProfileModel.SingleOrDefault(x => x.UserName == currentPerson);
                      var tegs = myModel.MyTegs;
                      if (tegs != null)
                      {
                          if (tegs.Contains(' ' + str + ' ') || tegs.Contains(' ' + str) || tegs.Contains(str + ' '))
                              myModel.MyTegs = Undo(tegs, str);
                          //if (tegs.Contains(' ' + str + ' '))
                          //    myModel.MyTegs = tegs.Replace(' ' + str + ' ', " ");
                          else
                          {
                              //if (tegs.Contains(' ' + str))
                              //    myModel.MyTegs = tegs.Replace(' ' + str, " ");
                              //else if (tegs.Contains(str + ' '))
                              //    myModel.MyTegs = tegs.Substring(str.Length + 1);
                              //else 
                              myModel.MyTegs = tegs + str + " ";
                          }
                      }
                      else myModel.MyTegs = str + " ";


                      db.SaveChanges();
                  }
                  else model = new ProfileModel() { };
              }
            }
  
            
          }
 
          /// <summary>
          /// The 'Invoker' class
          /// </summary>
          public class User
          {
            public void AddTeg(ProfileModel model)
            {
              // Create command operation and execute it
              ICommand command = new AddTegCommand();
              command.Execute(model);              
            }
  
        }
    }
}