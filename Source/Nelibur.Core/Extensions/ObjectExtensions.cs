using System;
using System.Linq.Expressions;
using Nelibur.Core.DataStructures;
using Nelibur.Core.Logging;

namespace Nelibur.Core.Extensions
{
    public static class ObjectExtensions
    {
        public static void BeginRaiseEvent<T>(this object sender, EventHandler<T> eventToRaise, T eventArgs)
            where T : EventArgs
        {
            if (eventToRaise == null)
            {
                return;
            }
            foreach (EventHandler<T> handler in eventToRaise.GetInvocationList())
            {
                EventHandler<T> handlerTemp = handler;
                handler.BeginInvoke(
                    sender,
                    eventArgs,
                    ar =>
                    {
                        try
                        {
                            handlerTemp.EndInvoke(ar);
                        }
                        catch (Exception ex)
                        {
                            LogManager.GetLogger(typeof(ObjectExtensions)).Error(ex);
                        }
                    },
                    null);
            }
        }

        public static void BeginRaiseEvent<T>(
            this object sender, EventHandler<T> eventToRaise, Func<T> eventArgsCreator)
            where T : EventArgs
        {
            if (eventToRaise == null)
            {
                return;
            }
            foreach (EventHandler<T> handler in eventToRaise.GetInvocationList())
            {
                EventHandler<T> handlerTemp = handler;
                handler.BeginInvoke(
                    sender,
                    eventArgsCreator(),
                    ar =>
                    {
                        try
                        {
                            handlerTemp.EndInvoke(ar);
                        }
                        catch (Exception ex)
                        {
                            LogManager.GetLogger(typeof(ObjectExtensions)).Error(ex);
                        }
                    },
                    null);
            }
        }

        public static string GetPropertyName<TValue, TResult>(
            this TValue obj, Expression<Func<TValue, TResult>> expression)
        {
            var member = expression.Body as MemberExpression;
            if (member != null)
            {
                return member.Member.Name;
            }
            throw new ArgumentException("Expression is not a member access", "expression");
        }

        public static bool IsNotNull(this object obj)
        {
            return obj != null;
        }

        public static bool IsNull(this object obj)
        {
            return obj == null;
        }

        public static void RaiseEvent<T>(this object sender, EventHandler<T> eventToRaise, T eventArgs)
            where T : EventArgs
        {
            if (eventToRaise == null)
            {
                return;
            }
            eventToRaise(sender, eventArgs);
        }

        public static void RaiseEvent<T>(this object sender, EventHandler<T> eventToRaise, Func<T> eventArgsCreator)
            where T : EventArgs
        {
            if (eventToRaise == null)
            {
                return;
            }
            eventToRaise(sender, eventArgsCreator());
        }

        public static Bag<T> ToBag<T>(this T value)
        {
            if (typeof(T).IsValueType == false && ReferenceEquals(value, null))
            {
                return Bag<T>.Empty;
            }
            return new Bag<T>(value);
        }

        public static Bag<TResult> ToType<TResult>(this object obj)
        {
            if (obj is TResult)
            {
                return new Bag<TResult>((TResult)obj);
            }
            return Bag<TResult>.Empty;
        }
    }
}
