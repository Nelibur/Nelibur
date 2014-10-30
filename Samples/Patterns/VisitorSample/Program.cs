using System;
using Nelibur.Sword.Patterns;

namespace VisitorSample
{
    internal static class Program
    {
        private static void Main()
        {
            new ActionVisitor().Do();
            Console.WriteLine();

            new FuncVisitor().Do();
            Console.ReadKey();
        }

        private sealed class ActionVisitor
        {
            public void Do()
            {
                Console.WriteLine("Action Visitor");
                IActionVisitor<Letter> visitor = Visitor.For<Letter>();
                visitor.Register<A>(ProcessA);
                visitor.Register<B>(ProcessB);

                Letter a = new A();
                Letter b = new B();
                visitor.Visit(a);
                visitor.Visit(b);
            }

            private static void ProcessA(A letter)
            {
                Console.WriteLine(letter.GetType().Name);
            }

            private static void ProcessB(B letter)
            {
                Console.WriteLine(letter.GetType().Name);
            }
        }

        private sealed class FuncVisitor
        {
            public void Do()
            {
                Console.WriteLine("Func Visitor");
                IFuncVisitor<Letter, string> visitor = Visitor.For<Letter, string>();
                visitor.Register<A>(ProcessA);
                visitor.Register<B>(ProcessB);

                Letter a = new A();
                Letter b = new B();
                Console.WriteLine(visitor.Visit(a));
                Console.WriteLine(visitor.Visit(b));
            }

            private static string ProcessA(A letter)
            {
                return letter.GetType().Name;
            }

            private static string ProcessB(B letter)
            {
                return letter.GetType().Name;
            }
        }
    }
}
