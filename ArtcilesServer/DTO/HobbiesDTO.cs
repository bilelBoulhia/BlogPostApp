using ArtcilesServer.Models;

namespace ArtcilesServer.DTO
{
    public class HobbiesDTO
    {
       public int userId {  get; set; }
       public List<int> Hobbies { get; set; } 
    }
}
