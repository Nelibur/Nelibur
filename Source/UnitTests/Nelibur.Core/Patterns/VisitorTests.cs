using Moq;
using Nelibur.Core.Patterns;
using Xunit;

namespace UnitTests.Nelibur.Core.Patterns
{
    public class VisitorTests
    {
        [Fact]
        public void ActionVisitor_Visit_Success()
        {
            var mock = new Mock<VisitorTests>();

            IActionVisitor<Letter> visitor = Visitor.For<Letter>();
            visitor.Register<A>(mock.Object.VisitAction);
            var value = new A();

            visitor.Visit(value);
            mock.Verify(x => x.VisitAction(value));
        }

        [Fact]
        public void FuncVisitor_Visit_Success()
        {
            var mock = new Mock<VisitorTests>();

            IFuncVisitor<Letter, int> visitor = Visitor.For<Letter, int>();
            visitor.Register<A>(mock.Object.VisitFunc);
            var value = new A();

            visitor.Visit(value);
            mock.Verify(x => x.VisitFunc(value));
        }

        protected virtual void VisitAction(A value)
        {
        }

        protected virtual int VisitFunc(A value)
        {
            return 1;
        }

        protected sealed class A : Letter
        {
        }

        protected abstract class Letter
        {
        }
    }
}
