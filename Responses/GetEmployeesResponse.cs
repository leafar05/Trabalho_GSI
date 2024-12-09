namespace GSI.WebApi.Responses
{
    public class GetEmployeesResponse
    {
        public List<GetEmployeesResponseItem> Items { get; set; } = new List<GetEmployeesResponseItem>();

        public class GetEmployeesResponseItem
        {
            public string Name { get; set; }
        }
    }
}