using OurLibrary.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace OurLibrary.Service
{
    public class BaseService
    {
        public int count = 0;
        protected LibraryEntities dbEntities = new LibraryEntities();

        public virtual List<object> ObjectList(int offset, int limit)
        {
            return null;
        }

        public virtual object Update(object obj)
        {
            return null;
        }

        public virtual object GetById(string Id)
        {
            return null;
        }

        public virtual void Delete(object obj)
        {
            
        }

        public virtual int ObjectCount()
        {
            return 0;
        }

        public virtual object Add(object Obj)
        {
            return null;
        }

        public virtual List<object> SearchAdvanced(Dictionary<string, object> Params, int limit = 0, int offset = 0)
        {
            return null;
        }

        public virtual int countSQL(string sql, object dbSet)
        {
            return 0;
        }

        public virtual int getCountSearch()
        {
            return count;
        }
    }
}