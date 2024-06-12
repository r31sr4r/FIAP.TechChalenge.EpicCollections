using FIAP.TechChalenge.EpicCollections.Application.Common;
using FIAP.TechChalenge.EpicCollections.Application.UseCases.User.Common;

namespace FIAP.TechChalenge.EpicCollections.Application.UseCases.User.ListUsers;
public class ListUsersOutput
    : PaginatedListOutput<UserModelOutput>
{
    public ListUsersOutput(
        int page,
        int perPage,
        int total,
        IReadOnlyList<UserModelOutput> items)
        : base(page, perPage, total, items)
    { }
}
