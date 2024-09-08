using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace CRUD_1st.Models
{
	public class viewModel
	{
		public User user { get; set; }

		[Required(ErrorMessage = "At least one role must be selected.")]
		public List<int> SelectedIds { get; set; } = new List<int>();
		public List<Role> roles { get; set; } = new List<Role>();
	}
}
