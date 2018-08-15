using System;
using System.Collections.Generic;
using Nelibur.Sword.Core;
using Nelibur.Sword.DataStructures;
using Nelibur.Sword.Extensions;

namespace Nelibur.Sword.Patterns
{
    /// <summary>
    ///     Visitor pattern.
    /// </summary>
    /// <remarks>
    ///     I don't like to put more than one class in a CS file, but considering I'm expecting
    ///     users to add this file to their projects, so the single file will be the best choise.
    /// </remarks>
    public static class Visitor
    {
        public static IFuncVisitor<TBase, TResult> For<TBase, TResult>()
            where TBase : class
        {
            return new FuncVisitor<TBase, TResult>();
        }

        public static IActionVisitor<TBase> For<TBase>()
            where TBase : class
        {
            return new ActionVisitor<TBase>();
        }

        public static IActionVisitor<TBase, TContext> ForWithContext<TBase, TContext>()
            where TBase : class
        {
            return new ActionVisitor<TBase, TContext>();
        }

        private sealed class ActionVisitor<TBase, TContext> : IActionVisitor<TBase, TContext>
            where TBase : class
        {
            private readonly Dictionary<Type, Action<TBase, TContext>> _repository =
                new Dictionary<Type, Action<TBase, TContext>>();

            private Option<Action> _defaultAction = Option<Action>.Empty;

            public IActionVisitor<TBase, TContext> Default(Action action)
            {
                _defaultAction = action.ToOption();
                return this;
            }

            /// <inheritdoc />
            public IActionVisitor<TBase, TContext> Register<T>(Action<T, TContext> action)
                where T : TBase
            {
                _repository[typeof(T)] = (x, y) => action((T)x, y);
                return this;
            }

            /// <inheritdoc />
            public void Visit<T>(T value, TContext context)
                where T : TBase
            {
                if (_repository.ContainsKey(value.GetType()) == false)
                {
                    _defaultAction.Do(x => x());
                    return;
                }
                Action<TBase, TContext> action = _repository[value.GetType()];
                action(value, context);
            }
        }

        private sealed class ActionVisitor<TBase> : IActionVisitor<TBase>
            where TBase : class
        {
            private readonly Dictionary<Type, Action<TBase>> _repository =
                new Dictionary<Type, Action<TBase>>();

            private Option<Action> _defaultAction = Option<Action>.Empty;

            public IActionVisitor<TBase> Default(Action action)
            {
                _defaultAction = action.ToOption();
                return this;
            }

            public IActionVisitor<TBase> Register<T>(Action<T> action)
                where T : TBase
            {
                _repository[typeof(T)] = x => action((T)x);
                return this;
            }

            public void Visit<T>(T value)
                where T : TBase
            {
                if (_repository.ContainsKey(value.GetType()) == false)
                {
                    _defaultAction.Do(x => x());
                    return;
                }
                Action<TBase> action = _repository[value.GetType()];
                action(value);
            }
        }

        private sealed class FuncVisitor<TBase, TResult> : IFuncVisitor<TBase, TResult>
            where TBase : class
        {
            private readonly Dictionary<Type, Func<TBase, TResult>> _repository =
                new Dictionary<Type, Func<TBase, TResult>>();

            private Option<Func<TResult>> _defaultAction = Option<Func<TResult>>.Empty;

            public IFuncVisitor<TBase, TResult> Default(Func<TResult> action)
            {
                _defaultAction = action.ToOption();
                return this;
            }

            public IFuncVisitor<TBase, TResult> Register<T>(Func<T, TResult> action)
                where T : TBase
            {
                _repository[typeof(T)] = x => action((T)x);
                return this;
            }

            public TResult Visit<T>(T value)
                where T : TBase
            {
                if (_repository.ContainsKey(value.GetType()) == false)
                {
                    return _defaultAction.Map(x => x()).Value;
                }
                Func<TBase, TResult> action = _repository[value.GetType()];
                return action(value);
            }
        }
    }

    public interface IFuncVisitor<in TBase, TResult> : IFluent
        where TBase : class
    {
        /// <summary>
        ///     Register default action.
        /// </summary>
        /// <param name="action">Action.</param>
        /// <returns>Result value.</returns>
        IFuncVisitor<TBase, TResult> Default(Func<TResult> action);

        /// <summary>
        ///     Register action on <see cref="T" />.
        /// </summary>
        /// <typeparam name="T">Concrete type.</typeparam>
        /// <param name="action">Action.</param>
        IFuncVisitor<TBase, TResult> Register<T>(Func<T, TResult> action)
            where T : TBase;

        /// <summary>
        ///     Visit concrete type.
        /// </summary>
        /// <param name="value">Type to visit.</param>
        /// <returns>Result value.</returns>
        TResult Visit<T>(T value)
            where T : TBase;
    }

    public interface IActionVisitor<in TBase> : IFluent
        where TBase : class
    {
        /// <summary>
        ///     Register default action.
        /// </summary>
        /// <param name="action">Action.</param>
        IActionVisitor<TBase> Default(Action action);

        /// <summary>
        ///     Register action on <see cref="T" />.
        /// </summary>
        /// <typeparam name="T">Concrete type.</typeparam>
        /// <param name="action">Action.</param>
        IActionVisitor<TBase> Register<T>(Action<T> action)
            where T : TBase;

        /// <summary>
        ///     Visit concrete type.
        /// </summary>
        /// <param name="value">Type to visit.</param>
        void Visit<T>(T value)
            where T : TBase;
    }

    public interface IActionVisitor<in TBase, TContext> : IFluent
        where TBase : class
    {
        /// <summary>
        ///     Register default action.
        /// </summary>
        /// <param name="action">Action.</param>
        IActionVisitor<TBase, TContext> Default(Action action);

        /// <summary>
        ///     Register action on <see cref="T" />.
        /// </summary>
        /// <typeparam name="T">Concrete type.</typeparam>
        /// <param name="action">Action.</param>
        IActionVisitor<TBase, TContext> Register<T>(Action<T, TContext> action)
            where T : TBase;

        /// <summary>
        ///     Visit concrete type.
        /// </summary>
        /// <param name="value">Type to visit.</param>
        /// <param name="context">Context.</param>
        void Visit<T>(T value, TContext context)
            where T : TBase;
    }
}
