using HotelBooking.Modules.Entity_Models;
using Microsoft.AspNet.Identity;

public class Role
{
    public int Id { get; set; }
    public string Name { get; set; }

    public ICollection<User> Users { get; set; }
}
