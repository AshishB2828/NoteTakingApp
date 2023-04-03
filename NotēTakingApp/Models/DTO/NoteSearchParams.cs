namespace NoteTakingApp.Models.DTO
{
    public class NoteSearchParams: NotePageInfo
    {
        public string SearchTitle { get; set; }

        public string SearchTag { get; set; }
    }
}
