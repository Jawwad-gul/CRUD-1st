using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace CRUD_1st.Models;

public partial class User
{
	public int Uid { get; set; }

	public string Uname { get; set; } = null!;

	public string? Upass { get; set; }

	public virtual ICollection<Role> Rids { get; set; } = new List<Role>();
}
