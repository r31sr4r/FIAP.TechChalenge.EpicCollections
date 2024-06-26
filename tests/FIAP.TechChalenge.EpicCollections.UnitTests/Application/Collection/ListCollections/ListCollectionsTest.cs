using FluentAssertions;
using Moq;
using FIAP.TechChalenge.EpicCollections.Application.UseCases.Collection.Common;
using FIAP.TechChalenge.EpicCollections.Domain.SeedWork.SearchableRepository;
using Xunit;
using DomainEntity = FIAP.TechChalenge.EpicCollections.Domain.Entity;
using UseCase = FIAP.TechChalenge.EpicCollections.Application.UseCases.Collection.ListCollections;

namespace FIAP.TechChalenge.EpicCollections.UnitTests.Application.Collection.ListCollections;
[Collection(nameof(ListCollectionsTestFixture))]
public class ListCollectionsTest
{
    private readonly ListCollectionsTestFixture _fixture;

    public ListCollectionsTest(ListCollectionsTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = nameof(ShouldReturnCollections))]
    [Trait("Application", "ListCollections - Use Cases")]
    public async void ShouldReturnCollections()
    {
        var collectionsList = _fixture.GetCollectionsList();
        var repositoryMock = _fixture.GetRepositoryMock();
        var input = _fixture.GetInput();
        var outputRepositorySearch = new SearchOutput<DomainEntity.Collection.Collection>(
            currentPage: input.Page,
            perPage: input.PerPage,
            items: (IReadOnlyList<DomainEntity.Collection.Collection>)collectionsList,
            total: new Random().Next(50, 200)
        );
        repositoryMock.Setup(x => x.Search(
            It.Is<SearchInput>(
                searchInput => searchInput.Page == input.Page &&
                searchInput.PerPage == input.PerPage &&
                searchInput.Search == input.Search &&
                searchInput.OrderBy == input.Sort &&
                searchInput.Order == input.Dir
            ),
            It.IsAny<CancellationToken>()
        )).ReturnsAsync(outputRepositorySearch);

        var useCase = new UseCase.ListCollections(repositoryMock.Object);

        var output = await useCase.Handle(input, CancellationToken.None);

        output.Should().NotBeNull();
        output.Page.Should().Be(outputRepositorySearch.CurrentPage);
        output.PerPage.Should().Be(outputRepositorySearch.PerPage);
        output.Total.Should().Be(outputRepositorySearch.Total);
        output.Items.Should().HaveCount(outputRepositorySearch.Items.Count);
        ((List<CollectionModelOutput>)output.Items).ForEach(outputItem =>
        {
            var repositoryCollection = outputRepositorySearch.Items
                .FirstOrDefault(c => c.Id == outputItem.Id);
            outputItem.Should().NotBeNull();
            outputItem.Name.Should().Be(repositoryCollection!.Name);
            outputItem.Description.Should().Be(repositoryCollection!.Description);
            outputItem.Category.Should().Be(repositoryCollection!.Category);
            outputItem.CreatedAt.Should().Be(repositoryCollection!.CreatedAt);
            outputItem.UserId.Should().Be(repositoryCollection!.UserId);
            outputItem.Id.Should().Be(repositoryCollection!.Id);
        });
        repositoryMock.Verify(x => x.Search(
            It.Is<SearchInput>(
                searchInput => searchInput.Page == input.Page &&
                searchInput.PerPage == input.PerPage &&
                searchInput.Search == input.Search &&
                searchInput.OrderBy == input.Sort &&
                searchInput.Order == input.Dir
            ),
            It.IsAny<CancellationToken>()
            ), Times.Once);
    }

    [Theory(DisplayName = nameof(ListInputWithoutAllParameters))]
    [Trait("Application", "ListCollections - Use Cases")]
    [MemberData(nameof(ListCollectionsTestDataGenerator.GetInputWithoutAllParameters),
        parameters: 18,
        MemberType = typeof(ListCollectionsTestDataGenerator)
        )]
    public async void ListInputWithoutAllParameters(
        UseCase.ListCollectionsInput input
        )
    {
        var collectionsList = _fixture.GetCollectionsList();
        var repositoryMock = _fixture.GetRepositoryMock();
        var outputRepositorySearch = new SearchOutput<DomainEntity.Collection.Collection>(
            currentPage: input.Page,
            perPage: input.PerPage,
            items: (IReadOnlyList<DomainEntity.Collection.Collection>)collectionsList,
            total: new Random().Next(50, 200)
        );
        repositoryMock.Setup(x => x.Search(
            It.Is<SearchInput>(
                searchInput => searchInput.Page == input.Page &&
                searchInput.PerPage == input.PerPage &&
                searchInput.Search == input.Search &&
                searchInput.OrderBy == input.Sort &&
                searchInput.Order == input.Dir
            ),
            It.IsAny<CancellationToken>()
        )).ReturnsAsync(outputRepositorySearch);

        var useCase = new UseCase.ListCollections(repositoryMock.Object);

        var output = await useCase.Handle(input, CancellationToken.None);

        output.Should().NotBeNull();
        output.Page.Should().Be(outputRepositorySearch.CurrentPage);
        output.PerPage.Should().Be(outputRepositorySearch.PerPage);
        output.Total.Should().Be(outputRepositorySearch.Total);
        output.Items.Should().HaveCount(outputRepositorySearch.Items.Count);
        ((List<CollectionModelOutput>)output.Items).ForEach(outputItem =>
        {
            var repositoryCollection = outputRepositorySearch.Items
                .FirstOrDefault(c => c.Id == outputItem.Id);
            outputItem.Should().NotBeNull();
            outputItem.Name.Should().Be(repositoryCollection!.Name);
            outputItem.Description.Should().Be(repositoryCollection!.Description);
            outputItem.Category.Should().Be(repositoryCollection!.Category);
            outputItem.CreatedAt.Should().Be(repositoryCollection!.CreatedAt);
            outputItem.UserId.Should().Be(repositoryCollection!.UserId);
            outputItem.Id.Should().Be(repositoryCollection!.Id);
        });
        repositoryMock.Verify(x => x.Search(
            It.Is<SearchInput>(
                searchInput => searchInput.Page == input.Page &&
                searchInput.PerPage == input.PerPage &&
                searchInput.Search == input.Search &&
                searchInput.OrderBy == input.Sort &&
                searchInput.Order == input.Dir
            ),
            It.IsAny<CancellationToken>()
            ), Times.Once);
    }

    [Fact(DisplayName = nameof(ListOkWhenEmpty))]
    [Trait("Application", "ListCollections - Use Cases")]
    public async void ListOkWhenEmpty()
    {
        var collectionsList = _fixture.GetCollectionsList();
        var input = _fixture.GetInput();
        var repositoryMock = _fixture.GetRepositoryMock();
        var outputRepositorySearch = new SearchOutput<DomainEntity.Collection.Collection>(
            currentPage: input.Page,
            perPage: input.PerPage,
            items: new List<DomainEntity.Collection.Collection>().AsReadOnly(),
            total: 0
        );
        repositoryMock.Setup(x => x.Search(
            It.Is<SearchInput>(
                searchInput => searchInput.Page == input.Page &&
                searchInput.PerPage == input.PerPage &&
                searchInput.Search == input.Search &&
                searchInput.OrderBy == input.Sort &&
                searchInput.Order == input.Dir
            ),
            It.IsAny<CancellationToken>()
        )).ReturnsAsync(outputRepositorySearch);

        var useCase = new UseCase.ListCollections(repositoryMock.Object);

        var output = await useCase.Handle(input, CancellationToken.None);

        output.Should().NotBeNull();
        output.Page.Should().Be(outputRepositorySearch.CurrentPage);
        output.PerPage.Should().Be(outputRepositorySearch.PerPage);
        output.Total.Should().Be(0);
        output.Items.Should().HaveCount(0);

        repositoryMock.Verify(x => x.Search(
            It.Is<SearchInput>(
                searchInput => searchInput.Page == input.Page &&
                searchInput.PerPage == input.PerPage &&
                searchInput.Search == input.Search &&
                searchInput.OrderBy == input.Sort &&
                searchInput.Order == input.Dir
            ),
            It.IsAny<CancellationToken>()
            ), Times.Once);
    }
}
