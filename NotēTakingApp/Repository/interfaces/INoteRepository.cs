using NoteTakingApp.Models.DTO;

namespace NoteTakingApp.Repository.interfaces
{
    public interface INoteRepository
    {

        NoteDto CreateNote(NoteDto note, string currentUser);
        NoteDto UpdateNote(NoteDto note, string currentUser);
        NoteDto DeleteNote(int id);
        NoteDto GetNote(int id);
        NoteList GetAllNotes(NoteSearchParams searchParams, string currentUserId, int cpage);
        bool IsUserAllowedToModify(int noteId, string userId);

    }
}
