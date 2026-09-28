namespace ShopTARpe25.Core.Dto
{
    public class FileToApiDto
    {
        public Guid Id { get; set; }

        //see muutuja hakkab näitama, kus asub meie file
        public string? ExistingFilePath { get; set; }
        public Guid? SpaceshipId { get; set; }
    }
}
