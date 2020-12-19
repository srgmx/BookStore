using System;
using System.Collections.Generic;

namespace BookStore.Business.Dto
{
    public class UserPermissionsDto
    {
        public UserPermissionsDto()
        {
            Permissions = new List<string>();
        }

        public Guid Id { get; set; }

        public List<string> Permissions { get; set; }
    }
}
