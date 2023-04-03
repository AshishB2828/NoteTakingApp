namespace NoteTakingApp.Models.DTO
{
    public class NoteList: NoteSearchParams
    {
        public List<NoteDto> Notes { get; set; } = new List<NoteDto>();
    }
}
