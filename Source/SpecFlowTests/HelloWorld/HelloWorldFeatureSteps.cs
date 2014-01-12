using System.Collections.Generic;
using System.Linq;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;
using Xunit;

namespace SpecFlowTests.HelloWorld
{
    [Binding]
    public class HelloWorldFeatureSteps
    {
        private readonly List<PersonItem> _persons = new List<PersonItem>();
        private string _firstLetter;

        [Given(@"following")]
        public void GivenFollowing(Table table)
        {
            _persons.AddRange(table.CreateSet<PersonItem>());
        }

        [Given(@"the following persons")]
        public void GivenTheFollowingPersons(Table table)
        {
            _persons.AddRange(table.CreateSet<PersonItem>());
        }

        [Then(@"the person was found")]
        public void ThenThePersonWasFound(Table table)
        {
            PersonItem actual = _persons.FirstOrDefault(x => x.FirstName.StartsWith(_firstLetter));
            var expected = table.CreateInstance<PersonItem>();
            Assert.Equal(actual, expected);
        }

        [When(@"the person start on '(.*)'")]
        public void WhenThePersonStartOn(string firstLetter)
        {
            _firstLetter = firstLetter;
        }
    }

    internal class PersonItem
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            if (ReferenceEquals(this, obj))
            {
                return true;
            }
            if (obj.GetType() != GetType())
            {
                return false;
            }
            return Equals((PersonItem)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((FirstName != null ? FirstName.GetHashCode() : 0)*397)
                    ^ (LastName != null ? LastName.GetHashCode() : 0);
            }
        }

        private bool Equals(PersonItem other)
        {
            return string.Equals(FirstName, other.FirstName) && string.Equals(LastName, other.LastName);
        }
    }
}
