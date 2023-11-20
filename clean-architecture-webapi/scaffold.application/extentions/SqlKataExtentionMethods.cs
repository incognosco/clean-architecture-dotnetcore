using SqlKata;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Scaffold.Application.Extentions
{
    public static class SqlKataExtensionMethods
    {
        public static Query From<T>(this Query q)
        {
            return q.From(typeof(T).Name);
        }

        //public static Query Join<R, T>(this Query q)
        //{
        //    var rname = typeof(R).Name;
        //    var tname = typeof(T).Name;

        //    return q.Join($"{tname}", $"{rname}.Id", $"{tname}.{rname}Id");
        //}

        //public static Query JoinChild<R, T>(this Query q)
        //{
        //    var rname = typeof(R).Name;
        //    var tname = typeof(T).Name;

        //    return q.Join($"{tname}", $"{tname}.Id", $"{rname}.{tname}Id");
        //}

        //public static Query Where<T>(this Query q, string field, object val)
        //{
        //    return q.Where($"{typeof(T).Name}.{field}", val);
        //}
        //public static Query AndWhere<T>(this Query q, string field, object val)
        //{
        //    return q.AndWhere<T>($"{typeof(T).Name}.{field}", val);
        //}
        //public static Query OrWhere<T>(this Query q, string field, object val)
        //{
        //    return q.OrWhere<T>($"{typeof(T).Name}.{field}", val);
        //}


        //public static Query Where<T, R>(this Query q, Expression<Func<T, R>> field, R value)
        //{
        //    var mInfo = (System.Linq.Expressions.MemberExpression)(field.Body);
        //    var memName = mInfo.Member;
        //    q.Where(memName.Name, value);
        //    return q;
        //}

        public static Query Where<T, R>(this Query q, Func<T, R> field, R value)
        {
            var mInfo = field.Method.Name;
            q.Where(mInfo, value);
            return q;
        }

        public static Query AndWhere<T, R>(this Query q, Func<T, R> field, R value)
        {
            var mInfo = field.Method.Name;
            q.Where<T, R>(field, value);
            return q;
        }
        public static Query OrWhere<T, R>(this Query q, Func<T, R> field, R value)
        {
            var mInfo = field.Method.Name;
            q.OrWhere(mInfo, value);
            return q;
        }
    }
}
