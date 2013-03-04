using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fanex.TicketControl.Misc.Services.Interface;
using Fanex.TicketControl.Misc.Entities;
using Fanex.TicketControl.Core.Models;
using Fanex.TicketControl.Core.Services.Impl;
using System.Web;
using System.IO;
using Fanex.TicketControl.Core.Web;
using Fanex.TicketControl.Core.Utils;

namespace Fanex.TicketControl.Misc.Services.Impl
{
    public class NumberWheelService : INumberWheelService
    {
        private List<NumberWheelChoice> _listChoice;
        private const string NumberGamePath = "~/App_Data/NumberWheelChoice.txt";
        public NumberWheelService()
        {
            _listChoice = new List<NumberWheelChoice>();

            var objListChoice = CacheManager.Get<List<NumberWheelChoice>>(CacheServiceKey.KeyNumberWheelChoice);

            if (objListChoice == null)
            {
                _listChoice = GetDefaultChoiceList();
                CacheManager.Set<List<NumberWheelChoice>>(CacheServiceKey.KeyNumberWheelChoice, _listChoice);
            }
            else
            {
                _listChoice = objListChoice as List<NumberWheelChoice>;
            }
        }

        public List<NumberWheelChoice> GetChoiceList()
        {
            return _listChoice;
        }

        public ActionResult InsertNewChoice(NumberWheelChoice newNumberWheelChoice)
        {
            throw new NotImplementedException();
        }

        public ActionResult UpdateChoice(int hdp1, int betTeam)
        {
            throw new NotImplementedException();
        }


        public string GetChoiceName(int hdp1, int betTeam)
        {
            if (_listChoice.Count.Equals(0)) return string.Empty;

            try
            {
                var objChoice = _listChoice.Where(item => item.Hdp1.Equals(hdp1) && item.Betteam.Equals(betTeam)).FirstOrDefault();

                if (objChoice != null) return objChoice.Choice;
            }
            catch (Exception ex)
            {
                LoggingHandler.Log(ex);
            }

            return string.Empty;
        }


        private static List<NumberWheelChoice> GetDefaultChoiceList()
        {
            var listChoice = new List<NumberWheelChoice>();
            var strNumberWheelDataPath = HttpContext.Current.Server.MapPath(NumberGamePath);

            try
            {
                if (File.Exists(strNumberWheelDataPath))
                {
                    using (var sr = new StreamReader(strNumberWheelDataPath))
                    {
                        while (sr.Peek() >= 0)
                        {
                            var strCurrent = sr.ReadLine();
                            if (string.IsNullOrEmpty(strCurrent))
                                continue;
                            var arrData = strCurrent.Split(' ');
                            if (arrData.Length < 3)
                                continue;
                            var choice = new NumberWheelChoice();
                            choice.Hdp1 = Convert.ToInt32(arrData[0]);
                            choice.Betteam = Convert.ToInt32(arrData[1]);
                            choice.Choice = arrData[2];

                            listChoice.Add(choice);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LoggingHandler.Log(ex);
            }

            return listChoice;
        }
    }
}
