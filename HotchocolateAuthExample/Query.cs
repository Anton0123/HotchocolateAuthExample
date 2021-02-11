using HotChocolate.AspNetCore.Authorization;

namespace HotchocolateAuthExample
{
    public class Query
    {
        public string Hello => "World";

        [Authorize]
        public string Auth => "Secret";
    }
}
