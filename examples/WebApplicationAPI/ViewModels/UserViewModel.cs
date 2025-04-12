using WebApplicationAPI.Domain;

namespace WebApplicationAPI.ViewModels;

public sealed record UserViewModel(
    int Id,
    string Name
)
{
    public static UserViewModel FromUser(User user) => new(user.Id, user.Name);
}
