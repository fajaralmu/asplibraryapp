using OurLibrary.Models;
using OurLibrary.Util.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OurLibrary.Service
{
    public class ClassService : BaseService
    {
        public ClassService()
        {

        }

        public override List<object> ObjectList(int offset, int limit)
        {
            List<object> ObjList = new List<object>();
            var Sql = (from p in dbEntities.classes orderby p.class_name select p);
            List<@class> List = Sql.Skip(offset * limit).Take(limit).ToList();
            foreach (@class c in List)
            {
                ObjList.Add(c);
            }
            return ObjList;
        }
        public override object Update(object Obj)
        {
            @class Class = (@class)Obj;
            dbEntities.Entry(Class).CurrentValues.SetValues(Class);
            dbEntities.SaveChanges();
            return Class;
        }

        public override object GetById(string Id)
        {
            @class Class = (from c in dbEntities.classes where c.id.Equals(Id) select c).SingleOrDefault();
            return Class;
        }

        public override void Delete(object Obj)
        {
            @class Class = (@class)Obj;
            dbEntities.classes.Remove(Class);
            dbEntities.SaveChanges();
        }

        public override int ObjectCount()
        {
            return dbEntities.classes.Count();
        }

        public override object Add(object Obj)
        {
            @class Class = (@class)Obj;
            if(Class.id==null)
                Class.id = StringUtil.GenerateRandom(7);
            @class newclass = dbEntities.classes.Add(Class);
            dbEntities.SaveChanges();
            return newclass;

        }
    }
}