namespace OnlineShop.Application.Dtos.JWT
{
    public class DeleteBlacklistTokensAppDto
    {
        public Guid Id { get; set; }
        public string Token { get; set; }
        public DateTime ExpireDate { get; set; }
    }
}
