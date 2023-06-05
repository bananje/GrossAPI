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

        public static string ActiveStatusId = "Активно";
        public static string NotActiveStatusId = "Неактивно";

        public const string CustomerRoleId = "84fbc89c-cf76-405b-a0af-4b2a1d9a9ee7";
        public const string AdminRoleId = "b82b722b-e159-49ae-aa78-cb726192bf65";
    }
}
