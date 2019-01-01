
namespace XamarinLinkerIssue {

	public class Contact {

		public int ID { get; set; }
		public string DisplayName { get; set; }

		/**
		 * Contact initialization.
		 */
		public Contact (int id, string displayName) {
			ID = id;
			DisplayName = displayName;
		}

	}
}
