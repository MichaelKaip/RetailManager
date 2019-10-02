using System;

/*
 * The aim of this class is to catch the data, which the API sends out.
 */
namespace RMDesktopUI.Library.Models
{
    public class LoggedInUserModel : ILoggedInUserModel
    {
        public string Token { get; set; } 
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAdress { get; set; }
        public DateTime CreatedDate { get; set; }

        public void ResetUserModel()
        {
            Token = "";
            Id = "";
            FirstName = "";
            LastName = "";
            EmailAdress = "";
            CreatedDate = DateTime.MinValue;
        }
    }
}
