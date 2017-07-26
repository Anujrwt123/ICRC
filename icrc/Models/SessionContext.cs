using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IC_RC.Models
{
    public class SessionContext<T>
    {

        private static SessionContext<T> _instance;

        public static SessionContext<T> Instance
        {
            get
            {
                return _instance ?? (_instance = new SessionContext<T>());
            }
        }

        public void SetSession(string name, T obj)
        {
            HttpContext.Current.Session[name] = obj;
        }


        public T GetSession(string name)
        {

            if (HttpContext.Current.Session[name] != null)
            {
                return (T)HttpContext.Current.Session[name];
            }
            return default(T);
        }

    }
}