using AutoMapper;
using BLL.DTO;
using BLL.Interfaces;
using BLL.Infrastructure;
using BLL.Services;
using DAL.Entities;
using DAL.Interfaces;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using BLL.Exceptions;

namespace BLLUnitTest.Service
{
    [TestFixture]
    public class CategoryServiceTest
    {
        static CategoryServiceTest()
        {
            try
            {
                Mapper.Initialize(cfg =>
                AutoMapperConfig.Configure(cfg)
                );
            }
            catch { }
        }

        private ICategoryService categoryService;
        private Mock<IUnitOfWork> uow;
        private Mock<IRepository<Category>> categoryRepository;

        [SetUp]
        public void Load()
        {
            uow = new Mock<IUnitOfWork>();
            categoryRepository = new Mock<IRepository<Category>>();

            uow.Setup(x => x.Categories).Returns(categoryRepository.Object);

            categoryService = new CategoryService(uow.Object);
        }

        [Test]
        public void CreateCategory_TryToCreateNullElement_ShouldThrowException()
        {
            //act & assert
            Assert.Throws<ArgumentNullException>(() => categoryService.CreateCategory(null));
        }

        [Test]
        public void CreateCategory_TryToCreate_RepositoryShouldCallOnce()
        {
            //act
            categoryService.CreateCategory(new CategoryDTO { Name = It.IsAny<string>()});

            //assert
            categoryRepository.Verify(x => x.Create(It.IsAny<Category>()), Times.Once);
        }

        [Test]
        public void EditCategory_TryToEdinNullValue_ShouldThrowException()
        {
            //act & assert
            Assert.Throws<ArgumentNullException>(() => categoryService.EditCategory(null));
        }

        [Test]
        public void EditCategory_TryToEdinNonExictingCategory_ShouldThrowException()
        {
            //arrange
            categoryRepository.Setup(x => x.Get(It.Is<int>(y => y > 0))).Returns<Category>(null);

            //act & assert
            Assert.Throws<ArgumentNullException>(() => categoryService.EditCategory(It.IsAny<CategoryDTO>()));
        }

        [Test]
        public void EditCategory_TryToEdit_RepositoryShouldCallOnce()
        {
            //arrange
            categoryRepository.Setup(x => x.Get(It.IsAny<int>())).Returns(new Category());

            //act 
            categoryService.EditCategory(new CategoryDTO { Name = It.IsAny<string>()});

            //assert
            categoryRepository.Verify(x => x.Update(It.IsAny<Category>()), Times.Once);
        }

        [Test]
        public void RemoveCategory_TryToRemoveDefaultCategory_ShouldThrowException()
        {
            //act & assert
            var  ex = Assert.Throws<AuctionException>(() => categoryService.RemoveCategory(1));
            Assert.AreEqual(ex.Message, "You can't delete default category");
        }

        [Test]
        public void RemoveCategory_TryToRemoveNonExistingCategory_ShouldThrowException()
        {
            //arrange
            categoryRepository.Setup(x => x.Get(It.IsAny<int>())).Returns<Category>(null);

            //act & assert
            Assert.Throws<ArgumentNullException>(() => categoryService.RemoveCategory(It.IsAny<int>()));
        }

        [Test]
        public void RemoveCategory_TryToRemoveCategory_RepositoryShouldCallOnce()
        {
            //arrange
            categoryRepository.Setup(x => x.Get(It.IsAny<int>())).Returns(new Category { Lots = new List<Lot> { new Lot { } } });

            //act
            categoryService.RemoveCategory(It.IsAny<int>());

            //assert
            categoryRepository.Verify(x => x.Delete(It.IsAny<int>()), Times.Once);
        }
    }
}
