using System;
using System.Collections.Generic;

namespace CRUD_1st.Models;

public partial class Role
{
    public int Rid { get; set; }

    public string Rname { get; set; } = null!;

    public virtual ICollection<User> Uids { get; set; } = new List<User>();
}
