using FluentAssertions;
using FIAP.TechChalenge.EpicCollections.Application.Exceptions;
using FIAP.TechChalenge.EpicCollections.Domain.SeedWork.SearchableRepository;
using FIAP.TechChalenge.EpicCollections.Infra.Data.EF;
using Repository = FIAP.TechChalenge.EpicCollections.Infra.Data.EF.Repositories;
using FIAP.TechChalenge.EpicCollections.Domain.Entity.Collection;

namespace FIAP.TechChalenge.EpicCollections.IntegrationTests.Infra.Data.EF.Repositories.CollectionRepository
{
    [Collection(nameof(CollectionRepositoryTestFixture))]
    public class CollectionRepositoryTest
    {
        private readonly CollectionRepositoryTestFixture _fixture;

        public CollectionRepositoryTest(CollectionRepositoryTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact(DisplayName = "Insert")]
        [Trait("Integration/Infra.Data", "CollectionRepository - Repositories")]
        public async Task Insert()
        {
            EpicCollectionsDbContext dbContext = _fixture.CreateDbContext();
            var userId = Guid.NewGuid();
            var exampleCollection = _fixture.GetExampleCollection(userId);
            var collectionRepository = new Repository.CollectionRepository(dbContext);

            await collectionRepository.Insert(exampleCollection, CancellationToken.None);
            await dbContext.SaveChangesAsync(CancellationToken.None);

            var dbCollection = await (_fixture.CreateDbContext(true))
                .Collections.FindAsync(exampleCollection.Id);

            dbCollection.Should().NotBeNull();
            dbCollection.Id.Should().Be(exampleCollection.Id);
            dbCollection.Name.Should().Be(exampleCollection.Name);
            dbCollection.Description.Should().Be(exampleCollection.Description);
            dbCollection.Category.Should().Be(exampleCollection.Category);
            dbCollection.UserId.Should().Be(exampleCollection.UserId);
            dbCollection.CreatedAt.Should().BeCloseTo(exampleCollection.CreatedAt, TimeSpan.FromSeconds(1));
        }

        [Fact(DisplayName = "Get")]
        [Trait("Integration/Infra.Data", "CollectionRepository - Repositories")]
        public async Task Get()
        {
            EpicCollectionsDbContext dbContext = _fixture.CreateDbContext();
            var userId = Guid.NewGuid();
            var exampleCollection = _fixture.GetExampleCollection(userId);
            var exampleCollectionList = _fixture.GetExampleCollectionList(userId, 15);
            exampleCollectionList.Add(exampleCollection);
            await dbContext.AddRangeAsync(exampleCollectionList);
            await dbContext.SaveChangesAsync(CancellationToken.None);
            var collectionRepository = new Repository.CollectionRepository(_fixture.CreateDbContext(true));

            var dbCollection = await collectionRepository.Get(exampleCollection.Id, CancellationToken.None);

            dbCollection.Should().NotBeNull();
            dbCollection.Id.Should().Be(exampleCollection.Id);
            dbCollection.Name.Should().Be(exampleCollection.Name);
            dbCollection.Description.Should().Be(exampleCollection.Description);
            dbCollection.Category.Should().Be(exampleCollection.Category);
            dbCollection.UserId.Should().Be(exampleCollection.UserId);
            dbCollection.CreatedAt.Should().BeCloseTo(exampleCollection.CreatedAt, TimeSpan.FromSeconds(1));
        }

        [Fact(DisplayName = "GetThrowIfNotFound")]
        [Trait("Integration/Infra.Data", "CollectionRepository - Repositories")]
        public async Task GetThrowIfNotFound()
        {
            EpicCollectionsDbContext dbContext = _fixture.CreateDbContext();
            var exampleId = Guid.NewGuid();
            var exampleCollectionList = _fixture.GetExampleCollectionList(Guid.NewGuid(), 15);
            await dbContext.AddRangeAsync(exampleCollectionList);
            await dbContext.SaveChangesAsync(CancellationToken.None);
            var collectionRepository = new Repository.CollectionRepository(dbContext);

            var task = async () => await collectionRepository.Get(exampleId, CancellationToken.None);

            await task.Should().ThrowAsync<NotFoundException>()
                .WithMessage($"Collection with id {exampleId} not found");
        }

        [Fact(DisplayName = "Update")]
        [Trait("Integration/Infra.Data", "CollectionRepository - Repositories")]
        public async Task Update()
        {
            EpicCollectionsDbContext dbContext = _fixture.CreateDbContext();
            var userId = Guid.NewGuid();
            var exampleCollection = _fixture.GetExampleCollection(userId);
            var newCollection = _fixture.GetExampleCollection(userId);
            var exampleCollectionList = _fixture.GetExampleCollectionList(userId, 15);
            exampleCollectionList.Add(exampleCollection);
            await dbContext.AddRangeAsync(exampleCollectionList);
            await dbContext.SaveChangesAsync(CancellationToken.None);
            var collectionRepository = new Repository.CollectionRepository(dbContext);

            exampleCollection.Update(newCollection.Name, newCollection.Description, newCollection.Category);
            await collectionRepository.Update(exampleCollection, CancellationToken.None);
            await dbContext.SaveChangesAsync(CancellationToken.None);

            var dbCollection = await (_fixture.CreateDbContext(true))
                .Collections.FindAsync(exampleCollection.Id);

            dbCollection.Should().NotBeNull();
            dbCollection!.Id.Should().Be(exampleCollection.Id);
            dbCollection.Name.Should().Be(exampleCollection.Name);
            dbCollection.Description.Should().Be(exampleCollection.Description);
            dbCollection.Category.Should().Be(exampleCollection.Category);
            dbCollection.UserId.Should().Be(exampleCollection.UserId);
            dbCollection.CreatedAt.Should().BeCloseTo(exampleCollection.CreatedAt, TimeSpan.FromSeconds(1));
        }

        [Fact(DisplayName = "Delete")]
        [Trait("Integration/Infra.Data", "CollectionRepository - Repositories")]
        public async Task Delete()
        {
            EpicCollectionsDbContext dbContext = _fixture.CreateDbContext();
            var userId = Guid.NewGuid();
            var exampleCollection = _fixture.GetExampleCollection(userId);
            var exampleCollectionList = _fixture.GetExampleCollectionList(userId, 15);
            exampleCollectionList.Add(exampleCollection);
            await dbContext.AddRangeAsync(exampleCollectionList);
            await dbContext.SaveChangesAsync(CancellationToken.None);
            var collectionRepository = new Repository.CollectionRepository(dbContext);

            await collectionRepository.Delete(exampleCollection, CancellationToken.None);
            await dbContext.SaveChangesAsync(CancellationToken.None);

            var dbCollection = await (_fixture.CreateDbContext(true))
                .Collections.FindAsync(exampleCollection.Id);

            dbCollection.Should().BeNull();
        }

        [Fact(DisplayName = "SearchReturnsEmpty")]
        [Trait("Integration/Infra.Data", "CollectionRepository - Repositories")]
        public async Task SearchReturnsEmpty()
        {
            EpicCollectionsDbContext dbContext = _fixture.CreateDbContext();
            var collectionRepository = new Repository.CollectionRepository(dbContext);
            var searchInput = new SearchInput(
                page: 1,
                perPage: 10,
                search: "",
                orderBy: "",
                SearchOrder.Asc
            );

            var output = await collectionRepository.Search(searchInput, CancellationToken.None);

            output.Should().NotBeNull();
            output.Items.Should().NotBeNull();
            output.CurrentPage.Should().Be(searchInput.Page);
            output.PerPage.Should().Be(searchInput.PerPage);
            output.Total.Should().Be(0);
            output.Items.Should().HaveCount(0);
        }

        [Theory(DisplayName = "SearchReturnsPaginated")]
        [Trait("Integration/Infra.Data", "CollectionRepository - Repositories")]
        [InlineData(10, 1, 5, 5)]
        [InlineData(7, 2, 5, 2)]
        [InlineData(10, 2, 5, 5)]
        [InlineData(7, 3, 5, 0)]
        public async Task SearchReturnsPaginated(
            int itemsToGenerate,
            int page,
            int perPage,
            int expectedTotal
        )
        {
            EpicCollectionsDbContext dbContext = _fixture.CreateDbContext();
            var userId = Guid.NewGuid();
            var exampleCollectionList = _fixture.GetExampleCollectionList(userId, itemsToGenerate);
            await dbContext.AddRangeAsync(exampleCollectionList);
            await dbContext.SaveChangesAsync(CancellationToken.None);
            var collectionRepository = new Repository.CollectionRepository(dbContext);
            var searchInput = new SearchInput(
                page: page,
                perPage: perPage,
                search: "",
                orderBy: "",
                SearchOrder.Asc
            );

            var output = await collectionRepository.Search(searchInput, CancellationToken.None);

            output.Should().NotBeNull();
            output.Items.Should().NotBeNull();
            output.CurrentPage.Should().Be(searchInput.Page);
            output.PerPage.Should().Be(searchInput.PerPage);
            output.Total.Should().Be(exampleCollectionList.Count);
            output.Items.Should().HaveCount(expectedTotal);
            foreach (Collection outputItem in output.Items)
            {
                var exampleItem = exampleCollectionList.Find(x => x.Id == outputItem.Id);
                outputItem.Should().NotBeNull();
                outputItem!.Id.Should().Be(exampleItem!.Id);
                outputItem.Name.Should().Be(exampleItem.Name);
                outputItem.Description.Should().Be(exampleItem.Description);
                outputItem.Category.Should().Be(exampleItem.Category);
                outputItem.UserId.Should().Be(exampleItem.UserId);
                outputItem.CreatedAt.Should().BeCloseTo(exampleItem.CreatedAt, TimeSpan.FromSeconds(1));
            }
        }

        [Theory(DisplayName = "SearchByText")]
        [Trait("Integration/Infra.Data", "CollectionRepository - Repositories")]
        [InlineData("Collection 1", 1, 5, 1, 1)]
        [InlineData("Description", 1, 5, 1, 1)]
        [InlineData("Random", 1, 5, 0, 0)]
        public async Task SearchByText(
            string search,
            int page,
            int perPage,
            int expectedTotalResult,
            int expectedTotalItems
        )
        {
            EpicCollectionsDbContext dbContext = _fixture.CreateDbContext();
            var userId = Guid.NewGuid();
            var exampleCollectionList = _fixture.GetExampleCollectionListWithNames(
                userId,
                new List<string>()
                {
                "Collection 1",
                "Collection 2",
                "Description Collection",
                "Example Collection 3",
                "Test Collection"
                }
            );
            await dbContext.AddRangeAsync(exampleCollectionList);
            await dbContext.SaveChangesAsync(CancellationToken.None);
            var collectionRepository = new Repository.CollectionRepository(dbContext);
            var searchInput = new SearchInput(
                page,
                perPage,
                search,
                orderBy: "",
                SearchOrder.Asc
            );

            var output = await collectionRepository.Search(searchInput, CancellationToken.None);

            output.Should().NotBeNull();
            output.Items.Should().NotBeNull();
            output.CurrentPage.Should().Be(searchInput.Page);
            output.PerPage.Should().Be(searchInput.PerPage);
            output.Total.Should().Be(expectedTotalResult);
            output.Items.Should().HaveCount(expectedTotalItems);
            foreach (Collection outputItem in output.Items)
            {
                var exampleItem = exampleCollectionList.Find(x => x.Id == outputItem.Id);
                outputItem.Should().NotBeNull();
                outputItem!.Id.Should().Be(exampleItem!.Id);
                outputItem.Name.Should().Be(exampleItem.Name);
                outputItem.Description.Should().Be(exampleItem.Description);
                outputItem.Category.Should().Be(exampleItem.Category);
                outputItem.UserId.Should().Be(exampleItem.UserId);
                outputItem.CreatedAt.Should().BeCloseTo(exampleItem.CreatedAt, TimeSpan.FromSeconds(1));
            }
        }

        [Theory(DisplayName = "SearchOrdered")]
        [Trait("Integration/Infra.Data", "CollectionRepository - Repositories")]
        [InlineData("name", "asc")]
        [InlineData("name", "desc")]
        [InlineData("createdAt", "asc")]
        [InlineData("createdAt", "desc")]
        public async Task SearchOrdered(
            string orderBy,
            string order
        )
        {
            EpicCollectionsDbContext dbContext = _fixture.CreateDbContext();
            var userId = Guid.NewGuid();
            var exampleCollectionList = _fixture.GetExampleCollectionList(userId, 10);
            await dbContext.AddRangeAsync(exampleCollectionList);
            await dbContext.SaveChangesAsync(CancellationToken.None);
            var collectionRepository = new Repository.CollectionRepository(dbContext);
            var searchOrder = order == "asc" ? SearchOrder.Asc : SearchOrder.Desc;
            var searchInput = new SearchInput(
                page: 1,
                perPage: 20,
                search: "",
                orderBy,
                searchOrder
            );

            var output = await collectionRepository.Search(searchInput, CancellationToken.None);

            var expectOrdered = _fixture.SortList(exampleCollectionList, orderBy, searchOrder);

            output.Should().NotBeNull();
            output.Items.Should().NotBeNull();
            output.CurrentPage.Should().Be(searchInput.Page);
            output.PerPage.Should().Be(searchInput.PerPage);
            output.Total.Should().Be(exampleCollectionList.Count);
            output.Items.Should().HaveCount(exampleCollectionList.Count);

            for (int i = 0; i < output.Items.Count; i++)
            {
                var outputItem = output.Items[i];
                var exampleItem = expectOrdered[i];
                outputItem.Should().NotBeNull();
                outputItem!.Id.Should().Be(exampleItem.Id);
                outputItem.Name.Should().Be(exampleItem.Name);
                outputItem.Description.Should().Be(exampleItem.Description);
                outputItem.Category.Should().Be(exampleItem.Category);
                outputItem.UserId.Should().Be(exampleItem.UserId);
                outputItem.CreatedAt.Should().BeCloseTo(exampleItem.CreatedAt, TimeSpan.FromSeconds(1));
            }
        }

        [Fact(DisplayName = "GetCollectionsByUserId")]
        [Trait("Integration/Infra.Data", "CollectionRepository - Repositories")]
        public async Task GetCollectionsByUserId()
        {
            EpicCollectionsDbContext dbContext = _fixture.CreateDbContext();
            var userId = Guid.NewGuid();
            var exampleCollectionList = _fixture.GetExampleCollectionList(userId, 10);
            await dbContext.AddRangeAsync(exampleCollectionList);
            await dbContext.SaveChangesAsync(CancellationToken.None);
            var collectionRepository = new Repository.CollectionRepository(dbContext);

            var collections = await collectionRepository.GetCollectionsByUserId(userId, CancellationToken.None);

            collections.Should().NotBeNull();
            collections.Should().HaveCount(10);
            foreach (var collection in collections)
            {
                collection.UserId.Should().Be(userId);
            }
        }

    }
}
