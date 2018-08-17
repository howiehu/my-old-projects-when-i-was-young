using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EEDDMS.Domain.Abstract;
using EEDDMS.Domain.Entities;
using System.Data;
using System.Reflection;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace EEDDMS.Domain.Concrete
{
    public class EFLocationRepository : ILocationRepository
    {
        private EFDbContext context = new EFDbContext();
        public IQueryable<Location> Locations
        {
            get { return context.Locations; }
        }

        public void SaveLocation(Location location)
        {
            if (location.Id == Guid.Empty)
            {
                context.Locations.Add(location);
            }
            else
            {
                location.ModifyDate = DateTime.Now;

                context.Entry(location).State = EntityState.Modified;
            }

            context.SaveChanges();
        }

        public void DeleteLocation(Location location)
        {
            context.Locations.Remove(location);

            context.SaveChanges();
        }
        /// <summary>
        /// 将字符串转换成日期
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private DateTime ConvertDateTime(string str)
        {
            DateTime datetime = DateTime.Parse("1900-01-01");
            DateTime.TryParse(str, out datetime);
            return datetime;
        }
        public IQueryable<Location> QuickQuery(string queryCondition)
        {


            //得到相当上下文的类型
            Type type = context.GetType();
            //搜索具体指定名称的公共属性
            PropertyInfo info = type.GetProperty("Locations");
            //返回字符串表示的属性的值
            dynamic query = (IEnumerable<object>)info.GetValue(context, null);
            IQueryable<Location> locations = null;
            locations = context.Locations.SqlQuery("select * from Locations where Name!='" + queryCondition + "'").AsQueryable();
            dynamic result = query.SqlQuery("select * from Locations where Name!='" + queryCondition + "'");

            locations = result.AsQueryable();
            return locations;
        }
    }
}
