using System.Linq;
using Library.WebAPI.Controllers;
using NUnit.Framework;

namespace Library.WebAPI.Tests.Unit
{
    public class BooksControllerTests
    {
        BooksController _sut = new BooksController();

        [Test]
        public void GetAll_ReturnsAllBooks()
        {
            var result = _sut.Get().ToList();

            Assert.AreEqual(2, result.Count);
        }

        [Test]
        public void GetById_ReturnABook()
        {
            var result = _sut.Get(1);

            Assert.IsNotNull(result);
            Assert.AreEqual("pesho", result.Author);
        }
    }
}