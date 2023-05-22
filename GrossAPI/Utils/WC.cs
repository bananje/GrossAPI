namespace GrossAPI.Utils
{
    public class WC
    {
        public static string PathPostImage
        {
            get { return (@"\images\PostImages\"); }
        }
        public static string PathReportImage
        {
            get { return (@"\images\ReportImages\"); }
        }
        public const string CustomerRoleId = "84fbc89c-cf76-405b-a0af-4b2a1d9a9ee7";
        public static Guid ActiveStatusId = Guid.Parse("96686a6c-c038-4a7f-9a3b-ae7338d1cd25");
        public static Guid NotActiveStatusId = Guid.Parse("8eceeae3-6d70-4f88-b204-29b025b5c889");
    }
}
